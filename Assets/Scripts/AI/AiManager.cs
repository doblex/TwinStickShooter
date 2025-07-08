using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class AiManager : MonoBehaviour
{
    public static AiManager Instance;

    [SerializeField] List<BehaviourController> controllers = new List<BehaviourController>();
    [SerializeField] int maxChasingController = 1;

    bool isZoneContested;
    bool isTeamCamping;

    public bool IsZoneContested { get => isZoneContested; }
    public bool IsTeamCamping { get => isTeamCamping; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        PointGenerator.Instance.RegisterEvent(OnCapture);
        PointGenerator.Instance.onPointChanged += OnPointChanged;
    }

    private void Update()
    {
        UpdateChecks();
    }

    private void UpdateChecks()
    {
        bool isTeamCamping = false;
        bool isTeamAllinChase = true;

        foreach (BehaviourController controller in controllers)
        {
            if (controller != null)
            {
                if (isTeamAllinChase && controller.CurrentState.stateName != STATE.CHASE)
                    isTeamAllinChase = false;

                if (controller.CurrentState.stateName == STATE.CAMP)
                {
                    isTeamCamping = true;
                }
            }
        }

        if (isTeamAllinChase)
            DefendPoint();

        this.isTeamCamping = isTeamCamping;
        isZoneContested = 
            PointGenerator.Instance.CurrentCapturePoint != null 
            && PointGenerator.Instance.CurrentCapturePoint.CurrentState == CapturePointState.Contended;
    }

    private void DefendPoint()
    {
        int rndIdx = Random.Range(0, controllers.Count);

        controllers[rndIdx].ChangeState(STATE.CAPTURE);
    }

    private void OnCapture(CapturePointState capturePointState)
    {
        if (capturePointState != CapturePointState.AI) return;

        List<BehaviourController> controllersToCheck = controllers.FindAll( (BehaviourController controller) => { return controller.CurrentState.stateName == STATE.CAPTURE; });

        if (controllersToCheck.Count == 0) return; 

        int rndIdx = Random.Range(0, controllersToCheck.Count);

        BehaviourController controller = controllersToCheck[rndIdx];
        controller.ChangeState(STATE.CHASE);

        controllersToCheck.RemoveAt(rndIdx);

        foreach (BehaviourController otherController in controllersToCheck)
        {
            otherController.ChangeState(STATE.CAMP);
        }
    }

    private void OnPointChanged(CapturePoint capturePoint)
    {
        foreach (BehaviourController controller in controllers)
        {
            if (controller != null)
            {
                controller.ChangeState(STATE.CAPTURE);
            }
        }
    }
}

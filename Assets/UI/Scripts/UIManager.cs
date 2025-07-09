using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] HUDController hud;
    [SerializeField] EndGameController endGame;
    [SerializeField] PauseController pause;

    bool isPaused = false;  
    bool isEnded = false;

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
        hud.ShowDoc(true);
        endGame.ShowDoc(false);
        HidePause();

        //Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isEnded)
        {
            ShowPause();
        }
    }

    public void ShowEndGameDoc(bool endGameStatus)
    {
        //Cursor.visible = true;
        isEnded = true;
        hud.ShowDoc(false);
        endGame.ShowDoc(true, endGameStatus);
    }

    public void ShowPause()
    {
        //Cursor.visible = true;
        hud.ShowDoc(false);
        pause.ShowDoc(true);
    }

    public void HidePause()
    {
        //Cursor.visible = false;
        hud.ShowDoc(true);
        pause.ShowDoc(false);
    }
}

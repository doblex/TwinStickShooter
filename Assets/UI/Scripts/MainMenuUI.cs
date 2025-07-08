using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void OnPlay()
    {
        SceneManager.LoadScene("Level");
    }

    public void OnQuit()
    {
        Application.Quit();
# if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseController : DocController
{
    Button resumeBtn;
    Button mainMenuBtn;
    Button quitBtn;

    protected override void SetComponents()
    {
        resumeBtn = Root.Q<Button>("ResumeBtn");
        resumeBtn.clicked += () =>
        {
            UIManager.Instance.HidePause();
            Time.timeScale = 1f;
        };

        mainMenuBtn = Root.Q<Button>("MainMenuBtn");
        mainMenuBtn.clicked += () =>
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        };

        quitBtn = Root.Q<Button>("QuitBtn");
        quitBtn.clicked += () =>
        {
            Time.timeScale = 1f;
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        };
    }

    public override void ShowDoc(bool show)
    {
        base.ShowDoc(show);
        Time.timeScale = show ? 0f : 1f;
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EndGameController : DocController
{
    Label endGameLabel;

    Button mainMenuBtn;
    Button quitBtn;

    protected override void SetComponents()
    {
        endGameLabel = Root.Q<Label>("EndGameLabel");

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

    public void ShowDoc(bool show, bool endGameStatus)
    {
        ShowDoc(show);
        Time.timeScale = show ? 0f : 1f;

        if (endGameStatus)
        {
            endGameLabel.text = "You Win!";
        }
        else
        {
            endGameLabel.text = "You Lose!";
        }
    }
}

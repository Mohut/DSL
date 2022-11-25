using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public void LoadEndScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("EndScreen");
    }

    public void LoadHighscoreScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("HighscoreList");
    }

    public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void LoadQuestionScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("QuestionScreen");
    }

    public void LoadSubjectScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SubjectMenu");
    }
}

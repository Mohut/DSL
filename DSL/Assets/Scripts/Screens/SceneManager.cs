using UnityEngine;

public static class SceneManager
{
    public static void LoadEndScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("EndScreen");
    }

    public static void LoadHighscoreScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("HighscoreList");
    }

    public static void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public static void LoadQuestionScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("QuestionScreen");
    }

    public static void LoadSubjectScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SubjectMenu");
    }
}

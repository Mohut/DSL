using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubjectButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inputField;
    
    [Header("Buttons")]
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    [SerializeField] private Button button3;
    [SerializeField] private Button button4;
    [SerializeField] private Button button5;
    [SerializeField] private Button homebutton;

    private void Start()
    {
        button1.onClick.AddListener(() =>
        {
            DataManager.Instance.AddNewGroup(inputField.text);
            GameManager.Instance.SetCurrentStation(DataManager.Instance.Stations[0]);
            SceneManager.LoadQuestionScreen();
        });

        button2.onClick.AddListener(() =>
        {
            DataManager.Instance.AddNewGroup(inputField.text);
            GameManager.Instance.SetCurrentStation(DataManager.Instance.Stations[1]);
            SceneManager.LoadQuestionScreen();
        });

        button3.onClick.AddListener(() =>
        {
            DataManager.Instance.AddNewGroup(inputField.text);
            GameManager.Instance.SetCurrentStation(DataManager.Instance.Stations[2]);
            SceneManager.LoadQuestionScreen();
        });

        button4.onClick.AddListener(() =>
        {
            DataManager.Instance.AddNewGroup(inputField.text);
            GameManager.Instance.SetCurrentStation(DataManager.Instance.Stations[3]);
            SceneManager.LoadQuestionScreen();
        });
        
        button5.onClick.AddListener(() =>
        {
            DataManager.Instance.AddNewGroup(inputField.text);
            GameManager.Instance.SetCurrentStation(DataManager.Instance.Stations[4]);
            SceneManager.LoadQuestionScreen();
        });
        
        homebutton.onClick.AddListener(SceneManager.LoadMainMenu);
    }

    private void OnDisable()
    {
        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();
        button3.onClick.RemoveAllListeners();
        button4.onClick.RemoveAllListeners();
        button5.onClick.RemoveAllListeners();
        homebutton.onClick.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

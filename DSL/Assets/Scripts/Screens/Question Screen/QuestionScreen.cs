using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionScreen : MonoBehaviour
{
    [Header("Textfields")]
    [SerializeField] private TextMeshProUGUI groupName;
    [SerializeField] private TextMeshProUGUI task;
    [SerializeField] private TextMeshProUGUI subject;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI question;
    [SerializeField] private TextMeshProUGUI answer1;
    [SerializeField] private TextMeshProUGUI answer2;
    [SerializeField] private TextMeshProUGUI answer3;
    [SerializeField] private TextMeshProUGUI answer4;

    [Header("Buttons")]
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    [SerializeField] private Button button3;
    [SerializeField] private Button button4;

    private float time;
    
    void Start()
    {
        groupName.text = GameManager.Instance.CurrentGroup.name;
        subject.text = GameManager.Instance.CurrentStation.name;
        question.text = GameManager.Instance.CurrentQuestion.text;
        
        SetAnswerText();
        SetButtonMethods();
    }

    private void SetAnswerText()
    {
        if(GameManager.Instance.CurrentAnswers.Count >= 1)
            answer1.text = GameManager.Instance.CurrentAnswers[0].text;
        if(GameManager.Instance.CurrentAnswers.Count >= 2)
            answer2.text = GameManager.Instance.CurrentAnswers[1].text;
        if(GameManager.Instance.CurrentAnswers.Count >= 3)
            answer3.text = GameManager.Instance.CurrentAnswers[2].text;
        if(GameManager.Instance.CurrentAnswers.Count >= 4)
            answer4.text = GameManager.Instance.CurrentAnswers[3].text;
    }

    private void SetButtonMethods()
    {
        button1.onClick.AddListener(() =>
        {
            if (GameManager.Instance.SetNextQuestion() == false)
            {
                SceneManager.LoadEndScreen();
                return;
            }
            SetAnswerText();
        });
        button2.onClick.AddListener( () => {
            if (GameManager.Instance.SetNextQuestion() == false)
            {
                SceneManager.LoadEndScreen();
                return;
            }
            SetAnswerText();
        });
        button3.onClick.AddListener(() =>
        {
            if (GameManager.Instance.SetNextQuestion() == false)
            {
                SceneManager.LoadEndScreen();
                return;
            }
            SetAnswerText();
        });
        button4.onClick.AddListener(() =>
        {
            if (GameManager.Instance.SetNextQuestion() == false)
            {
                SceneManager.LoadEndScreen();
                return;
            }
            SetAnswerText();
        });
    }
}

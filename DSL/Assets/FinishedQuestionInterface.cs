using TMPro;
using UnityEngine;
public class FinishedQuestionInterface : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questionNumber;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI answerText;
    [SerializeField] private TextMeshProUGUI tipsUsed;

    public TextMeshProUGUI QuestionNumber { get => questionNumber; set => questionNumber = value; }
    public TextMeshProUGUI QuestionText { get => questionText; set => questionText = value; }
    public TextMeshProUGUI AnswerText { get => answerText; set => answerText = value; }
    public TextMeshProUGUI TipsUsed { get => tipsUsed; set => tipsUsed = value; }
}

using UnityEngine;

public class ChosenAnswer : MonoBehaviour
{
    private int taskNumber;
    private Question question;
    private Answer answer;
    private bool right;
    private bool usedTip;

    public ChosenAnswer(int taskNumber, Question question, Answer answer, bool right, bool usedTip)
    {
        this.taskNumber = taskNumber;
        this.question = question;
        this.answer = answer;
        this.right = right;
        this.usedTip = usedTip;
    }
}

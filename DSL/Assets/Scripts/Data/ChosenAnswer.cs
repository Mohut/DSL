
public class ChosenAnswer
{
    private int taskNumber;
    private Question question;
    private bool right;
    private int usedTip;

    public ChosenAnswer(int taskNumber, Question question, bool right, int usedTip)
    {
        this.taskNumber = taskNumber;
        this.question = question;
        this.right = right;
        this.usedTip = usedTip;
    }
    public int TaskNumber { get => taskNumber; set => taskNumber = value; }
    public Question Question { get => question; set => question = value; }
    public bool Right { get => right; set => right = value; }
    public int UsedTip { get => usedTip; set => usedTip = value; }
}

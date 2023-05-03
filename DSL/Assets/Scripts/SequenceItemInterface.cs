using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SequenceItemInterface : MonoBehaviour
{
    [SerializeField] private Image sequenceImage;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI answerTMP;
    [SerializeField] private TextMeshProUGUI numberTMP;

    public TextMeshProUGUI AnswerTMP { get => answerTMP; set => answerTMP = value; }
    public TextMeshProUGUI NumberTMP { get => numberTMP; set => numberTMP = value; }
    public Image SequenceImage { get => sequenceImage; set => sequenceImage = value; }
    public Button Button { get => button; set => button = value; }
}

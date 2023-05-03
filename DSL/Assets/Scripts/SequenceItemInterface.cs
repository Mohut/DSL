using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SequenceItemInterface : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI answerTMP;
    [SerializeField] private TextMeshProUGUI numberTMP;

    public TextMeshProUGUI AnswerTMP { get => answerTMP; set => answerTMP = value; }
    public TextMeshProUGUI NumberTMP { get => numberTMP; set => numberTMP = value; }
}

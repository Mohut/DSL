using TMPro;
using UnityEngine;

public class SubjectButtonInterface : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI subjectTitleTMP;
    [SerializeField] private RectTransform rectTransform;
    public TextMeshProUGUI SubjectTitleTMP { get => subjectTitleTMP; }
    public RectTransform RectTransform { get => rectTransform; set => rectTransform = value; }
}
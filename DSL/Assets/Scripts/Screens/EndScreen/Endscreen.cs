using TMPro;
using UnityEngine;

public class Endscreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI groupName;
    [SerializeField] private TextMeshProUGUI points;
    [SerializeField] private TextMeshProUGUI tipps;

    private void Start()
    {
        groupName.text = GameManager.Instance.CurrentGroup.name;
        points.text = "Punkte: " + GameManager.Instance.CurrentGroup.points;
        tipps.text = "Tipps: " + GameManager.Instance.UsedTips;
    }
}

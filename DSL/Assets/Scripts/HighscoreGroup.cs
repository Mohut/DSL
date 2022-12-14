using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighscoreGroup : MonoBehaviour
{
    #region Attributes
    [SerializeField] private TextMeshProUGUI groupText;

    private int _rankingNumber;

    public int RankingNumber { get => _rankingNumber; set => _rankingNumber = value; }
    #endregion

    public void Initialize(Group group, int rankingNumber)
    {
        groupText.text = rankingNumber.ToString() + ": " + group.name + " " + group.points;
    }
}

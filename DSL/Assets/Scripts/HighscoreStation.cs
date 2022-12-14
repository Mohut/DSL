using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class HighscoreStation : MonoBehaviour
{
    #region Attributes
    [Header("References")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private GameObject groupUi;
    [SerializeField] private GameObject verticalLayoutGroup;

    private Station _station;
    #endregion

    public void Initialize(Station station)
    {
        titleText.text = station.name;
        _station = station;
    }

    public void CreateHighscoreGroupUi(List<Group> groups)
    {
        List<Group> tmp = new();

        foreach (Group group in GameManager.Instance.PlaysessionsGroups)
        {
            if (group.stationId == _station.id)
            {
                tmp.Add(group);
            }
        }

        // if the group did this station, put it in a temp list
        foreach (Group group in groups)
        {
            if(group.stationId == _station.id)
            {
                tmp.Add(group);
            }
        }
        // order by points
        tmp = tmp.OrderByDescending(x => x.points).ToList();

        for (int i = 0; i < tmp.Count; i++)
        {
            GameObject go = Instantiate(groupUi, verticalLayoutGroup.transform);
            HighscoreGroup highscoreStation = go.GetComponent<HighscoreGroup>();

            highscoreStation.Initialize(tmp[i], i + 1);
        }
    }
}

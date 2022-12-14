using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighscoreScreen : MonoBehaviour
{
    #region Attributes
    [Header("References")]
    [SerializeField] private GameObject highscoreStationUi;

    private List<Group> _groups;
    private List<Station> _stations;
    #endregion

    private IEnumerator Start()
    {
        _groups = DataManager.Instance.Groups;
        _stations = DataManager.Instance.Stations;

        yield return new WaitUntil(() => _stations.Count > 0);

        CreateHighscoreStations(_stations);
    }

    public void CreateHighscoreStations(List<Station> stations)
    {
        foreach (Station station in stations)
        {
            GameObject go = Instantiate(highscoreStationUi, transform);
            HighscoreStation highscoreStation = go.GetComponent<HighscoreStation>();

            highscoreStation.Initialize(station);
            highscoreStation.CreateHighscoreGroupUi(_groups);
        }
    }
}

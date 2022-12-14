using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Group
{
    public string name;
    public int points;
    // the station, for which this group as been created for
    public int stationId;

    public Group(string name, int points)
    {
        this.name = name;
        this.points = points;
    }
}

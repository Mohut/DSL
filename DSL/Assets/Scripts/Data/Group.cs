using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Group
{
    public string name;
    public int points;

    public Group(string name, int points)
    {
        this.name = name;
        this.points = points;
    }
}

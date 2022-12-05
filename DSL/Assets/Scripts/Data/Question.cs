using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Question
{
    public int id;
    public string text;
    public string type;
    public int points;
    public int[] answerId;
    public int hintId;
}

using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Hint
{
    public int id;

    public string text;

    public int price;
}
public class HintMap : ClassMap<Hint>
{
    public HintMap()
    {
        Map(m => m.id).Name("Nummer");
        Map(m => m.text).Name("Tipptext");
        Map(m => m.price).Name("Preis");
    }
}

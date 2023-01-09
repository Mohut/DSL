using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Hint
{
    [Optional]
    [Name("id")]
    public int id;

    [Optional]
    [Name("text")]
    public string text;

    [Optional]
    [Name("price")]
    public int price;
}
public class HintMap : ClassMap<Hint>
{
    public HintMap()
    {
        Map(m => m.id).Name("id");
        Map(m => m.text).Name("text");
        Map(m => m.price).Name("price");
    }
}

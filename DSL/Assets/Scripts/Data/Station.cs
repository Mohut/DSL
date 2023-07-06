using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Station
{
    public int id;

    public string name;

    [TypeConverter(typeof(ToIntArrayConverter))]
    public List<int> questionId { get; set; }
}

public class StationMap : ClassMap<Station>
{
    public StationMap()
    {
        Map(m => m.id).Name("Nummer");
        Map(m => m.name).Name("Name");
        Map(m => m.questionId).Name("Fragennummer").TypeConverter<ToIntArrayConverter>();
    }
}

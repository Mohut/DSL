using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Station
{
    [Name("id")]
    public int id;

    [Name("name")]
    public string name;

    [Name("time")]
    public int time;

    [Name("questionId")]
    [TypeConverter(typeof(ToIntArrayConverter))]
    public List<int> questionId { get; set; }
}

public class StationMap : ClassMap<Station>
{
    public StationMap()
    {
        Map(m => m.id).Name("id");
        Map(m => m.name).Name("name");
        Map(m => m.time).Name("time");
        Map(m => m.questionId).Name("questionId").TypeConverter<ToIntArrayConverter>();
    }
}

using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Question
{
    [Name("id")]
    public int id;

    [Optional]
    [Name("text")]
    public string text;

    [Optional]
    [Name("type")]
    public string type;

    [Optional]
    [Name("points")]
    public int points;

    [Optional]
    [Name("answerId")]
    [TypeConverter(typeof(ToIntArrayConverter))]
    public List<int> answerId{ get; set; }

    [Ignore]
    [Name("hintId")]
    public int hintId;
}
public class QuestionMap : ClassMap<Question>
{
    public QuestionMap()
    {
        Map(m => m.id).Name("id");
        Map(m => m.text).Name("text");
        Map(m => m.type).Name("type");
        Map(m => m.points).Name("points");
        Map(m => m.answerId).Name("answerId").TypeConverter<ToIntArrayConverter>();
    }
}

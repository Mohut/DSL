using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestionType
{
    choice,
    sequence
}

[Serializable]
public class Question
{
    public int id;

    public string text;

    public QuestionType type;

    public int points;

    [TypeConverter(typeof(ToIntArrayConverter))]
    public List<int> answerId{ get; set; }

    public string hintId;
}
public class QuestionMap : ClassMap<Question>
{
    public QuestionMap()
    {
        Map(m => m.id).Name("Nummer");
        Map(m => m.text).Name("Fragetext");
        Map(m => m.type).Name("Fragetyp").TypeConverter<QuestionTypeConverter>();
        Map(m => m.points).Name("Punkte");
        Map(m => m.answerId).Name("Antwortnummer").TypeConverter<ToIntArrayConverter>();
        Map(m => m.hintId).Name("Tippnummer");
    }
}

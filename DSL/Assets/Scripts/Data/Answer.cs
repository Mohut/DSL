using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;

[Serializable]
public class Answer
{
    public int id;

    public string text;

    public bool isCorrect;
}
public class AnswerMap : ClassMap<Answer>
{
    public AnswerMap()
    {
        Map(m => m.id).Name("Nummer");
        Map(m => m.text).Name("Antworttext");
        Map(m => m.isCorrect).Name("Richtigkeit").TypeConverter<BoolConverter>();
    }
}

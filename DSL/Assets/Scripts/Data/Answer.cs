using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;

[Serializable]
public class Answer
{
    [Name("id")]
    public int id;

    [Name("text")]
    public string text;

    [Name("isCorrect")]
    public bool isCorrect;
}
public class AnswerMap : ClassMap<Answer>
{
    public AnswerMap()
    {
        Map(m => m.id).Name("id");
        Map(m => m.text).Name("text");
        Map(m => m.isCorrect).Name("isCorrect").TypeConverter<BoolConverter>();
    }
}

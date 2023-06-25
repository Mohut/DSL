using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections;

[Serializable]
public class Result
{
    [Name("stationId")]
    public string station;
    [Name("questionId")]
    public string question;
    [Name("isCorrect")]
    public bool isCorrect;
    [Name("usedHint")]
    public bool usedHint;
    [Name("groupName")]
    public string groupName;
    [Name("points")]
    public string points;
}

public class ResultMap : ClassMap<Result>
{
    public ResultMap()
    {
        Map(m => m.station).Name("station");
        Map(m => m.question).Name("question");
        Map(m => m.isCorrect).Name("isCorrect");
        Map(m => m.usedHint).Name("usedHint");
        Map(m => m.groupName).Name("groupName");
        Map(m => m.points).Name("points");
    }
}

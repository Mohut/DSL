using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections;

[Serializable]
public class Result
{
    [Name("stationId")]
    public int stationId;
    [Name("questionId")]
    public int questionId;
    [Name("isCorrect")]
    public bool isCorrect;
    [Name("usedHint")]
    public bool usedHint;
    [Name("groupName")]
    public string groupName;
}

public class ResultMap : ClassMap<Result>
{
    public ResultMap()
    {
        Map(m => m.stationId).Name("stationId");
        Map(m => m.questionId).Name("questionId");
        Map(m => m.isCorrect).Name("isCorrect");
        Map(m => m.usedHint).Name("usedHint");
        Map(m => m.groupName).Name("groupName");
    }
}

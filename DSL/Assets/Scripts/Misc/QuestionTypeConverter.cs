using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestionTypeConverter : TypeConverter
{    
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        QuestionType type = QuestionType.choice;

        if (text.ToLower() == "frage/antwort" || text.ToLower() == "0")
        {
            type = QuestionType.choice;
        }
        else if (text.ToLower() == "sequenz" || text.ToLower() == "1")
        {
            type = QuestionType.sequence;
        }
        else
        {
            Debug.LogError("Wrong type convertion from " + text + " to QuestionType Enum. Error in google sheet data. Defaulted to choice type.");
        }

        return type;
    }
}

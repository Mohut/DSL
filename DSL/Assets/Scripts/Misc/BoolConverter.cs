using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoolConverter : TypeConverter
{    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        bool cellBool = true;

        if(text.ToLower() == "wahr" || text.ToLower() == "true")
        {
            cellBool = true;
        }
        else if(text.ToLower() == "falsch" || text.ToLower() == "false")
        {
            cellBool = false;
        }
        else
        {
            Debug.LogError("Wrong type convertion from " + text + " to bool. Error in google sheet data.");
        }

        return cellBool;
    }
}

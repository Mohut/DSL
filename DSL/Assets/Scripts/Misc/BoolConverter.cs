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

        if(text.ToLower() == "wahr")
        {
            cellBool = true;
        }
        else if(text.ToLower() == "falsch")
        {
            cellBool = false;
        }

        return cellBool;
    }
}

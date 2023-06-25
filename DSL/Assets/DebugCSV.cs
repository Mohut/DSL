using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DebugCSV : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    void Start()
    {
        string debug = "";

        if(!Directory.Exists(Application.persistentDataPath))
        {
            debug = "Directory doesnt exist: " + Application.persistentDataPath;
        }
        else if (!File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "Station.csv"))
        {
            debug = "File doesnt exist: " + Application.persistentDataPath + Path.DirectorySeparatorChar + "Station.csv";
        }
        else
        {
            debug = "File and Directory exists.";
        }

        //debug += "\n\n";
        /*
        if (DataManager.Instance.Stations.Count > 0)
        {
            debug += "DataManager has Station data.";
        }
        else
        {
            debug += "DataManager has no data.";
        }
        */

        debug += DataManager.Instance.debug;

        debug += "\n\nFiles:\n";

        foreach (var item in Directory.GetFiles(Application.persistentDataPath))
        {
            debug += item + "\n\n";
        } 

        text.text = debug;
    }

}

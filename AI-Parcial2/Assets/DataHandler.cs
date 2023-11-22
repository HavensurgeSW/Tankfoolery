using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class DataHandler<T> where T : class
{
    public static string directory = "/SavedSimulations/";
    public static string fileName = "BestAgent.txt";

    public static void Save(T savedObject, string team, string generation, string fitness, string foodAte)
    {
        string dir = Application.persistentDataPath + directory + team + "/";

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string[] baseFileName = fileName.Split(".");
        string fullFileName = baseFileName[0] + "_" + team + "_[Gen " + generation + "]_[Fitness " + fitness + "]_[Food Ate " + foodAte + "]." + baseFileName[1];
        string dataSerialized = JsonUtility.ToJson(savedObject);

        File.WriteAllText(dir + fullFileName, dataSerialized);
    }

    public static void Save(T savedObject)
    {
        string dir = Application.persistentDataPath + directory;

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string dataSerialized = JsonUtility.ToJson(savedObject);
        File.WriteAllText(dir + fileName, dataSerialized);
    }

    public static T Load(string teamFrom, string fileName)
    {
        string fullPath = Application.persistentDataPath + directory + teamFrom + "/" + fileName;
        T newObj = null;

        if (File.Exists(fullPath))
        {
            string dataDeserialized = File.ReadAllText(fullPath);
            newObj = JsonUtility.FromJson<T>(dataDeserialized);
        }
        else
        {
            Debug.LogWarning("The file wasn't found");
        }

        return newObj;
    }
}

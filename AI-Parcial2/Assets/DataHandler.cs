using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataHandler : MonoBehaviour
{

    string saveData;
    void SaveToJson() {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(Application.dataPath + "/ArtInt.json", json);
    }

    void LoadFromJson() {
        string json = File.ReadAllText(Application.dataPath + "/ArtInt.json");
        string data = JsonUtility.FromJson<string>(json);
    }
}

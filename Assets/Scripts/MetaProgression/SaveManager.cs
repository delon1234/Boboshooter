using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
// Uses downloaded package Newtonsoft to help with serializing Dictionaries to JSON for Upgrade saving

public static class SaveManager 
{
    // this will form the filepath to where the save.json should go
    private static readonly String SavePath = Path.Combine(Application.persistentDataPath, "save.json");   

    // Retrieves SaveData from MetaData and saves it to filepath
    public static void Save()
    {
        SaveData save = MetaData.GetSaveData();
        // String json = JsonUtility.ToJson(save, true);
        String json = JsonConvert.SerializeObject(save, Formatting.Indented);
        File.WriteAllText(SavePath, json);
    }

    // Reads file from filepath and sends it to MetaData for unpackaging
    public static void Load()
    {
        // save file dont exist, dont need do anything
        if (!File.Exists(SavePath))
        {
            return;
        }

        String json = File.ReadAllText(SavePath);
        // SaveData save = JsonUtility.FromJson<SaveData>(json);
        SaveData save = JsonConvert.DeserializeObject<SaveData>(json);
        MetaData.LoadFromSave(save);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

public class JsonManager  {

    //Q. Is loading the file every single time the best way to do this, can we have it all loaded once and reloaded when needed? (You can use a singleton instead of static for this) 
    //Q. Is this completly fool proof? Jsons are in a public folder, which can be edited by users/developers. Can this handle missing/corrupted files? If not, fix it.

    public static string LoadAllText(string filename)
    {
        //Q. If does not contain .json, or if it does, what is a good way to solve this?
        string startDataFilePath = Path.Combine(Application.streamingAssetsPath, filename);
        string toRet = File.ReadAllText(startDataFilePath);
        if (string.IsNullOrEmpty(toRet))
            Debug.LogError("File was missing or empty: " + Application.streamingAssetsPath + "/" + filename);
        return toRet;
    }

    public static JSONNode LoadJSONNodeFromStreamingAssetFile(string filename)
    {
        return JSON.Parse(LoadAllText(filename));
    }

    public static T LoadJsonFromStreamingAssetFile<T>(string filename)
    {
        return DeserializeJson<T>(LoadAllText(filename));
    }

    public static T DeserializeJson<T>(string jsonText)
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(jsonText);
        }
        catch
        {
            Debug.LogError("Failed to deserialize json node as type " + typeof(T));
            return default(T);
        }
    }


    //public static Dictionary<string,string> CreatePlayerLoadoutDictionary(Dictionary<string, string> defaultDict, JSONNode jnode)
    //{
    //    Dictionary<string, string> newPlayerDict = new Dictionary<string, string>(defaultDict); //Must be careful, these JSON are refs, so be sure to change the ptr, not the value
    //
    //    foreach (KeyValuePair<string, JSONNode> kv in (JSONObject)jnode)
    //        if (!newPlayerDict.ContainsKey(kv.Key))
    //            newPlayerDict.Add(kv.Key, kv.Value);
    //        else
    //            newPlayerDict[kv.Key] = kv.Value;
    //
    //    return newPlayerDict;
    //}

   

    //string fileAsText = File.ReadAllText(filePath);
    //return JsonConvert.DeserializeObject<Dictionary<string, string>>(fileAsText);

    private static T GetValue<T>(string key, Dictionary<string, string> dict)
    {
        if (!dict.ContainsKey(key))
        {
            Debug.LogError("Key not found in dictionary: " + key + " most likely missing from EntryInfo or PlayerLoadout json files");
            return default(T);
        }
                
        if(string.IsNullOrEmpty(dict[key]))
        {
            Debug.LogError("GetValue in json manager detected IsNullOrEmpty for key: " + key);
            return default(T);
        }
        
        try
        {
            return (T)System.Convert.ChangeType(dict[key], typeof(T));
        }
        catch
        {
            Debug.LogError("Could not convert the value : " + dict[key] + " of key: " + key + " in jnode to valuetype: " + typeof(T).ToString());
            return default(T);
        }
    }

    public static Dictionary<string,string> LoadRoundSetupFile(string fileName, bool fromTempFolder)
    {
        string dirPath = Path.Combine(Application.streamingAssetsPath, "RoundSetupSaves");
        if (fromTempFolder)
            dirPath = Path.Combine(dirPath, "Temp");
        string filePath = Path.Combine(dirPath, fileName + ".json");
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);

        if (!File.Exists(filePath))
        {
            Debug.Log("Trying to load file " + fileName + " does not exist at location: " + filePath);
            return new Dictionary<string, string>();
        }
        else
        {
            string fileAsText = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(fileAsText);
        }
    }

    public static void SaveObjectAsJSONToStreamingAsset(object toSave, string filename)
    {
        //Add so if the file exists, it is overwritten (Delete original and make new)
        //Add so if the folder does not exist, it creates it
        //Add so if the name already has .json in it, it does not add .json to the end
        string dirPath = Path.Combine(Application.streamingAssetsPath, filename + ".json");
        string toOut = JsonConvert.SerializeObject(toSave);
        using (StreamWriter sw = File.CreateText(dirPath))
            sw.Write(toOut);
    }
}

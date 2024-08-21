using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager
{
    Dictionary<string, ISave> saveableDict;

    string defaultPath;



    public void Init(string defaultPath = null)
    {
        if(defaultPath == null)
            this.defaultPath = Application.persistentDataPath + "/Data/";
        if(!Directory.Exists(this.defaultPath))
            Directory.CreateDirectory(this.defaultPath);

        if(saveableDict == null)
            saveableDict = new Dictionary<string, ISave>();

        // Init Setting

    }

    public void AddSaveable(string fileName, ISave saveable)
    {
        if (!saveableDict.ContainsKey(fileName))
        {
            saveableDict.Add(fileName, saveable);
            
            if(!File.Exists(defaultPath + fileName))
                saveable.Save(defaultPath + fileName); // make default file
        }
        else
            Debug.LogError("Already exist key.");
    }

    public void RemoveSaveable(string fileName)
    {
        if (saveableDict.ContainsKey(fileName))
            saveableDict.Remove(fileName);
        else
            Debug.LogError("Not exist key.");
    }

    public void Save(string fileName)
    {
        if (saveableDict.ContainsKey(fileName))
            saveableDict[fileName].Save(defaultPath + fileName);
        else
            Debug.LogError("Not exist key.");
    }

    public void Load(string fileName)
    {
        if (saveableDict.ContainsKey(fileName))
            saveableDict[fileName].Load(defaultPath + fileName);
        else
            Debug.LogError("Not exist key.");
    }
}

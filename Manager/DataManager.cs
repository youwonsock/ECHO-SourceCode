using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager
{
    Dictionary<string, ISave> saveableDict;

    string defaultPath;


    /// <summary>
    /// 초기화 함수
    /// </summary>
    /// <param name="defaultPath"></param>
    public void Init(string defaultPath = null)
    {
        if(defaultPath == null)
            this.defaultPath = Application.persistentDataPath + "/Data/";
        if(!Directory.Exists(this.defaultPath))
            Directory.CreateDirectory(this.defaultPath);

        if(saveableDict == null)
            saveableDict = new Dictionary<string, ISave>();

    }

    /// <summary>
    /// Data Manager에 의해 관리되는 saveable 객체를 추가하는 함수
    /// </summary>
    /// <param name="fileName">파일 이름</param>
    /// <param name="saveable">saveable 객체</param>
    public void AddSaveable(string fileName, ISave saveable)
    {
        if (!saveableDict.ContainsKey(fileName))
        {
            saveableDict.Add(fileName, saveable);
            
            if(!File.Exists(defaultPath + fileName))
                saveable.Save(defaultPath + fileName); // make default file
        }
        else
            Debug.LogError("Not exist key. : " + fileName);
    }

    /// <summary>
    /// Data Manager에 의해 관리되는 saveable 객체를 제거하는 함수
    /// </summary>
    /// <param name="fileName"></param>
    public void RemoveSaveable(string fileName)
    {
        if (saveableDict.ContainsKey(fileName))
            saveableDict.Remove(fileName);
        else
            Debug.LogError("Not exist key.");
    }

    /// <summary>
    /// 등록된 Saveable 객체의 Save를 호출하는 함수
    /// </summary>
    /// <param name="fileName"></param>
    public void Save(string fileName)
    {
        if (saveableDict.ContainsKey(fileName))
            saveableDict[fileName].Save(defaultPath + fileName);
        else
            Debug.LogError("Not exist key. : " + fileName);
    }

    /// <summary>
    /// 등록된 Saveable 객체의 Load를 호출하는 함수
    /// </summary>
    /// <param name="fileName"></param>
    public void Load(string fileName)
    {
        if (saveableDict.ContainsKey(fileName))
            saveableDict[fileName].Load(defaultPath + fileName);
        else
            Debug.LogError("Not exist key. : " + fileName);
    }
}

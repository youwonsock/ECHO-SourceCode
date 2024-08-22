using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager
{
    Dictionary<string, ISave> saveableDict;

    string defaultPath;


    /// <summary>
    /// �ʱ�ȭ �Լ�
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
    /// Data Manager�� ���� �����Ǵ� saveable ��ü�� �߰��ϴ� �Լ�
    /// </summary>
    /// <param name="fileName">���� �̸�</param>
    /// <param name="saveable">saveable ��ü</param>
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
    /// Data Manager�� ���� �����Ǵ� saveable ��ü�� �����ϴ� �Լ�
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
    /// ��ϵ� Saveable ��ü�� Save�� ȣ���ϴ� �Լ�
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
    /// ��ϵ� Saveable ��ü�� Load�� ȣ���ϴ� �Լ�
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

using System.IO;
using UnityEngine;

public class SaveManager : SingletonMB<SaveManager>
{
    private static string savePath;

    protected override void Awake()
    {
       base.Awake();
       savePath = Application.persistentDataPath + "/save.json";
        
    }
    

    /// <summary>
    /// Sauvegarde les donn�es
    /// </summary>
    public void SaveGameData(SaveData saveData)
    {
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(savePath, json);
        Debug.Log("### > Game Saved!");
    }

    /// <summary>
    /// R�cup�re les donn�es
    /// </summary>
    public SaveData FetchGameData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            return saveData;
        }
        else
        {
            return SaveData.Default();
        }
    }
}

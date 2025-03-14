using System.IO;
using UnityEngine;

public class SaveManager : SingletonMB<SaveManager>
{
    private static string savePath = Application.persistentDataPath + "/save.json";

    /// <summary>
    /// Sauvegarde les données
    /// </summary>
    public void SaveGameData(SaveData saveData)
    {
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(savePath, json);
        Debug.Log("### > Game Saved!");
    }

    /// <summary>
    /// Récupère les données
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

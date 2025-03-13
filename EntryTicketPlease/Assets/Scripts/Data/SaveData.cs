using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int currentDay;


    public static SaveData Default()
    {
        SaveData data = new();
        data.currentDay = 1;
        return data;
    }
}

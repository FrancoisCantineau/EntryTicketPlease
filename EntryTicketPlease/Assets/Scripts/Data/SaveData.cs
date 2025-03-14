using System;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int currentDay;
    public DateTime currentDate;

    public static SaveData Default()
    {
        SaveData data = new();
        data.currentDay = 1;
        data.currentDate = new DateTime(1984, 4, 4);
        return data;
    }
}

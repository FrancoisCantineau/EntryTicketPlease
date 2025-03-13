using System;
using UnityEngine;

[System.Serializable]
public class RoundData
{
    public int currentDay;


    public static RoundData Default()
    {
        RoundData data = new();
        data.currentDay = 1;
        return data;
    }
}

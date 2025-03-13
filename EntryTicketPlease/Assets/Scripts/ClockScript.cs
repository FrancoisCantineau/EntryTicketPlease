using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClockScript : MonoBehaviour
{
    [SerializeField] int nbSeconds;
    [SerializeField] TextMeshProUGUI affichage;
    private float ratio;
    DateTime time;

    [SerializeField] Date date;


    void Start()
    {
        ratio = 600 / nbSeconds;
        time = new DateTime(date.year, date.month, date.day, 8, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        time = time.AddMinutes(ratio * Time.deltaTime);
        affichage.text = time.ToString("H:mm dd/MM/yyyy");
    }
}

public struct Date
{
    public int year, month, day;
}
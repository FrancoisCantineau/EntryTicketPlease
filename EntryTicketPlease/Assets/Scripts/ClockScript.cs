using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClockScript : MonoBehaviour
{
    [SerializeField] int nbSeconds;
    [SerializeField] int year;
    [SerializeField] int month;
    [SerializeField] int day;
    [SerializeField] TextMeshProUGUI affichage;
    private float ratio;
    DateTime time;
    // Start is called before the first frame update
    void Start()
    {
        ratio = 600 / nbSeconds;
        time = new DateTime(year, month, day, 8, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        time = time.AddMinutes(ratio * Time.deltaTime);
        affichage.text = time.ToString("H:mm dd/MM/yyyy");
    }
}

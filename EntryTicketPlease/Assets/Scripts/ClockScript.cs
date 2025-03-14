using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ClockScript : MonoBehaviour
{
    [SerializeField] int nbSeconds;
    [SerializeField] TextMeshProUGUI affichage;
    [SerializeField] TextMeshProUGUI calandar;
    [SerializeField] int duration;
    [SerializeField] int startHour;
    [SerializeField] DateTime date;
    [SerializeField] int timewarnning;
    public UnityEvent<int> AlmostFinished = new();
    public UnityEvent Finished = new();
    private float ratio;


    public enum Season
    {
        Jan,
        Feb,
        Mar,
        Apr,
        May,
        Jun,
        Jul,
        Aug,
        Sep,
        Oct,
        Nov,
        Dec,
    }


    void Start()
    {
        ratio = (duration * 60) / nbSeconds;
        GameTime = date;
        TimeSpan ts = new TimeSpan(startHour, 0, 0);
        GameTime = GameTime.Date + ts;
        calandar.text = (Season)(GameTime.Month - 1) + "<br>" + GameTime.Day.ToString();
        durationReturn = nbSeconds;
    }

    // Update is called once per frame
    void Update()
    {
        GameTime = GameTime.AddMinutes(ratio * Time.deltaTime);
        affichage.text = GameTime.ToString("H:mm");
        if (GameTime.Hour == startHour + duration - timewarnning)
        {
            AlmostFinished.Invoke(timewarnning);
        }
        if (GameTime.Hour == startHour + duration)
        {
            Finished.Invoke();
        }
    }
    public DateTime GameTime { get; private set; }
    public int durationReturn { get; private set; }
}
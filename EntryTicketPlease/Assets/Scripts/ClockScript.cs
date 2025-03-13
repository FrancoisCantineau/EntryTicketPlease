using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ClockScript : MonoBehaviour
{
    [SerializeField] int nbSeconds;
    [SerializeField] TextMeshProUGUI affichage;
    [SerializeField] int duration;
    [SerializeField] int startHour;
    [SerializeField] DateTime date;
    [SerializeField] int timewarnning;
    public UnityEvent<int> AlmostFinished;
    public UnityEvent Finished;
    private float ratio;
    
   


    void Start()
    {
        ratio = (duration * 60) / nbSeconds;
        GameTime = date;
        TimeSpan ts = new TimeSpan(startHour, 0, 0);
        GameTime = GameTime.Date + ts;
    }

    // Update is called once per frame
    void Update()
    {
        GameTime = GameTime.AddMinutes(ratio * Time.deltaTime);
        affichage.text = GameTime.ToString("H:mm dd/MM/yyyy");
        if(GameTime.Hour == startHour + duration - timewarnning)
        {
            AlmostFinished.Invoke(timewarnning);
        }
        if(GameTime.Hour == startHour + duration)
        {
            Finished.Invoke();
        }
    }
    public DateTime GameTime { get; private set; }
}
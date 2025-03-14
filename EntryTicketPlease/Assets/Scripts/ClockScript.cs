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

    private void OnEnable()
    {
        StartCoroutine(PlayOpening());
    }


    void Start()
    {
        nbSeconds = (int)GameSettings.HourDurationSeconds*duration;
        ratio = (duration * 60) / nbSeconds;
        GameTime = date;
        TimeSpan ts = new TimeSpan(startHour, 0, 0);
        GameTime = GameTime.Date + ts;
        calandar.text = (Season)(GameTime.Month - 1) + "<br>" + GameTime.Day.ToString();
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




    IEnumerator PlayOpening()
    {
        var TargetPosition = transform.position;
        var EnterPosition = new Vector3(TargetPosition.x + 800, TargetPosition.y, TargetPosition.z);
        transform.position = EnterPosition;
        float duration = 1f;
        float time = 0;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(EnterPosition, TargetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
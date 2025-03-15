using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ClockScript : MonoBehaviour
{
    [SerializeField] int nbSeconds;
    [SerializeField] TextMeshProUGUI affichage;
    
    [SerializeField] int duration;
    [SerializeField] int startHour;
    [SerializeField] int endHour;
    int timewarnning = 2+1;
    bool isTimeLow = false;
    public UnityEvent<int> AlmostFinished = new();
    public UnityEvent Finished = new();
    private float ratio;

    [SerializeField] GameObject timesUpObject;
    [SerializeField] GameObject timeLowObject;

    public DateTime GameTime { get; private set; }

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
        

        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        while (GameTime.Hour < endHour)
        {
            yield return null;
            GameTime = GameTime.AddMinutes(ratio * Time.deltaTime);
            affichage.text = GameTime.ToString("H:mm");
            if (GameTime.Hour > endHour - timewarnning && !isTimeLow)
            {
                isTimeLow = true;
                //timeLowObject?.SetActive(true);
                AlmostFinished.Invoke(timewarnning);
            }
        }
        Finished.Invoke();
        //timesUpObject.SetActive(true);
    }




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
    public int durationReturn { get; private set; } = 8;
}
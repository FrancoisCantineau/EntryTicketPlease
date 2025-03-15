using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using static ClockScript;

public class DiegeticsManagers : MonoBehaviour
{

    [SerializeField] TextMeshPro calandar;
    [SerializeField] TextMeshPro section;

    public DateTime GameTime { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        var roundData = GameManager.CurrentRoundData;
        
        GameTime = roundData.currentDate;
        calandar.text = (Season)(GameTime.Month - 1) + "<br>" + GameTime.Day.ToString();
        calandar.ForceMeshUpdate(true);
        Debug.LogWarning(GameTime.ToString("F"));


        switch (roundData.closedSection)
        {
            case ClosedSection.A:
                section.text = "A";
                break;
            case ClosedSection.B:
                section.text = "B";
                break;
            case ClosedSection.C:
                section.text = "C";
                break;
            case ClosedSection.N:
                section.text = "-";
                break;
        }
        section.ForceMeshUpdate(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

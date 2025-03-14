using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements.Experimental;


public class RoundOpening : MonoBehaviour
{
    #region VARIABLES ----------------------------------------------------------------

    [SerializeField] TextMeshProUGUI m_dayNumber;
    [SerializeField] GameObject m_thougtsGrid;
    [SerializeField] GameObject m_thoughtChipPrefab;
    [SerializeField] GameObject m_StartSignal;

    List<string> thoughts;

    public UnityEvent m_OnOpeningEnd = new UnityEvent();


    #endregion
    #region LIFECYCLE ----------------------------------------------------------------

    private void OnEnable()
    {
        SaveData saveData = SaveManager.Instance.FetchGameData();
        SetDay(saveData.currentDay);

        FetchThoughts();

        StartCoroutine(PlayOpening());
    }

    private void OnDisable()
    {
        m_StartSignal.SetActive(true);
        m_OnOpeningEnd.Invoke();
        m_OnOpeningEnd.RemoveAllListeners();
    }

    #endregion
    #region API ----------------------------------------------------------------------

    #endregion
    #region METHODS ------------------------------------------------------------------

    void FetchThoughts()
    {
        thoughts = new List<string>();
        RoundData roundData = GameManager.CurrentRoundData;
        RoundData defaults = RoundData.Default(roundData.currentDate);

        if (roundData.startingHour < defaults.startingHour)
        {
            thoughts.Add("We had to start early today...");
        }
        else if(roundData.startingHour > defaults.startingHour)
        {
            thoughts.Add("I arrived late today...");
        }

        if(roundData.endingHour < defaults.endingHour)
        {
            thoughts.Add("We'll have to close early...");
        }
        else if (roundData.endingHour > defaults.endingHour)
        {
            thoughts.Add("I have to work overtime today...");
        }

        if(roundData.closedSection != ClosedSection.N)
        {
            thoughts.Add("A section of the park is closed today...");
        }
    }

    void SetDay(int _day)
    {
        m_dayNumber.text = _day.ToString("D2");
    }

    void AddThought(string _thought)
    {
        ThoughtChip thoughtChip = Instantiate(m_thoughtChipPrefab.gameObject, m_thougtsGrid.transform).GetComponent<ThoughtChip>();
        thoughtChip.SetText(_thought);
        thoughtChip.transform.SetParent(m_thougtsGrid.transform);
    }

    IEnumerator PlayOpening()
    {
        var CenterPosition = transform.position;
        var EnterPosition = new Vector3(CenterPosition.x - 1300, CenterPosition.y, CenterPosition.z);
        transform.position = EnterPosition;
        float duration = 1f;
        float time = 0;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(EnterPosition, CenterPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        time += Time.deltaTime;
        foreach (string thought in thoughts)
        {
            AddThought(thought);
            time += 0.5f;
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(6f-time);

        var ExitPosition = new Vector3(CenterPosition.x + 1300, CenterPosition.y, CenterPosition.z);
        while (time < duration)
        {
            transform.position = Vector3.Lerp(CenterPosition, ExitPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = CenterPosition;
        this.gameObject.SetActive(false);
    }
    #endregion
    #region EVENTS -------------------------------------------------------------------

    #endregion
}




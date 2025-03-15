using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RoundManager : SingletonMB<RoundManager>
{
    #region VARIABLES ----------------------------------------------------------------

    [SerializeField] RoundOpening m_roundOpening;
    [SerializeField] ClockScript m_clock;

    public UnityEvent<RoundData> OnStartRound { get; private set; } = new();

    #endregion
    #region LIFECYCLE ----------------------------------------------------------------

    private void OnEnable()
    {
        Debug.Log("RoundManagerEnabled");


        if (GameManager.Instance != null)
        {

            GameManager.Instance.BeginRound.AddListener(OpenRound);
        }
        else
        {
            Debug.LogWarning("GameManager.Instance is null. Cannot add listener to BeginRound.");
        }

        

    }

    private void OnDisable()
    {

        if (GameManager.Instance != null)
        {

            GameManager.Instance.BeginRound.RemoveListener(OpenRound);
        }
        else
        {
            Debug.LogWarning("GameManager.Instance is null. Cannot add listener to BeginRound.");
        }

       

    }

    #endregion
    #region API ----------------------------------------------------------------------


    #endregion
    #region METHODS ------------------------------------------------------------------

    void OpenRound(RoundData roundData)
    {
        m_roundOpening.gameObject.SetActive(true);
        m_roundOpening.m_OnOpeningEnd.AddListener(StartRound);
    }

    void StartRound()
    {
        m_clock.gameObject.SetActive(true);
        OnStartRound.Invoke(GameManager.CurrentRoundData);
    }

    #endregion
    #region EVENTS

    #endregion
}


 #region VARIABLES ----------------------------------------------------------------


#endregion
#region LIFECYCLE ----------------------------------------------------------------

#endregion
#region API ----------------------------------------------------------------------


#endregion
#region METHODS ------------------------------------------------------------------


#endregion
#region EVENTS -------------------------------------------------------------------

#endregion 

using System.IO;
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
        GameManager.Instance.BeginRound.AddListener(OpenRound);
    }

    private void OnDisable()
    {
        GameManager.Instance.BeginRound.RemoveListener(OpenRound);
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

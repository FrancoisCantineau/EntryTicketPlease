using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class RoundManager : SingletonMB<RoundManager>
{
    #region VARIABLES ----------------------------------------------------------------

    [SerializeField] RoundOpening m_roundOpening;

    #endregion
    #region LIFECYCLE ----------------------------------------------------------------

    private void OnEnable()
    {
        Debug.Log("RoundManagerEnabled");

        if (GameManager.Instance != null)
        {

            GameManager.Instance.BeginRound.AddListener(StartRound);
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

            GameManager.Instance.BeginRound.RemoveListener(StartRound);
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

    void StartRound(RoundData roundData)
    {
        m_roundOpening.gameObject.SetActive(true);
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

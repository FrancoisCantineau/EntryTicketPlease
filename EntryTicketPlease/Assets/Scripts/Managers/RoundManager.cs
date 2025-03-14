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
        GameManager.Instance.BeginRound.AddListener(StartRound);
    }

    private void OnDisable()
    {
        GameManager.Instance.BeginRound.RemoveListener(StartRound);
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

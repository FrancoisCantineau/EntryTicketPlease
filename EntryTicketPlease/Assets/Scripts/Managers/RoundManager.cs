using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class RoundManager : SingletonMB<RoundManager>
{
    #region VARIABLES ----------------------------------------------------------------

    [SerializeField] RoundOpening m_roundOpening;
    [SerializeField] ClockScript m_clock;

    [SerializeField] bool roundEnded;


    [SerializeField] private int succeededVisitors;
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
        succeededVisitors = 0;
        //m_clock.gameObject.SetActive(true);
        
        
    }

    void WinCheck()
    {
        roundEnded = true;
        int queueSize = VisitorsManager.Instance.GetQueueSize();
        if (queueSize == 0)
        {
            Debug.LogWarning("La file de visiteurs est vide !");
            return;
        }

        int percentage = (succeededVisitors * 100) / queueSize;

       
        int starsAmount = 0;
        if (percentage == 100) starsAmount = 3; 
        else if (percentage >= 70) starsAmount = 2;  
        else if (percentage >= 30) starsAmount = 1;  
        else starsAmount = 0;  

        Debug.Log($"Performance : {percentage}% - Étoiles obtenues : {starsAmount}");
    }

    public void EndOfVisitor(bool isAllowed)
    {
        if (roundEnded) return;

      
        bool isValid = VisitorsManager.Instance.CheckValidityCurrentVisitor();
        Debug.Log( isValid );
        if (isValid == isAllowed)
        {
            succeededVisitors += 1;
        }

        bool roundContinue =VisitorsManager.Instance.NextVisitor();

        if (!roundContinue)
        {
            WinCheck();
        }
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

using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class RoundManager : SingletonMB<RoundManager>
{
    #region VARIABLES ----------------------------------------------------------------

    [SerializeField] RoundOpening m_roundOpening;
    [SerializeField] GameObject m_clock;
    [SerializeField] ClockScript m_clockScript;

    [SerializeField] WinLoseText winLoseText;


    [SerializeField] bool roundEnded;


    [SerializeField] private int succeededVisitors;
    public UnityEvent<RoundData> OnStartRound { get; private set; } = new();

    #endregion
    #region LIFECYCLE ----------------------------------------------------------------
    void Start()
    {

    }
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

        m_clock.transform.GetChild(0).gameObject.SetActive(true);

        if (m_clockScript != null)
        {
            m_clockScript.Finished.AddListener(OnClockFinished);
        }
    }

    void OnClockFinished()
    {
        WinCheck(false);
    }

    void WinCheck(bool needCalculus)
    {
        bool isWin;
        int starsAmount = 0;

        roundEnded = true;

        if (needCalculus)
        {

            isWin = true;

            int queueSize = VisitorsManager.Instance.GetQueueSize();

            int percentage = (succeededVisitors * 100) / queueSize;


            if (percentage == 100) starsAmount = 3;
            else if (percentage >= 70) starsAmount = 2;
            else if (percentage >= 30) starsAmount = 1;
            else
            {
                starsAmount = 0;
                isWin = false;
            }

            Debug.Log($"Performance : {percentage}% - Étoiles obtenues : {starsAmount}");
        }
        else
        {
            isWin = false;
        }

        winLoseText.displayEndDayUI(isWin, starsAmount);

        GameManager.Instance.OnRoundTerminated();

    }

    public void EndOfVisitor(bool isAllowed)
    {
        if (roundEnded) return;


        bool isValid = VisitorsManager.Instance.CheckValidityCurrentVisitor();
        Debug.Log(isValid);
        if (isValid == isAllowed)
        {
            succeededVisitors += 1;
        }

        bool roundContinue = VisitorsManager.Instance.NextVisitor();

        if (!roundContinue)
        {
            WinCheck(true);
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

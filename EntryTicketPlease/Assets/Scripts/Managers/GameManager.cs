using UnityEngine;
using UnityEngine.Events;

public class GameManager : SingletonMB<GameManager>
{
    #region VARIABLES ----------------------------------------------------------------
    SaveData currentData;
    RoundData roundData;
    public static RoundData CurrentRoundData { get => Instance.roundData; }
    
    #endregion
    #region LIFECYCLE ----------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion
    #region API -------------------------------------------------------------------------

    public void NextDay()
    {
        Debug.Log("Next Day");
    }

    public void RestartDay()
    {
        Debug.Log("Restart Day");
    }

    #endregion
    #region METHODS ---------------------------------------------------------------------

    void InitGame()
    {
        currentData = SaveManager.Instance.FetchGameData();
        GenerateRound(currentData.currentDay);
    }

    void GenerateRound(int currentDay)
    {

    }
    #endregion
    #region EVENTS

    public UnityEvent<RoundData> BeginRound { get; private set; } = new();
    public UnityEvent 

    #endregion

}

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMB<GameManager>
{
    #region VARIABLES ----------------------------------------------------------------
    SaveData currentData;
    RoundData roundData;
    public static RoundData CurrentRoundData { get => Instance.roundData; }

    [SerializeField] ClockScript m_clock;

    #endregion
    #region LIFECYCLE ----------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        currentData = SaveManager.Instance.FetchGameData();
        InitGame();

    }

    private void OnEnable()
    {
        m_clock.Finished.AddListener(OnClockFinished);
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
        VerificationAlgo.UpdateAlgorithm(roundData);
        VisitorsManager.Instance.CreateQueue(roundData.queueLength);

        BeginRound.Invoke(roundData);
    }

    void OnClockFinished()
    {
        EndRound.Invoke();
        SceneManager.LoadSceneAsync("endDayScene", LoadSceneMode.Additive);
    }


    void GenerateRound(int currentDay)
    {
        roundData = RoundData.Default(currentData.currentDate);
        Random.InitState(10);

        // Modifier occasionnellement l'heure de départ et l'heure de fin
        bool changeBegin = Random.Range(0f, 1f) < 0.2f;
        if (changeBegin)
        {
            roundData.startingHour += Random.Range(-1, 3);
        }
        bool changeEnd = Random.Range(0f, 1f) < 0.2f;
        if (changeEnd)
        {
            roundData.endingHour += Random.Range(-1, 3);
        }
        roundData.shiftLength = roundData.endingHour - roundData.startingHour;


        roundData.queueLength = Mathf.RoundToInt(roundData.shiftLength * GameSettings.QueueLengthByShiftLengthFactor);

        // Gérer les incohérences et la fraude
        roundData.incoherencesOdds = Random.Range(0f, .8f); 
        roundData.fraudOdds = Random.Range(0f, .4f);

        // // // Gérer les règles supplémentaires (Notice) // // //


        // Limite de taille minimale
        roundData.notice.heightLimitEnabled = Random.Range(0f, 1f) < 0.5f;
        if (roundData.notice.heightLimitEnabled)
        {
            roundData.notice.heightLimit = Random.Range(GameSettings.RulerMinHeightCm, GameSettings.RulerMaxHeightCm);
        }

        // extension de validité
        roundData.notice.validityExtensionEnabled = Random.Range(0f, 1f) < 0.3f; 
        if (roundData.notice.validityExtensionEnabled)
        {
            roundData.notice.validityExtensionDays = Random.Range(7, 90); // Extension aléatoire entre 1 et 15 jours
        }

        // interdiction des enfants
        roundData.notice.kidsAreForbidenEnabled = Random.Range(0f, 1f) < 0.2f;

        // Limite d'âge maximal
        roundData.notice.maxAgeRestrictionEnabled = Random.Range(0f, 1f) < 0.2f; 
        if (roundData.notice.maxAgeRestrictionEnabled)
        {
            roundData.notice.maxAgeRestriction = Random.Range(18, 60);
        }

        // Limite de poids maximal
        roundData.notice.maxWeightRestrictionEnabled = Random.Range(0f, 1f) < 0.2f;
        if (roundData.notice.maxWeightRestrictionEnabled)
        {
            roundData.notice.maxWeightRestriction = Random.Range(70, 110);
        }

        // Limite de poids minimal
        roundData.notice.minWeightRestrictionEnabled = Random.Range(0f, 1f) < 0.2f;
        if (roundData.notice.minWeightRestrictionEnabled)
        {
            roundData.notice.minWeightRestriction = Random.Range(50, 80);
        }

        // Heure de fermeture pour les enfants
        roundData.notice.kidsNotAllowedAfterHourEnabled = Random.Range(0f, 1f) < 0.1f;
        if (roundData.notice.kidsNotAllowedAfterHourEnabled)
        {
            roundData.notice.kidsNotAllowedAfterHour = Random.Range(12, 18);
        }

        //Prix pour enfants
        roundData.priceGrid.childrenPriceModifEnabled = Random.Range(0f, 1f) < 0.2f;
        if (roundData.priceGrid.childrenPriceModifEnabled)
        {
            roundData.priceGrid.childrenPrice = Random.Range(8, 12);
        }

        //Prix pour ados
        roundData.priceGrid.teensPriceModifEnabled = Random.Range(0f, 1f) < 0.2f;
        if (roundData.priceGrid.teensPriceModifEnabled)
        {
            roundData.priceGrid.teensPrice = Random.Range(13, 17);
        }

        //Prix pour adultes
        roundData.priceGrid.adultsPriceModifEnabled = Random.Range(0f, 1f) < 0.2f;
        if (roundData.priceGrid.adultsPriceModifEnabled)
        {
            roundData.priceGrid.adultsPrice = Random.Range(18, 22);
        }

        // Gérer la fermeture d'une section
        roundData.closedSection = (ClosedSection)Random.Range(0, 4);
    }


    #endregion
    #region EVENTS

    public UnityEvent<RoundData> BeginRound { get; private set; } = new();
    public UnityEvent EndRound { get; private set; } = new();

    #endregion

}

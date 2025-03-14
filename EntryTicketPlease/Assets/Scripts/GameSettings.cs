using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameSettings : SingletonMB<GameSettings>
{
    #region VARIABLES ----------------------------------------------------------------
    [Header("Notice settings")]
    [SerializeField] int rulerMinHeightCm = 110;
    [SerializeField] int rulerMaxHeightCm = 200;

    [Header("Visitor settings")]
    [SerializeField] int visitorMinHeight = 100;
    [SerializeField] int visitorMaxHeight = 220;
    [SerializeField] int visitorMinWeight = 30;
    [SerializeField] int visitorMaxWeight = 120;
    [SerializeField] int childrensMinAge = 5;
    [SerializeField] int childrensMaxAge = 12;
    [SerializeField] int teensMinAge = 13;
    [SerializeField] int teensMaxAge = 17;
    [SerializeField] int adultsMinAge = 18;
    [SerializeField] int adultsMaxAge = 80;

    [Header("RoundSettings")]
    [SerializeField] float queueLengthByShiftLengthFactor = 1.5f;
    [SerializeField] float hourDurationSeconds = 12f;
    #endregion

    public static int RulerMinHeightCm { get => Instance.rulerMinHeightCm; }
    public static int RulerMaxHeightCm { get => Instance.rulerMaxHeightCm; }

    public static int VisitorMinHeight { get => Instance.visitorMinHeight; }
    public static int VisitorMaxHeight { get => Instance.visitorMaxHeight; }

    public static int VisitorMinWeight { get => Instance.visitorMinWeight; }
    public static int VisitorMaxWeight { get => Instance.visitorMaxWeight; }


    public static int ChildrensMinAge { get => Instance.childrensMinAge; }
    public static int ChildrensMaxAge { get => Instance.childrensMaxAge; }
    public static int TeensMinAge { get => Instance.teensMinAge; }
    public static int TeensMaxAge { get => Instance.teensMaxAge; }
    public static int AdultsMinAge { get => Instance.adultsMinAge; }
    public static int AdultsMaxAge { get => Instance.adultsMaxAge; }

    public static float QueueLengthByShiftLengthFactor { get => Instance.queueLengthByShiftLengthFactor; }


    public static Dictionary<(int min_age, int max_age), int> priceTable = new Dictionary<(int, int), int>()
    {     
        {(5,12), 10},    // Tarif enfant
        {(13, 17), 15},   // Tarif ado
        {(18, 100), 20},   // Tarif adulte
    };
    



    public static float HourDurationSeconds { get => Instance.hourDurationSeconds; }


    #region LIFECYCLE ----------------------------------------------------------------
    // Start is called before the first frame update

    #endregion
    #region METHODS ---------------------------------------------------------------------



    #endregion
    #region EVENTS
    #endregion

}

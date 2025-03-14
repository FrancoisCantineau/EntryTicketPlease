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
    
    [Header("RoundSettings")]
    [SerializeField] float queueLengthByShiftLengthFactor = 1.5f;
    
    #endregion

    public static int RulerMinHeightCm { get => Instance.rulerMinHeightCm; }
    public static int RulerMaxHeightCm { get => Instance.rulerMaxHeightCm; }

    public static int VisitorMinHeight { get => Instance.visitorMinHeight; }
    public static int VisitorMaxHeight { get => Instance.visitorMaxHeight; }

    public static int VisitorMinWeight { get => Instance.visitorMinWeight; }
    public static int VisitorMaxWeight { get => Instance.visitorMaxWeight; }

    public static float QueueLengthByShiftLengthFactor { get => Instance.queueLengthByShiftLengthFactor; }


    #region LIFECYCLE ----------------------------------------------------------------
    // Start is called before the first frame update

    #endregion
    #region METHODS ---------------------------------------------------------------------



    #endregion
    #region EVENTS
    #endregion

}

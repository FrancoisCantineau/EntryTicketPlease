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
    #endregion

    public static int RulerMinHeightCm { get => Instance.rulerMinHeightCm; }
    public static int RulerMaxHeightCm { get => Instance.rulerMaxHeightCm; }


    #region LIFECYCLE ----------------------------------------------------------------
    // Start is called before the first frame update

    #endregion
    #region METHODS ---------------------------------------------------------------------



    #endregion
    #region EVENTS
    #endregion

}

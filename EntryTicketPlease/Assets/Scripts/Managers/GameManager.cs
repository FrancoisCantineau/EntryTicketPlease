using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region VARIABLES ----------------------------------------------------------------
    SaveData currentData;
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
    #region METHODS ---------------------------------------------------------------------

    void InitGame()
    {
        currentData = SaveManager.Instance.FetchGameData();
    }

    #endregion
    #region EVENTS
    #endregion

}

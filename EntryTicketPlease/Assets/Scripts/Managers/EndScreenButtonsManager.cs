using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenButtonsManager : MonoBehaviour
{
    #region VARIABLES ----------------------------------------------------------------

    #endregion
    #region LIFECYCLE ----------------------------------------------------------------

    #endregion
    #region API ----------------------------------------------------------------------

    public void RetryClicked()
    {
        Destroy(SaveManager.Instance);
        SceneManager.LoadScene("GameLucie", LoadSceneMode.Single);
    }
    
    public void NextDayClicked()
    {
        var data = SaveManager.Instance.FetchGameData();
        data.currentDay++;
        data.currentDate = data.currentDate.AddDays(1);
        SaveManager.Instance.SaveGameData(data);

        SceneManager.LoadScene("GameLucie", LoadSceneMode.Single);
    }

    #endregion
    #region METHODS ------------------------------------------------------------------


    #endregion
    #region EVENTS -------------------------------------------------------------------

    #endregion
}

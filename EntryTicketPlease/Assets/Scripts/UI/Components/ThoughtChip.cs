using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class ThoughtChip : MonoBehaviour
{
    #region VARIABLES ----------------------------------------------------------------

    string m_thought = "The developers of this game forgot to fill this text";

    #endregion
    #region LIFECYCLE ----------------------------------------------------------------

    private void OnEnable()
    {
        StartCoroutine(ChipLifeCycle());
    }

    #endregion
    #region API ----------------------------------------------------------------------

    public void SetText(string _thought)
    {
        m_thought = _thought;
        GetComponentInChildren<TextMeshProUGUI>().text = m_thought;
    }

    #endregion
    #region METHODS ------------------------------------------------------------------

    IEnumerator ChipLifeCycle()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    #endregion
    #region EVENTS -------------------------------------------------------------------

    #endregion
}




using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements.Experimental;


public class PopUpAndOut : MonoBehaviour
{
    #region VARIABLES ----------------------------------------------------------------

    float baseScale;

    #endregion
    #region LIFECYCLE ----------------------------------------------------------------

    private void Awake()
    {
        baseScale = transform.localScale.x;
    }

    private void OnEnable()
    {
       StartCoroutine(DoPopUpAndOut());
    }


    #endregion
    #region API ----------------------------------------------------------------------

    #endregion
    #region METHODS ------------------------------------------------------------------

    IEnumerator DoPopUpAndOut()
    {
        // uses a Bounce Easing Function to animate the scale of the object. The animation goes from 0 to baseScale in 1 second, then wait 1 second, then goes from baseScale to 0 in 0.8 seconds, then desactivate object

        float t = 0;
        float duration = 1;
        while (t < duration)
        {
            t += Time.deltaTime;
            float scale = Mathf.Lerp(0, baseScale, Easing.OutBounce(t/duration));
            transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
        yield return null;
        yield return new WaitForSeconds(0.3f);
        t = 0;
        duration = 0.4f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float scale = Mathf.Lerp(baseScale, 0, Easing.InBack(t / duration));
            transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
        gameObject.SetActive(false);

    }

    #endregion
    #region EVENTS -------------------------------------------------------------------

    #endregion
}




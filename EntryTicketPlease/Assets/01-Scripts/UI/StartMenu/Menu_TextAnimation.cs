using System.Collections;
using UnityEngine;
using TMPro;

public class Menu_TextAnimation : MonoBehaviour
{
    public TextMeshProUGUI uiText;
    public string fullText;
    public float fadeInDuration = 1.5f;
    public float letterDelay = 0.05f;

    void Start()
    {
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        uiText.text = "";
        uiText.color = new Color(uiText.color.r, uiText.color.g, uiText.color.b, 0);

        for (int i = 0; i < fullText.Length; i++)
        {
            uiText.text += fullText[i];
            float elapsedTime = 0;
            while (elapsedTime < fadeInDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Clamp01(elapsedTime / fadeInDuration);
                uiText.color = new Color(uiText.color.r, uiText.color.g, uiText.color.b, alpha);
                yield return null;
            }
            yield return new WaitForSeconds(letterDelay);
        }
    }
}

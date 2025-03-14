using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private GameObject transitionPanel;
    [SerializeField] private float transitionDuration = 1f;
    [SerializeField] private Button transitionButton;
    [SerializeField] private string nextSceneName;

    void Start()
    {
        if (transitionButton != null)
        {
            transitionButton.onClick.AddListener(() => LoadNextLevel(nextSceneName));
        }
    }

    public void LoadNextLevel(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    private IEnumerator FadeIn()
    {
        transitionPanel.SetActive(true);
        CanvasGroup canvasGroup = transitionPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = transitionPanel.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 1;
        canvasGroup.DOFade(0, transitionDuration);
        yield return new WaitForSeconds(transitionDuration);
        transitionPanel.SetActive(false);
    }

    private IEnumerator FadeOut(string sceneName)
    {
        transitionPanel.SetActive(true);
        CanvasGroup canvasGroup = transitionPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = transitionPanel.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, transitionDuration);
        yield return new WaitForSeconds(transitionDuration);
        SceneManager.LoadScene(sceneName);
    }

    public void ActivatePanel()
    {
        StartCoroutine(FadeIn());
    }
}

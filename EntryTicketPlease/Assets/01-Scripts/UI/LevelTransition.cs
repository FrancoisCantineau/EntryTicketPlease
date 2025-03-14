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
        CanvasGroup canvasGroup = GetOrAddCanvasGroup();
        canvasGroup.alpha = 1;
        canvasGroup.DOFade(0, transitionDuration).SetUpdate(true);
        yield return new WaitForSecondsRealtime(transitionDuration);
        transitionPanel.SetActive(false);
    }

    private IEnumerator FadeOut(string sceneName)
    {
        transitionPanel.SetActive(true);
        CanvasGroup canvasGroup = GetOrAddCanvasGroup();
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, transitionDuration).SetUpdate(true);
        yield return new WaitForSecondsRealtime(transitionDuration);
        Time.timeScale = 1f; // Assure que la transition fonctionne même en pause
        SceneManager.LoadScene(sceneName);
    }

    private CanvasGroup GetOrAddCanvasGroup()
    {
        CanvasGroup canvasGroup = transitionPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = transitionPanel.AddComponent<CanvasGroup>();
        }
        return canvasGroup;
    }

    public void ActivatePanel()
    {
        StartCoroutine(FadeIn());
    }
}
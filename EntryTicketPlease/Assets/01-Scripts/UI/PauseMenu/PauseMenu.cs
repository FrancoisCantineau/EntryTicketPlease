using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private float animationDuration = 0.3f;

    private bool isPaused = false;

    void Start()
    {
        pausePanel.transform.localScale = Vector3.one; // Assure que la taille est correcte au départ
        pausePanel.SetActive(false);
        closeButton.onClick.AddListener(TogglePause);
        pauseButton.onClick.AddListener(TogglePause);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            pausePanel.SetActive(true);
            pauseButton.gameObject.SetActive(false);
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0f, animationDuration).SetUpdate(true);
            pausePanel.transform.localScale = Vector3.zero; // Assure que l'animation part de zéro
            pausePanel.transform.DOScale(1f, animationDuration).SetEase(Ease.OutBack).SetUpdate(true);
        }
        else
        {
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, animationDuration).SetUpdate(true);
            pausePanel.transform.DOScale(0f, animationDuration).SetEase(Ease.InBack).SetUpdate(true).OnComplete(() =>
            {
                pausePanel.SetActive(false);
                pauseButton.gameObject.SetActive(true);
            });
        }
    }
}
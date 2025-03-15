using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PanelToggle : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource; // AudioSource pour jouer les sons
    [SerializeField] private AudioClip buttonClickSound; // Son pour les boutons
    [SerializeField] private AudioClip panelSlideSound; // Son pour l'animation du panel

    [SerializeField] private Button listButton;
    [SerializeField] private GameObject panel;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private float buttonScaleFactor = 1.1f;
    [SerializeField] private bool allowOtherPanels = false; // Permet d'ouvrir d'autres panels en pause

    private bool isPanelOpen = false;
    private RectTransform panelRect;
    private Vector3 closedPosition;
    private Vector3 openPosition;
    private Vector3 originalButtonScale;

    void Start()
    {
        panelRect = panel.GetComponent<RectTransform>();
        originalButtonScale = listButton.transform.localScale;
        closedPosition = new Vector3(-panelRect.rect.width - 200, panelRect.anchoredPosition.y, 0);
        openPosition = new Vector3(-200, panelRect.anchoredPosition.y, 0);
        panelRect.anchoredPosition = closedPosition;
        panel.SetActive(false);
        listButton.onClick.AddListener(TogglePanel);
    }

    void Update()
    {
        if (isPanelOpen && Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUI())
            {
                TogglePanel();
            }
        }
    }

    private bool IsPointerOverUI()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    public void TogglePanel()
    {
        isPanelOpen = !isPanelOpen;
        listButton.image.sprite = isPanelOpen ? pressedSprite : normalSprite;

        // Jouer le son de bouton
        PlaySound(buttonClickSound);

        if (isPanelOpen)
        {
            panel.SetActive(true);

            // Jouer le son de slider
            PlaySound(panelSlideSound);

            panelRect.DOAnchorPos(openPosition, animationDuration).SetEase(Ease.OutBounce).SetUpdate(true);
            listButton.transform.DOScale(originalButtonScale * buttonScaleFactor, animationDuration * 0.5f).SetEase(Ease.OutQuad).SetUpdate(true);
        }
        else
        {
            // Jouer le son de slider
            PlaySound(panelSlideSound);

            panelRect.DOAnchorPos(closedPosition, animationDuration).SetEase(Ease.InOutQuad).SetUpdate(true).OnComplete(() =>
            {
                panel.SetActive(false);
            });
            listButton.transform.DOScale(originalButtonScale, animationDuration * 0.5f).SetEase(Ease.InOutQuad).SetUpdate(true);
        }
    }
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

}

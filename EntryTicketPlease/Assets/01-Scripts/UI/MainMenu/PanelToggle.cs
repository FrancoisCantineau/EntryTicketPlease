using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PanelToggle : MonoBehaviour
{
    [SerializeField] private Button listButton;
    [SerializeField] private GameObject panel;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private float buttonScaleFactor = 1.1f;

    private bool isPanelOpen = false;
    private RectTransform panelRect;
    private Vector3 closedPosition;
    private Vector3 openPosition;
    private Vector3 originalButtonScale;

    void Start()
    {
        panelRect = panel.GetComponent<RectTransform>();
        originalButtonScale = listButton.transform.localScale;
        closedPosition = new Vector3(-panelRect.rect.width-200, panelRect.anchoredPosition.y, 0);
        openPosition = new Vector3(-200, panelRect.anchoredPosition.y, 0);
        panelRect.anchoredPosition = closedPosition;
        panel.SetActive(false);
        listButton.onClick.AddListener(TogglePanel);
    }

    void Update()
    {
        if (isPanelOpen && Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverGameObject())
            {
                TogglePanel();
            }
        }
    }

    private bool IsPointerOverGameObject()
    {
        return IsPointerOverUIElement(panelRect) || IsPointerOverUIElement(listButton.GetComponent<RectTransform>());
    }

    private bool IsPointerOverUIElement(RectTransform rectTransform)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, null);
    }

    public void TogglePanel()
    {
        isPanelOpen = !isPanelOpen;
        listButton.image.sprite = isPanelOpen ? pressedSprite : normalSprite;

        if (isPanelOpen)
        {
            panel.SetActive(true);
            panelRect.DOAnchorPos(openPosition, animationDuration).SetEase(Ease.OutBounce);
            listButton.transform.DOScale(originalButtonScale * buttonScaleFactor, animationDuration * 0.5f).SetEase(Ease.OutQuad);
        }
        else
        {
            panelRect.DOAnchorPos(closedPosition, animationDuration).SetEase(Ease.InOutQuad).OnComplete(() => panel.SetActive(false));
            listButton.transform.DOScale(originalButtonScale, animationDuration * 0.5f).SetEase(Ease.InOutQuad);
        }
    }
}
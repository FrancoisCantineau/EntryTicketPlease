using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PanelSlideOut : MonoBehaviour
{
    [SerializeField] private RectTransform rectPanel;
    [SerializeField] private GameObject gameobjectPanel;
    [SerializeField] private float slideDuration = 0.5f;
    private Vector2 hiddenPosition;
    private Vector2 visiblePosition;

    void Start()
    {
        gameobjectPanel.SetActive(true);
        visiblePosition = rectPanel.anchoredPosition;
        hiddenPosition = new Vector2(rectPanel.anchoredPosition.x, Screen.height);
        rectPanel.gameObject.SetActive(true);
        StartBouncingEffect();
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SlideOut();
        }
    }

    private void SlideOut()
    {
        DOTween.Kill("BounceEffect"); // Stoppe le rebond
        rectPanel.DOAnchorPos(hiddenPosition, slideDuration).SetEase(Ease.InOutQuad);
    }

    private void StartBouncingEffect()
    {
        rectPanel.DOAnchorPosY(visiblePosition.y + 50f, 0.3f) // Monte de 30 pixels
            .SetEase(Ease.OutQuad)
            .SetLoops(-1, LoopType.Yoyo) // Rebond infini
            .SetDelay(3f) // Toutes les 3 secondes
            .SetId("BounceEffect"); // Identifiant pour stopper l'animation plus tard
    }

}
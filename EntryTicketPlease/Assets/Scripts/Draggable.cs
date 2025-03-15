using UnityEngine;
using UnityEngine.UI;

public class DraggableObject : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;
    private Vector3 initialScale;
    private RectTransform rectTransform;

    public Vector2 minBounds = new Vector2(-5f, -5f);
    public Vector2 maxBounds = new Vector2(5f, 5f);
    public RectTransform noDragZone;
    public float zoomScale = 1.5f;
    public float zoomSpeed = 5f;

    private AudioSource audioSource;
    public AudioClip grabSound;

    private bool isSpawning = true;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Aucune cam�ra principale trouv�e");
            enabled = false;
            return;
        }

        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("RectTransform manquant sur cet objet !");
            enabled = false;
            return;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Ne pas d�finir initialScale ici, il sera d�fini par CharacterNavMeshMovement3D via SetInitialScale
        Invoke("FinishSpawning", 0.6f);
    }

    private void FinishSpawning()
    {
        isSpawning = false;
    }

    void Update()
    {
        if (mainCamera == null || isSpawning) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = GetWorldPosition(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (IsTouchingObject(touchPosition))
                    {
                        isDragging = true;
                        offset = rectTransform.position - touchPosition;
                        ZoomIn();
                    }
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        MoveObject(touchPosition);
                    }
                    break;

                case TouchPhase.Ended:
                    if (isDragging)
                    {
                        isDragging = false;
                        ZoomOut();
                    }
                    break;
            }
        }
        else
        {
            Vector3 mousePosition = GetWorldPosition(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                if (IsTouchingObject(mousePosition))
                {
                    isDragging = true;
                    offset = rectTransform.position - mousePosition;
                    ZoomIn();
                    Debug.Log("D�but du drag � : " + rectTransform.position);
                }
            }
            else if (Input.GetMouseButton(0) && isDragging)
            {
                MoveObject(mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (isDragging)
                {
                    isDragging = false;
                    ZoomOut();
                    Debug.Log("Fin du drag � : " + rectTransform.position);
                }
            }
        }

        if (initialScale != Vector3.zero)
        {
            if (isDragging)
            {
                rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, initialScale * zoomScale, Time.deltaTime * zoomSpeed);
            }
            else
            {
                rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, initialScale, Time.deltaTime * zoomSpeed);
            }
        }
    }

    private Vector3 GetWorldPosition(Vector2 screenPosition)
    {
        if (mainCamera == null) return Vector3.zero;

        if (float.IsNaN(screenPosition.x) || float.IsNaN(screenPosition.y) ||
            float.IsInfinity(screenPosition.x) || float.IsInfinity(screenPosition.y))
        {
            return rectTransform.position;
        }

        float zDepth = Mathf.Abs(rectTransform.position.z - mainCamera.transform.position.z);
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, zDepth));
        return new Vector3(worldPos.x, worldPos.y, rectTransform.position.z);
    }

    private bool IsTouchingObject(Vector3 worldPosition)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            mainCamera.WorldToScreenPoint(worldPosition),
            mainCamera,
            out localPoint
        );
        return rectTransform.rect.Contains(localPoint);
    }

    private void MoveObject(Vector3 inputPosition)
    {
        Vector3 newPosition = inputPosition + offset;

        if (noDragZone != null && RectTransformUtility.RectangleContainsScreenPoint(
            noDragZone,
            mainCamera.WorldToScreenPoint(newPosition),
            mainCamera))
        {
            return;
        }

        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);
        newPosition.z = rectTransform.position.z;
        rectTransform.position = newPosition;
    }

    private void ZoomIn()
    {
        if (audioSource != null && grabSound != null)
        {
            audioSource.PlayOneShot(grabSound);
        }
        rectTransform.SetAsLastSibling();
    }

    private void ZoomOut()
    {
        // Rien ici, l'�chelle est g�r�e dans Update
    }

    // M�thode publique pour d�finir l'�chelle initiale
    public void SetInitialScale(Vector3 scale)
    {
        initialScale = scale;
        // Ne pas d�finir rectTransform.localScale ici, car l'animation DOTween le fera
    }
}
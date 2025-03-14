using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;
    private Vector3 initialScale;
    private SpriteRenderer spriteRenderer;
    private int initialSortingOrder;

    public Vector2 minBounds = new Vector2(-5f, -5f);
    public Vector2 maxBounds = new Vector2(5f, 5f);
    public Collider2D noDragZone;
    public float zoomScale = 1.5f;
    public float zoomSpeed = 5f;

    private static int highestSortingOrder = 0;
    private AudioSource audioSource;
    public AudioClip grabSound;

    private bool isSpawning = true;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Aucune caméra principale trouvée");
            enabled = false;
            return;
        }
        if (noDragZone == null)
        {
            Debug.LogWarning("Aucune zone interdite définie");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer manquant sur cet objet !");
            enabled = false;
            return;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }



        initialScale = transform.localScale;
        if (initialScale == Vector3.zero)
        {
            initialScale = Vector3.one;
        }

        initialSortingOrder = spriteRenderer.sortingOrder;
        highestSortingOrder = Mathf.Max(highestSortingOrder, initialSortingOrder);

        Invoke("FinishSpawning", 0.6f);
    }

    private void FinishSpawning()
    {
        isSpawning = false;
    }

    void Update()
    {
        if (mainCamera == null || isSpawning) return;

        // Mode Mobile (Touch)
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
                        offset = transform.position - touchPosition;
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
        // Mode PC (Souris)
        else
        {
            Vector3 mousePosition = GetWorldPosition(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                if (IsTouchingObject(mousePosition))
                {
                    isDragging = true;
                    offset = transform.position - mousePosition;
                    ZoomIn();
                    Debug.Log("Début du drag à : " + transform.position);
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
                    Debug.Log("Fin du drag à : " + transform.position);
                }
            }
        }

        if (initialScale != Vector3.zero)
        {
            if (isDragging)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, initialScale * zoomScale, Time.deltaTime * zoomSpeed);
            }
            else
            {
                transform.localScale = Vector3.Lerp(transform.localScale, initialScale, Time.deltaTime * zoomSpeed);
            }
        }
    }



    private Vector3 GetWorldPosition(Vector2 screenPosition)
    {
        if (mainCamera == null) return Vector3.zero;

        if (float.IsNaN(screenPosition.x) || float.IsNaN(screenPosition.y) ||
            float.IsInfinity(screenPosition.x) || float.IsInfinity(screenPosition.y))
        {
            return transform.position;
        }

        float zDepth = Mathf.Abs(transform.position.z - mainCamera.transform.position.z);
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, zDepth));
        return new Vector3(worldPos.x, worldPos.y, transform.position.z);
    }

    private bool IsTouchingObject(Vector3 worldPosition)
    {
        Collider2D col = GetComponent<Collider2D>();
        return col != null && col == Physics2D.OverlapPoint(worldPosition);
    }

    private void MoveObject(Vector3 inputPosition)
    {
        Vector3 newPosition = inputPosition + offset;

        if (noDragZone != null && noDragZone.OverlapPoint(newPosition))
        {
            return;
        }

        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);
        newPosition.z = transform.position.z;
        transform.position = newPosition;

    }

    private void ZoomIn()
    {
        if (audioSource != null && grabSound != null)
        {
            audioSource.PlayOneShot(grabSound);
        }
        SetOnTop();
    }

    private void ZoomOut()
    {
        // Rien ici, géré dans Update
    }

    private void SetOnTop()
    {
        if (spriteRenderer != null)
        {
            highestSortingOrder++;
            spriteRenderer.sortingOrder = highestSortingOrder;
        }
    }
}

using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterNavMeshMovement3D : MonoBehaviour
{
    [Header("Cibles")]
    public Vector3 initialTarget = new Vector3(-0.107f, 0.851f, -7.421f);
    public Vector3 validateTarget = new Vector3(1.38f, 0.898f, -6.36f);
    public Vector3 refuseTarget = new Vector3(-1.867f, 0.898f, -6.201f);
    [Header("Paramètres")]
    public float stoppingDistance = 0.1f;
    public float rotationSpeed = 5f;

    [Header("Sprites")]
    public Sprite[] validateSprites;
    public Sprite[] refuseSprites;
    public GameObject ticketPrefab;
    public int ticketCount = 1;
    public float spriteHeight = 1.5f;
    public float spriteDuration = 2f;
    public float ticketSpawnRadius = 0.5f;

    [Header("Table")]
    public Transform tableTransform;

    private RectTransform noDragZone;
    private Canvas worldSpaceCanvas;
    private UnityEngine.UI.Button validateButton;
    private UnityEngine.UI.Button refuseButton;

    private Vector3 targetPosition;
    private NavMeshAgent navAgent;
    private Animator animator;
    private Camera mainCamera;
    private bool isMoving = false;
    private bool hasReachedInitialTarget = false;
    private bool hasProcessedDecision = false;
    private bool hasSpawnedTickets = false; // Nouveau drapeau pour éviter plusieurs spawns de tickets

    private GameObject currentSpriteObj;
    private List<GameObject> spawnedTickets = new List<GameObject>();

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

        if (navAgent == null || animator == null || mainCamera == null || ticketPrefab == null)
        {
            Debug.LogError("Composants essentiels manquants sur " + gameObject.name + " !");
            enabled = false;
            return;
        }

        GameObject canvasObj = GameObject.Find("Zone");
        if (canvasObj != null)
        {
            worldSpaceCanvas = canvasObj.GetComponent<Canvas>();
            if (worldSpaceCanvas == null || worldSpaceCanvas.renderMode != RenderMode.WorldSpace)
            {
                Debug.LogError("L'objet 'Zone' n’est pas un Canvas en mode WorldSpace !");
                enabled = false;
                return;
            }
        }
        else
        {
            Debug.LogError("Aucun objet nommé 'Zone' trouvé dans la scène !");
            enabled = false;
            return;
        }

        GameObject noDragObj = GameObject.Find("NoDragZone");
        if (noDragObj != null)
        {
            noDragZone = noDragObj.GetComponent<RectTransform>();
            if (noDragZone == null)
            {
                Debug.LogError("L'objet 'NoDragZone' n’a pas de RectTransform !");
                enabled = false;
                return;
            }
        }

        GameObject validateObj = GameObject.Find("ValidateButton");
        if (validateObj != null)
        {
            validateButton = validateObj.GetComponent<UnityEngine.UI.Button>();
            if (validateButton == null)
            {
                Debug.LogError("L'objet 'ValidateButton' n’a pas de composant Button !");
                enabled = false;
                return;
            }
            validateButton.onClick.RemoveAllListeners();
            validateButton.onClick.AddListener(OnValidateButton);
        }
        else
        {
            Debug.LogError("Aucun objet nommé 'ValidateButton' trouvé dans la scène !");
            enabled = false;
            return;
        }

        GameObject refuseObj = GameObject.Find("RefuseButton");
        if (refuseObj != null)
        {
            refuseButton = refuseObj.GetComponent<UnityEngine.UI.Button>();
            if (refuseButton == null)
            {
                Debug.LogError("L'objet 'RefuseButton' n’a pas de composant Button !");
                enabled = false;
                return;
            }
            refuseButton.onClick.RemoveAllListeners();
            refuseButton.onClick.AddListener(OnRefuseButton);
        }
        else
        {
            Debug.LogError("Aucun objet nommé 'RefuseButton' trouvé dans la scène !");
            enabled = false;
            return;
        }

        navAgent.updateRotation = false;
        if (validateButton != null) validateButton.interactable = false;
        if (refuseButton != null) refuseButton.interactable = false;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(initialTarget, out hit, 10.0f, NavMesh.AllAreas)) initialTarget = hit.position;
        if (NavMesh.SamplePosition(validateTarget, out hit, 10.0f, NavMesh.AllAreas)) validateTarget = hit.position;
        if (NavMesh.SamplePosition(refuseTarget, out hit, 10.0f, NavMesh.AllAreas)) refuseTarget = hit.position;

        targetPosition = initialTarget;
        SetTarget(targetPosition);
    }

    void Update()
    {
        isMoving = navAgent.velocity.magnitude > 0.1f && navAgent.remainingDistance > stoppingDistance;

        if (!isMoving)
        {
            // Vérifie spécifiquement si le personnage atteint initialTarget
            if (!hasReachedInitialTarget && Vector3.Distance(transform.position, initialTarget) <= stoppingDistance)
            {
                hasReachedInitialTarget = true;
                if (validateButton != null) validateButton.interactable = true;
                if (refuseButton != null) refuseButton.interactable = true;
                if (!hasSpawnedTickets)
                {
                    SpawnTicketsOnTable();
                    hasSpawnedTickets = true; // Empêche de respawn les tickets
                    Debug.Log("Tickets spawnés à initialTarget pour " + gameObject.name);
                }
            }
            // Suppression quand il atteint validateTarget ou refuseTarget
            else if ((Vector3.Distance(transform.position, validateTarget) <= stoppingDistance) ||
                     (Vector3.Distance(transform.position, refuseTarget) <= stoppingDistance))
            {
                Debug.Log("Suppression de " + gameObject.name + " à validate/refuseTarget");
                Destroy(gameObject);
                return;
            }

            // Rotation vers la caméra lorsqu'il est immobile
            Vector3 directionToCamera = (mainCamera.transform.position - transform.position).normalized;
            directionToCamera.y = 0;
            if (directionToCamera != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(directionToCamera, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }
        }
        else if (isMoving && navAgent.velocity != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(navAgent.velocity.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("IsWalking", isMoving);
            if (!isMoving && !animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                animator.Play("Idle");
            }
        }
    }

    private void SetTarget(Vector3 newTarget)
    {
        targetPosition = newTarget;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(newTarget, out hit, 10.0f, NavMesh.AllAreas))
        {
            targetPosition = hit.position;
        }
        else
        {
            targetPosition = transform.position;
        }

        navAgent.SetDestination(targetPosition);
        isMoving = true;
        // Ne réinitialise pas hasReachedInitialTarget ici, car on veut conserver l'état
    }

    private void SpawnTicketsOnTable()
    {
        if (ticketPrefab == null)
        {
            Debug.LogWarning("Ticket Prefab non assigné !");
            return;
        }

        spawnedTickets.Clear();

        for (int i = 0; i < ticketCount; i++)
        {
            GameObject ticketObj = Instantiate(ticketPrefab, worldSpaceCanvas.transform);
            spawnedTickets.Add(ticketObj);

            DraggableObject draggable = ticketObj.GetComponent<DraggableObject>();
            if (draggable == null)
            {
                Debug.LogError("DraggableObject manquant sur le ticket prefab !");
                continue;
            }

            if (noDragZone != null)
            {
                draggable.noDragZone = noDragZone;
            }

            Image image = ticketObj.GetComponent<Image>();
            if (image == null || image.sprite == null)
            {
                Debug.LogError("Le ticket spawné n’a pas d’Image ou de sprite !");
            }

            RectTransform rectTransform = ticketObj.GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                Debug.LogError("RectTransform manquant sur le ticket prefab !");
                continue;
            }

            Vector3 originalScale = rectTransform.localScale;
            draggable.SetInitialScale(originalScale);
            ticketObj.transform.localScale = Vector3.zero;
            ticketObj.SetActive(true);
            ticketObj.transform.DOScale(originalScale, 0.5f).SetEase(Ease.OutBack);
        }
    }

    private void ShowSprite(Sprite sprite, Vector3 nextTarget)
    {
        if (currentSpriteObj != null)
        {
            Destroy(currentSpriteObj);
        }

        currentSpriteObj = new GameObject("SpriteFeedback");
        currentSpriteObj.transform.SetParent(transform);
        SpriteRenderer renderer = currentSpriteObj.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;

        Vector3 startPos = transform.position + Vector3.up * spriteHeight;
        currentSpriteObj.transform.position = startPos;

        currentSpriteObj.transform.localScale = Vector3.zero;
        renderer.color = new Color(1f, 1f, 1f, 1f);
        Sequence spriteSequence = DOTween.Sequence();
        spriteSequence
            .Append(currentSpriteObj.transform.DOScale(1f, 0.5f))
            .Join(currentSpriteObj.transform.DOMoveY(startPos.y + 0.5f, spriteDuration).SetEase(Ease.OutQuad))
            .Append(renderer.DOFade(0f, 0.5f))
            .OnComplete(() =>
            {
                Destroy(currentSpriteObj);
                SetTarget(nextTarget);
            });
    }

    private void DestroySpawnedTickets()
    {
        foreach (GameObject ticket in spawnedTickets)
        {
            if (ticket != null)
            {
                Destroy(ticket);
            }
        }
        spawnedTickets.Clear();
        Debug.Log("Tous les tickets spawnés ont été supprimés pour " + gameObject.name);
    }

    public void OnValidateButton()
    {
        Debug.Log("Clicked Valider par " + gameObject.name);

        if (hasReachedInitialTarget && !hasProcessedDecision)
        {
            hasProcessedDecision = true;
            DestroySpawnedTickets();
            if (validateSprites != null && validateSprites.Length > 0)
            {
                Sprite randomSprite = validateSprites[Random.Range(0, validateSprites.Length)];
                ShowSprite(randomSprite, validateTarget);
            }
            else
            {
                SetTarget(validateTarget);
            }

            if (RoundManager.Instance != null)
            {
                RoundManager.Instance.EndOfVisitor(true);
                Debug.Log("EndOfVisitor(true) appelé par " + gameObject.name);
            }
            else
            {
                Debug.LogWarning("RoundManager.Instance est null !");
            }
        }
    }

    public void OnRefuseButton()
    {
        Debug.Log("Clicked Refuser par " + gameObject.name);

        if (hasReachedInitialTarget && !hasProcessedDecision)
        {
            hasProcessedDecision = true;
            DestroySpawnedTickets();
            if (refuseSprites != null && refuseSprites.Length > 0)
            {
                Sprite randomSprite = refuseSprites[Random.Range(0, refuseSprites.Length)];
                ShowSprite(randomSprite, refuseTarget);
            }
            else
            {
                SetTarget(refuseTarget);
            }

            if (RoundManager.Instance != null)
            {
                RoundManager.Instance.EndOfVisitor(false);
                Debug.Log("EndOfVisitor(false) appelé par " + gameObject.name);
            }
            else
            {
                Debug.LogWarning("RoundManager.Instance est null !");
            }
        }
    }
}
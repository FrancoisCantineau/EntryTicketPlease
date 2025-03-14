using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class CharacterNavMeshMovement3D : MonoBehaviour
{
    [Header("Cibles")]
    public Vector3 initialTarget = new Vector3(-0.107f, 0.851f, -7.421f); // Premier point
    public Vector3 validateTarget = new Vector3(1.38f, 0.898f, -6.36f);   // Apr�s "Valider"
    public Vector3 refuseTarget = new Vector3(-1.867f, 0.898f, -6.201f);  // Apr�s "Refuser"

    [Header("Param�tres")]
    public float stoppingDistance = 0.1f; // Distance � laquelle le personnage s'arr�te
    public float rotationSpeed = 5f;      // Vitesse de rotation pour regarder la cam�ra

    [Header("Sprites")]
    public Sprite[] validateSprites;      // Liste de sprites pour "Valider"
    public Sprite[] refuseSprites;        // Liste de sprites pour "Refuser"
    public GameObject ticketPrefab;       // Prefab pour les tickets
    public int ticketCount = 1;           // Nombre de tickets � faire spawn
    public float spriteHeight = 1.5f;     // Hauteur au-dessus du personnage pour validate/refuse
    public float spriteDuration = 2f;     // Dur�e de l'animation validate/refuse
    public float ticketSpawnRadius = 0.5f;// Rayon de spawn autour de la table (non utilis� ici)

    [Header("Table")]
    public Transform tableTransform;      // R�f�rence � la table dans la sc�ne (non utilis� ici)

    [Header("Draggable Zone")]
    public Collider2D noDragZone;         // R�f�rence � la zone "no drag" dans la sc�ne

    private Vector3 targetPosition;       // Position cible actuelle
    private NavMeshAgent navAgent;        // R�f�rence au NavMeshAgent
    private Animator animator;            // Pour g�rer les animations
    private Camera mainCamera;            // R�f�rence � la cam�ra principale
    private bool isMoving = false;        // Indique si le personnage bouge
    private bool hasReachedInitialTarget = false; // Indique si le premier point est atteint
    private GameObject currentSpriteObj;  // R�f�rence au sprite actuel (validate/refuse)

    [Header("UI")]
    public UnityEngine.UI.Button validateButton; // R�f�rence au bouton TMP "Valider"
    public UnityEngine.UI.Button refuseButton;   // R�f�rence au bouton TMP "Refuser"

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

        if (navAgent == null || animator == null || mainCamera == null || ticketPrefab == null)
        {
            enabled = false;
            return;
        }

        // Recherche automatique des boutons TMP
        if (validateButton == null)
        {
            GameObject validateObj = GameObject.Find("Valider");
            if (validateObj != null) validateButton = validateObj.GetComponent<UnityEngine.UI.Button>();
        }
        if (refuseButton == null)
        {
            GameObject refuseObj = GameObject.Find("Refuser");
            if (refuseObj != null) refuseButton = refuseObj.GetComponent<UnityEngine.UI.Button>();
        }

        // Recherche automatique de noDragZone si non assign�
        if (noDragZone == null)
        {
            GameObject noDragObj = GameObject.Find("NoDragZone"); // Remplace par le nom exact de ton objet
            if (noDragObj != null) noDragZone = noDragObj.GetComponent<Collider2D>();
            if (noDragZone == null) Debug.LogWarning("NoDragZone non trouv� dans la sc�ne !");
        }

        navAgent.updateRotation = false;

        // D�sactive les boutons au d�marrage
        if (validateButton != null) validateButton.interactable = false;
        if (refuseButton != null) refuseButton.interactable = false;

        // Ajuste toutes les cibles au NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(initialTarget, out hit, 10.0f, NavMesh.AllAreas))
            initialTarget = hit.position;
        if (NavMesh.SamplePosition(validateTarget, out hit, 10.0f, NavMesh.AllAreas))
            validateTarget = hit.position;
        if (NavMesh.SamplePosition(refuseTarget, out hit, 10.0f, NavMesh.AllAreas))
            refuseTarget = hit.position;

        targetPosition = initialTarget;
        SetTarget(targetPosition);
    }

    void Update()
    {
        isMoving = navAgent.velocity.magnitude > 0.1f && navAgent.remainingDistance > stoppingDistance;

        if (!isMoving && !hasReachedInitialTarget && Vector3.Distance(transform.position, targetPosition) <= stoppingDistance)
        {
            hasReachedInitialTarget = true;
            if (validateButton != null) validateButton.interactable = true;
            if (refuseButton != null) refuseButton.interactable = true;
            SpawnTicketsOnTable();
        }

        if (isMoving && navAgent.velocity != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(navAgent.velocity.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
        else if (!isMoving)
        {
            Vector3 directionToCamera = (mainCamera.transform.position - transform.position).normalized;
            directionToCamera.y = 0;
            if (directionToCamera != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(directionToCamera, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }
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
        hasReachedInitialTarget = false;
    }

    private void SpawnTicketsOnTable()
    {
        if (ticketPrefab == null)
        {
            Debug.LogWarning("Ticket Prefab non assign� !");
            return;
        }

        for (int i = 0; i < ticketCount; i++)
        {
            // Position et rotation fixes sp�cifi�es
            Vector3 spawnPos = new Vector3(0f, 0.8f, -8f);
            Quaternion spawnRot = Quaternion.Euler(30f, 0.6f, 271f);

            // Instancie le prefab directement � la bonne position et rotation
            GameObject ticketObj = Instantiate(ticketPrefab, spawnPos, spawnRot);

            // R�cup�re le composant DraggableObject et assigne noDragZone
            DraggableObject draggable = ticketObj.GetComponent<DraggableObject>();
            if (draggable != null && noDragZone != null)
            {
                draggable.noDragZone = noDragZone;
            }
            else
            {
                Debug.LogWarning("DraggableObject ou noDragZone manquant pour le ticket !");
            }

            // V�rifie le SpriteRenderer
            SpriteRenderer renderer = ticketObj.GetComponent<SpriteRenderer>();
            if (renderer == null || renderer.sprite == null)
            {
                Debug.LogError("Le ticket spawn� n�a pas de SpriteRenderer ou de sprite !");
            }
            else
            {
                renderer.sortingOrder = 10; // Met le ticket devant
            }

            // Animation d�apparition avec DOTween
            ticketObj.transform.localScale = Vector3.zero;
            ticketObj.SetActive(true); // S�assure que le ticket est actif
            ticketObj.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                Debug.Log("Animation termin�e pour ticket � : " + ticketObj.transform.position + " �chelle finale : " + ticketObj.transform.localScale);
            });
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

    public void OnValidateButton()
    {
        if (hasReachedInitialTarget)
        {
            if (validateSprites != null && validateSprites.Length > 0)
            {
                Sprite randomSprite = validateSprites[Random.Range(0, validateSprites.Length)];
                ShowSprite(randomSprite, validateTarget);
            }
            else
            {
                SetTarget(validateTarget);
            }
        }
    }

    public void OnRefuseButton()
    {
        if (hasReachedInitialTarget)
        {
            if (refuseSprites != null && refuseSprites.Length > 0)
            {
                Sprite randomSprite = refuseSprites[Random.Range(0, refuseSprites.Length)];
                ShowSprite(randomSprite, refuseTarget);
            }
            else
            {
                SetTarget(refuseTarget);
            }
        }
    }
}
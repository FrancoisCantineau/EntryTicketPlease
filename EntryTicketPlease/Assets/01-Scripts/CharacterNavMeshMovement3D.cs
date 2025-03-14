using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class CharacterNavMeshMovement3D : MonoBehaviour
{
    [Header("Cibles")]
    public Vector3 initialTarget = new Vector3(-0.107f, 0.851f, -7.421f); // Premier point
    public Vector3 validateTarget = new Vector3(1.38f, 0.898f, -6.36f);   // Après "Valider"
    public Vector3 refuseTarget = new Vector3(-1.867f, 0.898f, -6.201f);  // Après "Refuser"

    [Header("Paramètres")]
    public float stoppingDistance = 0.1f;
    public float rotationSpeed = 5f;     

    [Header("Sprites")]
    public Sprite[] validateSprites;     
    public Sprite[] refuseSprites;       
    public float spriteHeight = 1.5f;    
    public float spriteDuration = 2f;    

    private Vector3 targetPosition;      
    private NavMeshAgent navAgent;      
    private Animator animator;          
    private Camera mainCamera;            
    private bool isMoving = false;       
    private bool hasReachedInitialTarget = false; 
    private GameObject currentSpriteObj;  

    [Header("UI")]
    public UnityEngine.UI.Button validateButton; 
    public UnityEngine.UI.Button refuseButton;   

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

        if (navAgent == null || animator == null || mainCamera == null)
        {
            enabled = false;
            return;
        }

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

        navAgent.updateRotation = false;

        if (validateButton != null) validateButton.interactable = false;
        if (refuseButton != null) refuseButton.interactable = false;

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
using UnityEngine;
using UnityEngine.AI;

public class CharacterNavMeshMovement3D : MonoBehaviour
{
    [Header("Cibles")]
    public Vector3 initialTarget = new Vector3(-0.107f, 0.851f, -7.421f); // Premier point
    public Vector3 validateTarget = new Vector3(1.38f, 0.898f, -6.36f);   // Après "Valider"
    public Vector3 refuseTarget = new Vector3(-1.867f, 0.898f, -6.201f);  // Après "Refuser"

    [Header("Paramètres")]
    public float stoppingDistance = 0.1f; // Distance à laquelle le personnage s'arrête
    public float rotationSpeed = 5f;      // Vitesse de rotation pour regarder la caméra

    private Vector3 targetPosition;       // Position cible actuelle
    private NavMeshAgent navAgent;        // Référence au NavMeshAgent
    private Animator animator;            // Pour gérer les animations
    private Camera mainCamera;            // Référence à la caméra principale
    private bool isMoving = false;        // Indique si le personnage bouge
    private bool hasReachedInitialTarget = false; // Indique si le premier point est atteint

    [Header("UI")]
    public UnityEngine.UI.Button validateButton; // Référence au bouton TMP "Valider"
    public UnityEngine.UI.Button refuseButton;   // Référence au bouton TMP "Refuser"

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

        navAgent.updateRotation = false;

        // Désactive les boutons au démarrage
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

    public void OnValidateButton()
    {
        if (hasReachedInitialTarget)
        {
            SetTarget(validateTarget);
        }
    }

    public void OnRefuseButton()
    {
        if (hasReachedInitialTarget)
        {
            SetTarget(refuseTarget);
        }
    }
}
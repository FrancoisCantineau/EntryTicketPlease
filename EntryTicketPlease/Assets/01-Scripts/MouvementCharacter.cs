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

    [Header("UI (TMP Buttons)")]
    public UnityEngine.UI.Button validateButton; // Référence au bouton TMP "Valider"
    public UnityEngine.UI.Button refuseButton;   // Référence au bouton TMP "Refuser"

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

        if (navAgent == null)
        {
            Debug.LogError("NavMeshAgent manquant sur le personnage !");
            enabled = false;
            return;
        }
        if (animator == null)
        {
            Debug.LogError("Animator manquant sur le personnage !");
            enabled = false;
            return;
        }
        if (mainCamera == null)
        {
            Debug.LogError("Aucune caméra principale trouvée dans la scène !");
            enabled = false;
            return;
        }

        // Recherche automatique des boutons TMP
        if (validateButton == null)
        {
            GameObject validateObj = GameObject.Find("Valider"); // Ajuste au nom exact
            if (validateObj != null) validateButton = validateObj.GetComponent<UnityEngine.UI.Button>();
        }
        if (refuseButton == null)
        {
            GameObject refuseObj = GameObject.Find("Refuser"); // Ajuste au nom exact
            if (refuseObj != null) refuseButton = refuseObj.GetComponent<UnityEngine.UI.Button>();
        }

        if (validateButton == null || refuseButton == null)
        {
            Debug.LogWarning("Boutons TMP non trouvés ou non assignés ! Vérifie leurs noms ou assigne-les manuellement.");
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
            Debug.Log("Premier point atteint, activation des boutons.");
            if (validateButton != null)
            {
                validateButton.interactable = true;
                Debug.Log("Bouton Valider activé");
            }
            if (refuseButton != null)
            {
                refuseButton.interactable = true;
                Debug.Log("Bouton Refuser activé");
            }
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
                Debug.Log("Forcé l'animation Idle.");
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
            Debug.Log($"Cible ajustée à : {targetPosition}");
        }
        else
        {
            Debug.LogWarning($"La cible {newTarget} n'est pas sur le NavMesh, repli sur la position actuelle !");
            targetPosition = transform.position;
        }

        navAgent.SetDestination(targetPosition);
        isMoving = true;
        hasReachedInitialTarget = false;
    }

    public void OnValidateButton()
    {
        Debug.Log("OnValidateButton appelé");
        if (hasReachedInitialTarget)
        {
            SetTarget(validateTarget);
            Debug.Log("Bouton Valider cliqué : Direction " + validateTarget);
        }
        else
        {
            Debug.Log("Bouton Valider ignoré : Premier point non atteint.");
        }
    }

    public void OnRefuseButton()
    {
        Debug.Log("OnRefuseButton appelé");
        if (hasReachedInitialTarget)
        {
            SetTarget(refuseTarget);
            Debug.Log("Bouton Refuser cliqué : Direction " + refuseTarget);
        }
        else
        {
            Debug.Log("Bouton Refuser ignoré : Premier point non atteint.");
        }
    }
}
using UnityEngine;
using UnityEngine.AI;

public class CharacterNavMeshMovement3D : MonoBehaviour
{
    public Vector3 targetPosition = new Vector3(-0.107f, 0.851f, -7.421f); 
    public float stoppingDistance = 0.1f;
    public float rotationSpeed = 5f;      

    private NavMeshAgent navAgent; 
    private Animator animator;     
    private Camera mainCamera;    
    private bool isMoving = false; 
    private bool hasReachedTarget = false; 

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

        if (navAgent == null)
        {
            enabled = false;
            return;
        }
        if (animator == null)
        {
            enabled = false;
            return;
        }
        if (mainCamera == null)
        {
            enabled = false;
            return;
        }

        navAgent.updateRotation = false;

        SetTarget(targetPosition);
    }

    void Update()
    {
        isMoving = navAgent.velocity.magnitude > 0.1f && navAgent.remainingDistance > stoppingDistance;

        if (!isMoving && !hasReachedTarget && Vector3.Distance(transform.position, targetPosition) <= stoppingDistance)
        {
            hasReachedTarget = true;
            Debug.Log("Cible atteinte, le personnage regarde la caméra.");
        }

        if (isMoving && navAgent.velocity != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(navAgent.velocity.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
        else if (hasReachedTarget)
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

    public void SetTarget(Vector3 newTarget)
    {
        targetPosition = newTarget;
        navAgent.SetDestination(targetPosition);
        hasReachedTarget = false; 

        NavMeshHit hit;

    }
}
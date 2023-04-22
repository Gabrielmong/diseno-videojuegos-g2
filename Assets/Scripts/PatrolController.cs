using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolController : MonoBehaviour
{
    enum States
    {
        Patrol,
        Chase,
        Attack
    }

    [SerializeField]
    Animator animator;

    [Header("Target")]
    [SerializeField]
    Transform target;

    [SerializeField]
    LayerMask whatIsPlayer;

    [Header("Patrol")]
    [SerializeField]
    LayerMask whatIsGround;

    [SerializeField]
    float walkPointRange;

    [SerializeField]
    int sightRange;

    [Header("NavMeshAgent")]
    [SerializeField]
    NavMeshAgent agent;

    Vector3 walkPoint;

    bool walkPointSet;
    bool targetInSight;

    bool targetInAttackRange;

    States currentState;

    [SerializeField]
    bool attacking;

    [Header("Attack")]
    [SerializeField]
    float attackRange;

    [SerializeField]
    float attackRate;

    [SerializeField]
    int attackDamage;

    float nextAttackTime;

    private bool waiting;




    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        targetInSight = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        targetInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        currentState =
            targetInAttackRange && targetInSight
                ? States.Attack
                : targetInSight && !targetInAttackRange
                    ? States.Chase
                    : States.Patrol;


        switch (currentState)
        {
            case States.Patrol:
                Patrol();
                break;
            case States.Chase:
                Chase();
                break;
            case States.Attack:
                Attack();
                break;

        }
    }

    void Patrol()
    {
        if (waiting)
        {
            return;
        }

        if (!walkPointSet) SearchWalkPoint();


        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            Debug.Log("Walking");
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {


            StartCoroutine(StayStill());

            walkPointSet = false;
            animator.SetBool("isWalking", false);
        }

    }

    void SearchWalkPoint()
    {


        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;

    }

    void Chase()
    {
        agent.SetDestination(target.position);
        animator.SetBool("isWalking", true);
    }

    void Attack()
    {
        // Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(target);
        animator.SetBool("isWalking", false);

        if (Time.time >= nextAttackTime)
        {
            animator.SetBool("isAttacking", true);
            AttackPlayer();

            
        }
        


    }

    private void AttackPlayer()
    {
        // Attack code here
        Collider[] hitPlayer = Physics.OverlapSphere(transform.position, attackRange, whatIsPlayer);

        foreach (Collider player in hitPlayer)
        {
            player.GetComponent<PlayerController>().TakeDamage(attackDamage);
        }

        // random time between 1 and 3 seconds
        float waitTime = Random.Range(1f, 3f);

        Invoke("ResetAttack", waitTime);
    }

    void ResetAttack()
    {
        animator.SetBool("isAttacking", false);
        
        attacking = false;

        // set next attack time
        nextAttackTime = Time.time + 1f / attackRate;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    IEnumerator StayStill()
    {
        // random time between 1 and 3 seconds
        float waitTime = Random.Range(1f, 3f);
        waiting = true;
        yield return new WaitForSeconds(waitTime);
        waiting = false;
    }


}

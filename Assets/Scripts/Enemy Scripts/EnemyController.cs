
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
  PATROL,
  CHASE,
  ATTACK
}

public class EnemyController : MonoBehaviour
{
  private EnemyAnimator enemyAnimator;
  private NavMeshAgent navMeshAgent;
  private EnemyState enemyState;

  public float walkSpeed = 0.5f;
  public float runSpeed = 0.5f;
  public float chaseDistance = 10f;
  private float currentChaseDistance;
  public float attackDistance = 2.2f;
  public float chaseAfterAttackDistance = 2f;
  public float patrolRadiusMin = 20f, patrolRadiusMax = 60f;
  public float patrolForThisTime = 15f;
  private float patrolTimer;
  public float waitBeforeAttack = 0.2f;
  private float attackTimer;
  private Transform target;
  public GameObject attackPoint;
  public EnemyAudio enemyAudio;

  void Awake()
  {
    enemyAnimator = GetComponent<EnemyAnimator>();
    navMeshAgent = GetComponent<NavMeshAgent>();
    target = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;
    enemyAudio = GetComponentInChildren<EnemyAudio>();
  }

  void Start()
  {
    enemyState = EnemyState.PATROL;
    patrolTimer = patrolForThisTime;
    attackTimer = waitBeforeAttack;

    // Calculate the chase to reset it later
    currentChaseDistance = chaseDistance;
  }

  // Update is called once per frame
  void Update()
  {
    switch (enemyState)
    {
      case EnemyState.PATROL:
        Patrol();
        break;
      case EnemyState.CHASE:
        Chase();
        break;
      case EnemyState.ATTACK:
        Attack();
        break;
    }

  }

  void Patrol()
  {
    // Enable nav agent to move
    navMeshAgent.isStopped = false;
    navMeshAgent.speed = walkSpeed;
    patrolTimer += Time.deltaTime;

    bool needToChangePatrolDirection = patrolTimer > patrolForThisTime;
    bool isEnemyMoving = navMeshAgent.velocity.sqrMagnitude > 0;

    if (needToChangePatrolDirection)
    {
      SetNewRandomDestination();
      patrolTimer = 0f;
    }

    enemyAnimator.Walk(isEnemyMoving);

    // Check if enemy is in range of start chasing
    if (Vector3.Distance(transform.position, target.position) <= chaseDistance)
    {
      enemyAnimator.Walk(false);
      enemyState = EnemyState.CHASE;

      enemyAudio.PlayScreamSound();
    }
  }

  void Chase()
  {
    navMeshAgent.isStopped = false;
    navMeshAgent.speed = runSpeed;
    navMeshAgent.SetDestination(target.position);

    bool isEnemyMoving = navMeshAgent.velocity.sqrMagnitude > 0;
    bool isEnemyInRangeOfAttack = Vector3.Distance(transform.position, target.position) <= attackDistance;
    bool playerRanAway = Vector3.Distance(transform.position, target.position) > chaseDistance;

    enemyAnimator.Run(isEnemyMoving);

    if (isEnemyInRangeOfAttack)
    {
      enemyAnimator.Run(false);
      enemyAnimator.Walk(false);
      enemyState = EnemyState.ATTACK;

      ResetChaseDistance();
    }
    else if (playerRanAway)
    {
      enemyAnimator.Run(false);
      enemyState = EnemyState.PATROL;
      ResetChaseDistance();
    }
  }

  void Attack()
  {
    bool shouldChaseAfterAttacking = Vector3.Distance(transform.position, target.position) > (attackDistance + chaseAfterAttackDistance);
    navMeshAgent.velocity = Vector3.zero;
    navMeshAgent.isStopped = true;
    attackTimer += Time.deltaTime;

    if (attackTimer > waitBeforeAttack)
    {
      transform.LookAt(target);
      enemyAnimator.Attack();
      attackTimer = 0f;

      enemyAudio.PlayAttackSound();
    }

    if (shouldChaseAfterAttacking)
    {
      enemyState = EnemyState.CHASE;
    }
  }

  void SetNewRandomDestination()
  {
    float randomRadius = Random.Range(patrolRadiusMin, patrolRadiusMax);
    Vector3 randomDirection = Random.insideUnitSphere * randomRadius;

    randomDirection += transform.position;

    // Check if random direction is inside walkable area
    NavMeshHit navHit;
    NavMesh.SamplePosition(randomDirection, out navHit, randomRadius, -1);

    navMeshAgent.SetDestination(navHit.position);
  }

  void ResetChaseDistance()
  {
    if (chaseDistance != currentChaseDistance)
      chaseDistance = currentChaseDistance;
  }

  void TurnOnAttackPoint()
  {
    attackPoint.SetActive(true);
  }
  void TurnOffAttackPoint()
  {
    if (attackPoint.activeInHierarchy)
      attackPoint.SetActive(false);
  }

  public EnemyState EnemyState
  {
    get; set;
  }
}

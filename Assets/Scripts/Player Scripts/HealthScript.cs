using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealthScript : MonoBehaviour
{

  private EnemyAnimator enemyAnimator;
  private NavMeshAgent navMeshAgent;
  private EnemyController enemyController;

  public float health = 100f;
  private bool isPlayer, isEnemy;
  private bool isDead;
  private EnemyAudio enemyAudio;
  private PlayerStats playerStats;


  void Awake()
  {
    isPlayer = transform.CompareTag(Tags.PLAYER_TAG);
    isEnemy = transform.CompareTag(Tags.ENEMY_TAG);

    if (isEnemy)
    {
      enemyAnimator = GetComponent<EnemyAnimator>();
      enemyController = GetComponent<EnemyController>();
      navMeshAgent = GetComponent<NavMeshAgent>();
      enemyAudio = GetComponentInChildren<EnemyAudio>();
    }

    if (isPlayer)
    {
      playerStats = GetComponent<PlayerStats>();
    }
  }

  public void ApplyDamage(float damage)
  {
    if (!isDead)
    {
      health -= damage;

      if (isPlayer)
      {
        playerStats.DisplayHealthStats(health);
      }

      if (isEnemy)
      {
        if (enemyController.EnemyState == EnemyState.PATROL)
        {
          enemyController.chaseDistance = 100f;
        }
      }

      if (health <= 0f)
      {
        isDead = true;
        EntityDied();
      }
    }
  }

  void EntityDied()
  {
    if (isEnemy)
    {
      enemyAudio.PlayDeathSound();

      GetComponent<Animator>().enabled = false;
      GetComponent<BoxCollider>().isTrigger = false;
      GetComponent<Rigidbody>().AddTorque(transform.forward * 100f);

      enemyController.enabled = false;
      navMeshAgent.enabled = false;
      enemyAnimator.enabled = false;
    }

    if (isPlayer)
    {
      GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG);

      foreach (GameObject enemy in enemies)
      {
        enemy.GetComponent<EnemyController>().enabled = false;
      }
      GetComponent<PlayerMovement>().enabled = false;
      GetComponent<PlayerAttack>().enabled = false;
      GetComponent<WeaponManager>().GetCurrentSelectedWeapon().gameObject.SetActive(false);
    }
  }

}

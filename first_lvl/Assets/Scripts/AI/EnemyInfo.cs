using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyInfo : MonoBehaviour
{
    [SerializeField] private float enemyHealth;
    public float EnemyHealth
    {
        get => enemyHealth;
        set => enemyHealth = value;
    }

    void GetDamage(float damage)
    {
        if (enemyHealth - damage > 0)
        {
            enemyHealth -= damage;
            // TODO: Hurt animation
        }
        else
            Die();

    }

    void Die()
    {
        // Stop walking
        GetComponent<NavMeshAgent>().isStopped = true;

        // TODO: Die animation

        // Remove from current enemy list
        GameManager.instance.CurrentEnemiesList.Remove(gameObject);

        // Destroy current enemy
        Destroy(gameObject);
    }
}

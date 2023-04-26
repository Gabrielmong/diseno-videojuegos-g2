using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject gameStateObject;

    [SerializeField]
    private float attackRange = 5f;

    [SerializeField]
    private int maxHealth = 100;

    [SerializeField]
    private int currentHealth;

    [SerializeField]
    private Animator animator;

    
    [SerializeField]
    PatrolController patrolController;

    [SerializeField]
    private GameObject healthMessage;

    [SerializeField]
    private Transform healthMessageSpawn;

    [SerializeField]
    private int chanceOfHealing = 10;

    private float nextHealTime = 0f;

    private void Start()
    {
        GetComponent<SphereCollider>().radius = attackRange / 2.7f;
        currentHealth = maxHealth;

    }

    private void Update()
    {
        if (Time.time >= nextHealTime)
        {
            nextHealTime = Time.time + 5f;
            if (Random.Range(0, 100) < chanceOfHealing)
            {
                Heal(5);
            }
        }
    }

    private void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        DisplayHealing(amount);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void onTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player escaped enemy");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        DisplayDamageTaken(damage);

        animator.SetTrigger("Hurt");
        Debug.Log("Enemy took " + damage + " damage. Current health: " + currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Die animation
        animator.SetTrigger("Die");

        // Disable the player
        this.enabled = false;

        // Disable the patrol controller
        patrolController.Disable();
    }

    void DisplayDamageTaken(int damage)
    {
        GameObject message = Instantiate(healthMessage, healthMessageSpawn.position, Quaternion.identity);
        message.GetComponent<TextMesh>().text = damage.ToString();
        message.GetComponent<TextMesh>().color = Color.red;

    }

    void DisplayHealing(int healing)
    {
        GameObject message = Instantiate(healthMessage, healthMessageSpawn.position, Quaternion.identity);
        message.GetComponent<TextMesh>().text = healing.ToString();
        message.GetComponent<TextMesh>().color = Color.green;
    }

}

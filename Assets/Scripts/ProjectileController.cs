using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] 
    private float speed = 10f;

    private Rigidbody rb;

    [SerializeField]
    private float lifeTime = 5f;

    [SerializeField]
    private int maxDamage = 18;

    [SerializeField]
    private int minDamage = 14;
    
    [SerializeField]
    LayerMask whatIsSolid;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.right * speed;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int damage = Random.Range(minDamage, maxDamage);

            Debug.Log("Player hit for " + damage + " damage");
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }

}

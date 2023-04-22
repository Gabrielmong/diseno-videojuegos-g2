using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject gameStateObject;

    [SerializeField]
    private float attackRange = 5f;

    private void Start()
    {
        GetComponent<SphereCollider>().radius = attackRange / 2.7f;
    }


    private void onTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player in attack range");
        }
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
}

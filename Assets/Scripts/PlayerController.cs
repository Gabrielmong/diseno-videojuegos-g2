using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement Components
    CharacterController controller;
    Animator animator;

    private float moveSpeed = 4F;

    [Header("Movement System")]
    public float walkSpeed = 4F;
    public float runSpeed = 8F;

    [Header("Health System")]
    
    [SerializeField]
    private int maxHealth = 100;

    [SerializeField]
    private int currentHealth;


    [Header("Attack System")]
    [SerializeField]
    public int minDamage;

    [SerializeField]
    public int maxDamage;

    [SerializeField]
    private GameObject healthMessage;

    [SerializeField]
    private Transform healthMessageSpawn;
    

    // Start is called before the first frame update
    void Start()
    {
        // Get movements components
        controller= GetComponent<CharacterController>();
        animator= GetComponent<Animator>();
        currentHealth = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        // Runs is called once per frame
        Move();
        HandleClick();


        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        } else if (Input.GetKeyDown(KeyCode.R))
        {
            Heal(10);
        }
    }

    public void Move()
    {
        // Is the sprint key pressed down?
        if (Input.GetButton("Sprint"))
        {
            // Set the animaion to run and increases our movespeed
            moveSpeed = runSpeed;
            animator.SetBool("Run", true);
        }
        else
        {
            // Set the animaion to walk and increases our movespeed
            moveSpeed = walkSpeed;
            animator.SetBool("Run", false);
        }

        // Get the horizontal and vertical inputs as a number
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Direction in a normalized vector
        Vector3 dir = new Vector3(horizontal, 0.0F, vertical).normalized;
        Vector3 velocity = moveSpeed * Time.deltaTime * dir;

        // Check if there is movement
        if (dir.magnitude >= 0.1F)
        {
            // Look towards that direction smoothly
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 0.15F);
            

            // Move
            controller.Move(velocity);
        }

        // Animator speed parameter
        animator.SetFloat("Speed", dir.magnitude);
    }

    public void HandleClick(){

        if (Input.GetMouseButtonDown(0))
        {
            // cast a ray down, if the layer is grid, play the crop picking animation, else attack
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log(hit.collider.gameObject.layer);
                if (hit.collider.gameObject.tag == "grid")
                {
                    animator.SetBool("isPulling", true);
                    StartCoroutine(Delay());
                }
                else
                {
                    animator.SetBool("isPulling", false);
                        
                    Attack();
                }
            }
        }
    }

    public void Attack()
    {
        animator.SetBool("Attack", true);
        
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.1F);
        animator.SetBool("Attack", false);
        animator.SetBool("isPulling", false);
        animator.SetBool("isDamaged", false);
    }

    public void DealDamage() {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 0.3F);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.GetComponent<EnemyController>())
            {
                int damage = Random.Range(minDamage, maxDamage);
                enemy.GetComponent<EnemyController>().TakeDamage(damage);
            }
        }
    }

    

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        DisplayDamageTaken(damage);
        animator.SetBool("isDamaged", true);
        StartCoroutine(Delay());
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

    public void Heal(int healing)
    {
        currentHealth += healing;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        DisplayHealing(healing);
    }

}

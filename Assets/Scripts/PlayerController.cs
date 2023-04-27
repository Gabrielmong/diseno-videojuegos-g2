using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
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

    public HealthBar healthBar;

    [SerializeField]
    private TextMeshProUGUI healthPotionsText;

    [SerializeField]
    private int healthPotions = 3;

    [Header("Attack System")]
    [SerializeField]
    public int minDamage;

    [SerializeField]
    public int maxDamage;

    [SerializeField]
    private GameObject healthMessage;

    [SerializeField]
    private Transform healthMessageSpawn;

    [SerializeField]
    private AudioClip[] woodStepsSounds;

    [SerializeField]
    private AudioClip[] grassStepsSounds;

    [SerializeField]
    private AudioClip[] sandStepsSounds;

    [SerializeField]
    private AudioClip punchSound;

    [SerializeField]
    private AudioClip hitSound;

    private bool inNpcRange = false;
    private bool freezed = false;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get movements components
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
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
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (healthPotions > 0)
            {
                healthPotions--;
                currentHealth += 20;
                healthBar.SetHealth(currentHealth);
                healthPotionsText.text = healthPotions.ToString();
                DisplayHealing(20);
            }
        }
    }

    public void Move()
    {
        if (freezed)
        {
            return;
        }

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

    public void HandleClick()
    {

        if (Input.GetMouseButtonDown(0))
        {
            // cast a ray down, if the layer is grid, play the crop picking animation, else attack
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "grid")
                {
                    //face player towards the clicked grid
                    transform.LookAt(hit.collider.gameObject.transform.position);
                    animator.SetBool("isPulling", true);
                    StartCoroutine(Delay());
                }
                else
                {
                    animator.SetBool("isPulling", false);

                    // if the player is on an NPC collider,dont attack
                    if (inNpcRange)
                    {
                        return;
                    }

                    Attack();
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "grid")
                {
                    transform.LookAt(hit.collider.gameObject.transform.position);
                    animator.SetBool("isPlanting", true);
                    StartCoroutine(Delay());
                }
                else
                {
                    animator.SetBool("isPlanting", false);
                }
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            inNpcRange = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            inNpcRange = false;
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
        animator.SetBool("isPlanting", false);
        animator.SetBool("isDamaged", false);
    }

    public void DealDamage()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 0.1F);


        if (hitEnemies.Length == 0)
        {
            AudioManager.Instance.PlaySoundAtPosition(punchSound, transform.position);
        }


        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.tag == "Enemy")
            {
                Vector3 enemyPosition = enemy.transform.position;	
                Vector3 playerPosition = transform.position;
                Vector3 playerForward = transform.forward;
                Vector3 playerToEnemy = enemyPosition - playerPosition;
                float angle = Vector3.Angle(playerForward, playerToEnemy);
                if (angle < 45)
                {
                    if (enemy.GetComponent<EnemyController>())
                    {
                        int damage = Random.Range(minDamage, maxDamage);
                        enemy.GetComponent<EnemyController>().TakeDamage(damage);
                        AudioManager.Instance.PlaySoundAtPosition(hitSound, transform.position);
                    }
                }
            }
        }
    }


    public void TakeDamage(int damage)
    {
        // freeze player movement
        freezed = true;

        currentHealth -= damage;
        DisplayDamageTaken(damage);
        animator.SetBool("isDamaged", true);
        StartCoroutine(Delay());
        StartCoroutine(DamageDelay());

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator DamageDelay()
    {
        yield return new WaitForSeconds(2F);
        freezed = false;
    }

    void Die()
    {
        // Die animation
        animator.SetBool("isDead", true);

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

        healthBar.SetHealth(currentHealth);
    }

    public void Step()
    {
        // check if the player is on wood or grass
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.5F))
        {
            if (hit.collider.gameObject.tag == "Roads")
            {
                // play a random wood sound
                int index = Random.Range(0, woodStepsSounds.Length);
                AudioManager.Instance.PlaySoundAtPosition(woodStepsSounds[index], transform.position);
            }
            else if (hit.collider.gameObject.tag == "Ground" || hit.collider.gameObject.tag == "grid")
            {
                int index = Random.Range(0, grassStepsSounds.Length);
                AudioManager.Instance.PlaySoundAtPosition(grassStepsSounds[index], transform.position);
            }
            else if (hit.collider.gameObject.tag == "Sand")
            {
                int index = Random.Range(0, sandStepsSounds.Length);
                AudioManager.Instance.PlaySoundAtPosition(sandStepsSounds[index], transform.position);
            }
        }
    }

    public void AddHealthPotions(int amount)
    {
        healthPotions += amount;
        healthPotionsText.text = healthPotions.ToString();
    }

}

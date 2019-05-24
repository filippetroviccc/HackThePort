using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;
    [SerializeField] private Animator anim;

    public float attackWaitingTime = 0;

    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    bool playerInRange;
    float timer;

    public EnemyMovement movement;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("player in range");
        if (other.gameObject == player)
        {
            playerInRange = true;
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            playerInRange = false;
        }
    }


    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenAttacks && playerInRange /* && enemyHealth.currentHealth > 0*/)
        {
            StopMovingNAttack(attackWaitingTime);
        }

        if (playerHealth.currentHealth <= 0)
        {
            //anim.SetTrigger("PlayerDead");
        }
    }

    private void StopMovingNAttack(float time)
    {
        movement.enabled = false;
        anim.Play("Attack");

        StartCoroutine(ReactivateMovement(time));
    }

    private IEnumerator ReactivateMovement(float time)
    {
        yield return new WaitForSeconds(time);
        Attack();
        movement.enabled = true;
    }

    void Attack()
    {
        timer = 0f;

        if (playerHealth.currentHealth > 0 && playerInRange)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }
}
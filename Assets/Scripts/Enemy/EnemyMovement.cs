using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    Transform player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    NavMeshAgent nav;
    public float rotationSpeed;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        nav = GetComponent<NavMeshAgent>();
        nav.updateRotation = false;
        nav.updateUpAxis = false;
    }


    void Update()
    {
        if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
        {
            LookAtPlayer();
            nav.SetDestination(player.position);
        }
        else
        {
            nav.enabled = false;
        }
    }

    private void LookAtPlayer()
    {
        Vector3 dir = player.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

        transform.rotation =
            Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward),
                rotationSpeed * Time.deltaTime);
    }
}
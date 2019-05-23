using System.Collections;
using UnityEngine;

public class EnemyImpact : MonoBehaviour
{
    public int EnemyDamage;

    private void Start()
    {
        StartCoroutine(DestroyThisObject());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        if (enemyHealth != null) enemyHealth.TakeDamage(EnemyDamage, Vector3.zero);

        if (!other.gameObject.CompareTag("Player")) Destroy(gameObject);
    }

    private IEnumerator DestroyThisObject()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
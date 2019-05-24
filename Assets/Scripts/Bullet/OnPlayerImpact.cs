using System.Collections;
using UnityEngine;

public class OnPlayerImpact : MonoBehaviour
{
    public int damage;

    private void Start()
    {
        StartCoroutine(DestroyThisObject());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.gameObject.GetComponent<PlayerHealth>();
        if (player != null) player.TakeDamage(damage);

        if (!other.gameObject.CompareTag("Enemy") && !other.isTrigger) Destroy(gameObject);
    }

    private IEnumerator DestroyThisObject()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    private readonly List<Collider2D> collidersInside = new List<Collider2D>();

    public GameObject slashObject;
    public int attackDamage = 20;

    public void Attack()
    {
        slashObject.SetActive(true);

        foreach (var col in collidersInside)
        {
            var colObj = col.gameObject;
            EnemyHealth enemyHealth = colObj.GetComponent<EnemyHealth>();
            if (enemyHealth)
            {
                enemyHealth.TakeDamage(attackDamage, Vector3.zero);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!collidersInside.Contains(other))
        {
            collidersInside.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        collidersInside.Remove(other);
    }
}

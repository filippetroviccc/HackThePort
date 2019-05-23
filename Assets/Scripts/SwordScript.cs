using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    private readonly List<Collider2D> collidersInside = new List<Collider2D>();

    public void Attack()
    {
        foreach (var col in collidersInside)
        {
            var colObj = col.gameObject;
            if (colObj.tag.Equals("Enemy")) ; // TODO take damage
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!collidersInside.Contains(other)) collidersInside.Add(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        collidersInside.Remove(other);
    }
}
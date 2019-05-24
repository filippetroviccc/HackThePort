﻿using System.Collections;
using UnityEngine;

public class EnemyGunScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float force;
    public float coolDown = 2;
    private bool canShoot = true;
    private GameObject player;

    public void Start()
    {
        player = GameObject.Find("Player");
    }

    public void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.name == "Player" && canShoot) Fire();
        }
    }

    public void Fire()
    {
        Debug.Log("FIRE");
        var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * force);
        canShoot = false;
        StartCoroutine(CooldownCorout());
    }

    private IEnumerator CooldownCorout()
    {
        yield return new WaitForSeconds(coolDown);
        canShoot = true;
    }
}
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public GameObject bulletPrefab;

    public float force;

    public void Fire()
    {
        var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * force);
    }
}
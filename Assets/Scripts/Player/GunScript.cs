using UnityEngine;

public class GunScript : MonoBehaviour
{
    public GameObject bulletPrefab;

    public int maxNumOfBullets;
    public int currNumOfBullets;

    public float force;

    [SerializeField] private GameObject bulletShower;

    public void Start()
    {
        Reload();
    }

    private void Reload()
    {
        currNumOfBullets = maxNumOfBullets;

        UpdateHud();
    }

    public void Fire()
    {
        var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * force);
        currNumOfBullets--;

        UpdateHud();
    }

    private void UpdateHud()
    {
    }
}
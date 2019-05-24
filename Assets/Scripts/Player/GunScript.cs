using UnityEngine;

public class GunScript : MonoBehaviour
{
    public GameObject bulletPrefab;

    public int maxNumOfBullets;
    public int currNumOfBullets;

    public float force;

    [SerializeField] private GameObject bulletShower;
    [SerializeField] private Animator animator;

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
        animator.Play("player_gunAttacking");

        UpdateHud();
    }

    private void UpdateHud()
    {
    }
}
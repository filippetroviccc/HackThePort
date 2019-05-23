using UnityEngine;

public class Player : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int IsSwordAttacking = Animator.StringToHash("isSwordAttacking");
    private static readonly int IsGunAttacking = Animator.StringToHash("isGunAttacking");

    public int health = 100;
    public GameObject swordObject;
    public GameObject gunObject;

    public Animator animator;
    public float speed;


    void Update()
    {
        void HandleMovement()
        {
            var isMoving = false;

            void Move(Vector2 dir)
            {
                transform.Translate(dir * speed, Space.World);
                isMoving = true;
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) Move(Vector2.up);
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) Move(Vector2.down);

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) Move(Vector2.left);
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) Move(Vector2.right);


            animator.SetBool(IsMoving, isMoving);
        }

        void RotateTowardsMouse()
        {
            var mouse = Input.mousePosition;
            var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
            var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
            var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }

        void HandleAttack()
        {
            var isSwordAttacking = false;
            var isGunAttacking = false;

            void AttackWithSword()
            {
                swordObject.GetComponent<SwordScript>().Attack();
                isSwordAttacking = true;

            }

            void AttackWithGun()
            {
                gunObject.GetComponent<GunScript>().Fire();
                isGunAttacking = true;
            }

            if (Input.GetMouseButtonDown(0)) AttackWithSword();
            else if (Input.GetMouseButtonDown(1)) AttackWithGun();

            animator.SetBool(IsSwordAttacking, isSwordAttacking);
            animator.SetBool(IsGunAttacking, isGunAttacking);
        }

        HandleMovement();
        RotateTowardsMouse();
        HandleAttack();
    }
}
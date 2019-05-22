using UnityEngine;

public class Player : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int IsSwordAttacking = Animator.StringToHash("isSwordAttacking");
    private static readonly int IsGunAttacking = Animator.StringToHash("isGunAttacking");

    public Animator animator;
    public float speed;

    void Start()
    {
    }

    void Update()
    {
        void HandleMovement()
        {
            var isMoving = false;

            void Move(Vector2 dir)
            {
                transform.Translate(dir * speed);
                isMoving = true;
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) Move(Vector2.up);
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) Move(Vector2.down);

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) Move(Vector2.left);
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) Move(Vector2.right);


            animator.SetBool(IsMoving, isMoving);
        }

        void HandleAttack()
        {
            var isSwordAttacking = false;
            var isGunAttacking = false;

            void AttackWithSword()
            {
                void SwingSword()
                {
                }

                SwingSword();
                isSwordAttacking = true;
            }

            void AttackWithGun()
            {
                void FireBullet()
                {
                }

                FireBullet();
                isGunAttacking = true;
            }

            if (Input.GetMouseButtonDown(0)) AttackWithSword();
            else if (Input.GetMouseButtonDown(1)) AttackWithGun();

            animator.SetBool(IsSwordAttacking, isSwordAttacking);
            animator.SetBool(IsGunAttacking, isGunAttacking);
        }

        HandleMovement();
        HandleAttack();
    }
}
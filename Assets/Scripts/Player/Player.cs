using UnityEngine;

public class Player : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int IsSwordAttacking = Animator.StringToHash("isSwordAttacking");
    private static readonly int IsGunAttacking = Animator.StringToHash("isGunAttacking");

    public GameObject swordObject;
    public GameObject gunObject;

    public Animator animator;
    public float speed;
    [SerializeField] double rotateAnimationTolerance = 0.001f;

    private GunScript gunScript;

    private void Start()
    {
        gunScript = gunObject.GetComponent<GunScript>();
    }

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
            var newAngle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg - 90; //weird offset

            var lastRotation = transform.rotation;
            var newRotation = transform.rotation = Quaternion.Euler(0, 0, newAngle);

            var angleDiff = Quaternion.Angle(newRotation, lastRotation) - 90; //weird offset

            if (!animator.GetBool(IsMoving))
                animator.SetBool(IsMoving, angleDiff > rotateAnimationTolerance);
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
                gunScript.Fire();
                isGunAttacking = true;
            }

            if (Input.GetMouseButtonDown(0)) AttackWithSword();
            else if (Input.GetMouseButtonDown(1) && gunScript.currNumOfBullets > 0) AttackWithGun();

            animator.SetBool(IsSwordAttacking, isSwordAttacking);
            animator.SetBool(IsGunAttacking, isGunAttacking);
        }

        HandleMovement();
        RotateTowardsMouse();
        HandleAttack();
    }
}
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int IsSwordAttacking = Animator.StringToHash("isSwordAttacking");
    private static readonly int IsGunAttacking = Animator.StringToHash("isGunAttacking");

    public GameObject swordObject;
    public GameObject gunObject;
    public int gunCooldown = 3;
    public Slider gunColdownSlider;

    public Animator animator;
    [SerializeField] private PlayerAudioController audioController;

    public float speed;
    [SerializeField] private double rotateAnimationTolerance = 0.001f;

    private GunScript gunScript;
    private bool gunReady = true;

    private void Start()
    {
        gunScript = gunObject.GetComponent<GunScript>();
    }

    void Update()
    {
        var isMoving = false;
        

        void HandleMovement()
        {
            var dir = Vector2.zero;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) dir += Vector2.up;
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) dir += Vector2.down;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) dir += Vector2.left;
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) dir += Vector2.right;

            dir.Normalize();

            if (dir != Vector2.zero) //player wants to move
            {
                transform.Translate(dir * speed, Space.World);
                audioController.PlayWalkSound();
                isMoving = true;
            }

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

            if (Input.GetMouseButtonDown(0))
            {
                swordObject.GetComponent<SwordScript>().Attack();
                audioController.PlaySwordSound();
                isSwordAttacking = true;
            }

            else if (Input.GetMouseButtonDown(1))
            { 
                if(gunReady)
                {
                    gunScript.Fire();
                    audioController.PlayGunSounds();
                    StartCoroutine(coolDownGun());
                    isGunAttacking = true;
                }
                return;
            }

            animator.SetBool(IsSwordAttacking, isSwordAttacking);
            animator.SetBool(IsGunAttacking, isGunAttacking);

        }

        HandleMovement();
        RotateTowardsMouse();
        HandleAttack();

    }
        private IEnumerator coolDownGun()
        {
            gunReady = false;
            float elipsedTime = 0;
        Color initialColor = gunColdownSlider.image.color;
            while (elipsedTime <= gunCooldown)
            {
                elipsedTime += Time.deltaTime;
                gunColdownSlider.value = elipsedTime / gunCooldown;
                gunColdownSlider.image.color = Color.Lerp(Color.red, initialColor, gunColdownSlider.normalizedValue);
                yield return 13;
            }
        gunColdownSlider.image.color = initialColor;
        gunReady = true;


    }
}
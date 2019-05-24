using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed;

    public Rigidbody2D shipBody;

    void Update()
    {
        void HandleMovement()
        {
            var leftRotation = 1;
            var rightRotation = -1;

            void Move(Vector2 dir)
            {
                shipBody.AddForce(dir * movementSpeed);
            }

            void Rotate(int dir)
            {
                shipBody.angularVelocity += dir * rotationSpeed;
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) Move(transform.up);
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) Move(-transform.up);

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) Rotate(leftRotation);
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) Rotate(rightRotation);
        }

        HandleMovement();
    }

    private void HandleInput()
    {
        //Console.Write(Input.GetAxis("Horizontal"));
    }
}
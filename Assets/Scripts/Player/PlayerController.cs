using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField, Range(1f,10f)] private float speed;
        [SerializeField, Range(1f,10f)] private float rotateForwardSpeed;
        [SerializeField] private bool useWasd;
        private SpriteRenderer sprite;

        private void Start()
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
        }
        private void Update()
        {
            if (useWasd) WasdMovement();
            else MouseMovement();
        }

        // Expenisve
        private void WasdMovement()
        {
            Vector2 inputVector = new Vector2();
            if (Input.GetKey(KeyCode.W))
            {
                inputVector.y += 1;
            }

            if (Input.GetKey(KeyCode.S))
            {
                inputVector.y -= 1;
            }

            if (Input.GetKey(KeyCode.D))
            {
                inputVector.x += 1;
            }

            if (Input.GetKey(KeyCode.A))
            {
                inputVector.x -= 1;
            }
            
            Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, 0f).normalized;
            transform.position += moveDir * (speed * Time.deltaTime);
            RotatePlayerToFoward(moveDir);
        }

        private void MouseMovement()
        {
            
        }
        
        private void RotatePlayerToFoward(Vector3 moveDirection)
        {
            // Rotate the player to the current forward position
            
            transform.up = Vector3.Slerp(transform.up, moveDirection,
                rotateForwardSpeed * Time.deltaTime);
        }
    }
}


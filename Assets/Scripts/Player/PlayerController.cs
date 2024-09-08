using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private bool useWasd;
        
        [SerializeField] private enum SpriteRotation
        {
            MoveUp,
            MoveDown,
            MoveLeft,
            MoveRight,
        }
        private SpriteRotation _spriteRotation;

        private SpriteRenderer sprite;

        private void Start()
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
        }
        private void Update()
        {
            if (useWasd) WasdMovement();
            else MouseMovement();
            
            switch (_spriteRotation)
            {
                case SpriteRotation.MoveUp:
                    sprite.transform.rotation = Quaternion.Euler(0f,0f,0f);
                    break;
                case SpriteRotation.MoveDown:
                    sprite.transform.rotation = Quaternion.Euler(0f,0f,-180f);
                    break;
                case SpriteRotation.MoveLeft:
                    sprite.transform.rotation = Quaternion.Euler(0f,0f,90f);
                    break;
                case SpriteRotation.MoveRight:
                    sprite.transform.rotation = Quaternion.Euler(0f,0f,-90f);
                    break;
            }
        }

        // Expenisve
        private void WasdMovement()
        {
            Vector2 inputVector = new Vector2();
            if (Input.GetKey(KeyCode.W))
            {
                inputVector.y += 1;
                _spriteRotation = SpriteRotation.MoveUp;
            }

            if (Input.GetKey(KeyCode.S))
            {
                inputVector.y -= 1;
                _spriteRotation = SpriteRotation.MoveDown;
            }

            if (Input.GetKey(KeyCode.D))
            {
                inputVector.x += 1;
                _spriteRotation = SpriteRotation.MoveRight;
            }

            if (Input.GetKey(KeyCode.A))
            {
                inputVector.x -= 1;
                _spriteRotation = SpriteRotation.MoveLeft;
            }
            
            Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, 0f).normalized;
            transform.position += moveDir * (speed * Time.deltaTime);
        }

        private void MouseMovement()
        {
            
        }
    }
}


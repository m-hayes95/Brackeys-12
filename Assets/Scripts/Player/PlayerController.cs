using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private bool useWasd;
        private void Update()
        {
            if (useWasd) WasdMovement();
            else MouseMovement();
        }

        // Expenisve
        private void WasdMovement()
        {
            Vector2 inputVector = new Vector2();
            if (Input.GetKey(KeyCode.W)) inputVector.y += 1;
            if (Input.GetKey(KeyCode.S)) inputVector.y -= 1;
            if (Input.GetKey(KeyCode.D)) inputVector.x += 1;
            if (Input.GetKey(KeyCode.A)) inputVector.x -= 1;
            Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, 0f).normalized;
            transform.position += moveDir * (speed * Time.deltaTime);
        }

        private void MouseMovement()
        {
        
        }
    
    }
}


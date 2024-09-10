using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField, Range(1f,10f)] private float speed;
        [SerializeField, Range(1f,100f), Tooltip("Set the player rotate speed when using WASD only")] 
        private float rotateForwardSpeedWasd;
        [SerializeField] private bool useWasd;
        [SerializeField, Tooltip("Set mouse controls to use Lerp (does not effect wasd)")] 
        private bool useMouseLerp;
        private Rigidbody2D _rb;

        private void Start()
        {
            
        }
        private void FixedUpdate()
        {
            if (useWasd) WasdMovement();
            else MouseMovement();
        }

        #region WASD Controls
        // Expensive
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
            RotatePlayerToForward(moveDir);
        }
        private void RotatePlayerToForward(Vector3 moveDirection)
        {
            // Rotate the player to the current forward position
            
            transform.up = Vector3.Slerp(transform.up, moveDirection,
                rotateForwardSpeedWasd * Time.deltaTime);
        }
        #endregion

        #region Mouse Controls
        private void MouseMovement()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log(mousePosition);
            if (!useMouseLerp)
                transform.position = Vector2.MoveTowards(
                    transform.position, mousePosition, speed * Time.deltaTime
                );
            else
                transform.position = Vector2.Lerp(
                    transform.position, mousePosition, speed * Time.deltaTime
                );
            MouseRotation();
        }
        private void MouseRotation()
        {
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; // need to change
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
            transform.rotation = rotation;
        }
        #endregion
    }
}
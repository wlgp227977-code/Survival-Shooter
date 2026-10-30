using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static readonly int HashMove = Animator.StringToHash("Move");  
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    private Camera mainCamera;    
    private Animator playerAnimator;
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;

    private void Awake()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        playerAnimator.SetFloat(HashMove, Mathf.Abs(playerInput.MoveX) + Mathf.Abs(playerInput.MoveZ));
        RotateToMouse();
    }
    
    private void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(playerInput.MoveZ, 0f, playerInput.MoveX);
        moveDirection = moveDirection.normalized;

        playerRigidbody.MovePosition(playerRigidbody.position + 
        moveDirection * moveSpeed * Time.fixedDeltaTime);
    }
    private void RotateToMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        Plane plane = new Plane(Vector3.up, transform.position);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 targetPosition = ray.GetPoint(distance);
            Vector3 direction = targetPosition - transform.position;

            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                float targetY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

                float currentY = transform.eulerAngles.y;

                float newY = Mathf.LerpAngle(
                    currentY,
                    targetY,
                    rotationSpeed * Time.deltaTime
                );

                transform.rotation = Quaternion.Euler(0f, newY, 0f);
            }
        }
    }
}

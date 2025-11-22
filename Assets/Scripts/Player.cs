using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movingSpeed = 10f;
    private PlayerInputAction playerAction;
    private Rigidbody2D rb;
    //public VectorValue pos;
    //private void Start()
    //{
        //transform.position = pos.initialValue;
    //}
    private void Awake()
    {
        playerAction = new PlayerInputAction();
        playerAction.Enable();
        rb = GetComponent<Rigidbody2D>();
    }
    private Vector2 GetMovementSpeed()
    {
        Vector2 inputAction = playerAction.Player.Move.ReadValue<Vector2>();
        return inputAction;
    }
    private void FixedUpdate()
    {
        Vector2 inputVector = GetMovementSpeed();
        inputVector = inputVector.normalized;
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.deltaTime));
    }
    
}

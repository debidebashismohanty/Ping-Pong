using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    [SerializeField] bool _IsPlayerOne; // Flag to determine if this is Player One or not
    [SerializeField] private float _speed; // Movement speed of the player

    private Rigidbody2D _rigidbody; // Reference to the Rigidbody component
    private float _movement; // Movement input value

    private Vector3 _StartPostition; // Initial position of the player
    private int _Player1Life = 100; // Player One's initial life
    public int _Player2Life = 100; // Player Two's initial life

    // Properties to access player life values
    public int Player1Life { set => _Player1Life = value; get => _Player1Life; }
    public int Player2Life { set => _Player2Life = value; get => _Player2Life; }

    private void Start()
    {
        _StartPostition = transform.position; // Store the initial position of the player
        _rigidbody = GetComponent<Rigidbody2D>(); // Get the Rigidbody component
    }

    void Update()
    {
        // Get vertical movement input based on the player
        if (_IsPlayerOne)
            _movement = Input.GetAxisRaw("Vertical");
        else
            _movement = Input.GetAxisRaw("ArrowVertical");

        // Set the velocity of the player based on the movement input and speed
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _movement * _speed);
    }

    // Reset the player to its initial position and stop its movement
    public void ResetPlayer()
    {
        transform.position = _StartPostition; // Reset the position of the player
        _rigidbody.velocity = Vector2.zero; // Stop the player's movement
    }
}

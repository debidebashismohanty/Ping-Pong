using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] float _speed; // Speed of the ball
    Vector3 _position; // Initial position of the ball

    private Rigidbody2D _rigidbody;

    void Start()
    {
        _position = transform.position; // Store the initial position of the ball

        _rigidbody = GetComponent<Rigidbody2D>(); // Get the Rigidbody component
    }

    // Launch the ball in a random direction
    private void LaunchBall()
    {
        // Generate random x and y directions
        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Random.Range(0, 2) == 0 ? -1 : 1;

        // Set the velocity of the ball based on the random directions and speed
        _rigidbody.velocity = new Vector3(_speed * x, _speed * y);
    }

    // Reset the ball to its initial position and launch it again
    public void ResetBall()
    {
        // Reset the position of the ball
        transform.position = _position;

        // Reset the velocity of the ball to zero
        _rigidbody.velocity = Vector2.zero;

        // Launch the ball again
        LaunchBall();
    }
}

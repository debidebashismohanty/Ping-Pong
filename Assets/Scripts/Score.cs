using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private bool IsPlayer1ScoreWall; // Determines if this is the Player 1 score wall

    // Called when a 2D collider enters this trigger collider
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is tagged as "Ball"
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Get the GameManager and call PlayerOneScored() or PlayerTwoScored() based on IsPlayer1ScoreWall
            if (IsPlayer1ScoreWall)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().PlayerTwoScored();
            }
            else
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().PlayerOneScored();
            }
        }
    }
}

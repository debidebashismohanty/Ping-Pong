using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreStorage : MonoBehaviour
{
    private static ScoreStorage instance; // Singleton instance of ScoreStorage
    private GameManager gameManager; // Reference to the GameManager
    private int PlayerOneScore; // Player One's score
    private int PlayerTwoScore; // Player Two's score

    private TextMeshProUGUI player1ScoreText; // Text for Player One's score
    private TextMeshProUGUI player2ScoreText; // Text for Player Two's score

    private GameObject canvas; // Reference to the canvas GameObject

    // Singleton pattern for ScoreStorage
    public static ScoreStorage Instance
    {
        get
        {
            if (instance == null)
            {
                // Find an existing instance of ScoreStorage
                instance = FindObjectOfType<ScoreStorage>();
                if (instance == null)
                {
                    // Create a new GameObject to hold the ScoreStorage instance
                    GameObject singletonObject = new GameObject("ScoreStorageSingleton");
                    instance = singletonObject.AddComponent<ScoreStorage>();
                    DontDestroyOnLoad(singletonObject); // Ensure ScoreStorage persists across scenes
                }
            }
            return instance;
        }
    }

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Ensure only one instance of ScoreStorage exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Ensure ScoreStorage persists across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate ScoreStorage instances
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Find the GameManager GameObject and get its GameManager component
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Update PlayerOneScore and PlayerTwoScore from GameManager if available
        if (gameManager != null)
        {
            PlayerOneScore = gameManager.Player1Score;
            PlayerTwoScore = gameManager.Player2Score;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            // Find the GameManager GameObject and get its GameManager component when in Scene 1
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        // Initialize UI elements when in Scene 2
        if (canvas == null && SceneManager.GetActiveScene().buildIndex == 2)
        {
            // Find the canvas GameObject and get the TextMeshProUGUI components for player scores
            canvas = GameObject.Find("ScoreCanvas");
            player1ScoreText = canvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            player2ScoreText = canvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            // Update player score text based on PlayerOneScore and PlayerTwoScore
            player1ScoreText.text = $"Player One Scored {PlayerOneScore} Points";
            player2ScoreText.text = $"Player Two Scored {PlayerTwoScore} Points";
        }
    }
}

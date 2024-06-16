using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    float count = 4; // Countdown timer duration
    public int GameCount = 1; // Current round count
    int scoreDecrement = 10; // Amount to decrement player health when scoring

    public UnityAction OnGameOver; // Event invoked when game over
    public UnityAction OnAnyPlayerDead; // Event invoked when any player dies

    [Header("Players")]
    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] GameObject PlayerOne;
    [SerializeField] GameObject PlayerTwo;
    private int _playerOneScore;
    private int _playerTwoScore;

    [Header("Ball")]
    [SerializeField] GameObject ball;

    [Header("UI Updates")]
    [SerializeField] TextMeshProUGUI PlayerOneScore;
    [SerializeField] TextMeshProUGUI PlayerTwoScore;
    [SerializeField] Slider _player1LifeSlider;
    [SerializeField] Slider _player2LifeSlider;
    [SerializeField] TextMeshProUGUI _CountDownText;
    [SerializeField] TextMeshProUGUI helthTextPlayer1;
    [SerializeField] TextMeshProUGUI HelthTextPlayer2;
    [SerializeField] TextMeshProUGUI roundText;
    [SerializeField] TextMeshProUGUI ScoreText1;
    [SerializeField] TextMeshProUGUI scoreText2;
    [SerializeField] GameObject ScorePannel;
    [SerializeField] TextMeshProUGUI roundWinnerText;
    [SerializeField] TextMeshProUGUI roundScoreText;


    private PlayerMovements _player1movements;
    private PlayerMovements _player2movements;
    private Ball _ball;

    public int Player1Score { get { return _playerOneScore; } }
    public int Player2Score { get { return _playerTwoScore; } }

    private void Start()
    {
        // Get references to player movements and ball
        _player1movements = PlayerOne.GetComponent<PlayerMovements>();
        _player2movements = PlayerTwo.GetComponent<PlayerMovements>();
        _ball = ball.GetComponent<Ball>();

        // Initialize UI elements and events
        HelthTextPlayer2.text = _player2movements.Player2Life.ToString();
        helthTextPlayer1.text = _player1movements.Player1Life.ToString();
        OnAnyPlayerDead += OnAnyPlayerIsDead;
        OnGameOver += GameOver;
        roundText.text = "Round " + GameCount.ToString();
        StartCoroutine(CountDown(count));
    }

    public void PlayerOneScored()
    {
        _particleSystem.Play();
        _playerOneScore++;
        PlayerOneScore.text = _playerOneScore.ToString();
        _player2movements.Player2Life -= scoreDecrement;
        UpdatePlayerHealth();

        if (_player2movements.Player2Life <= 0)
            OnAnyPlayerDead.Invoke();
        else
            StartCoroutine(CountDown(count));

      
        PlayerOneScore.text = _playerOneScore.ToString();
    }

    public void PlayerTwoScored()
    {
        _particleSystem.Play();
        _playerTwoScore++;
        PlayerTwoScore.text = _playerTwoScore.ToString();
        _player1movements.Player1Life -= scoreDecrement;
        UpdatePlayerHealth();

        if (_player1movements.Player1Life <= 0)
            OnAnyPlayerDead.Invoke();
        else
            StartCoroutine(CountDown(2));

      
        PlayerTwoScore.text = _playerTwoScore.ToString();
    }


    // Update UI with player health values
    public void UpdatePlayerHealth()
    {
        _player1LifeSlider.value = _player1movements.Player1Life;
        helthTextPlayer1.text = _player1movements.Player1Life.ToString();

        _player2LifeSlider.value = _player2movements.Player2Life;
        HelthTextPlayer2.text = _player2movements.Player2Life.ToString();
    }

    // Restart the game by resetting players and ball
    public void RestartGame()
    {
        _player1movements.ResetPlayer();
        _player2movements.ResetPlayer();
        _ball.ResetBall();
    }

    // Countdown coroutine for round start
    public IEnumerator CountDown(float count)
    {
        float timer = count;

        while (timer >= -1f)
        {
            if (timer <= 0)
                _CountDownText.text = "Start";
            else
                _CountDownText.text = Mathf.CeilToInt(timer).ToString();

            timer -= Time.deltaTime;
            yield return null;
        }
        //For diabling all UI's
        DisableUI();
        RestartGame();
    }

    // Called when any player is dead
    public void OnAnyPlayerIsDead()
    {
        GameCount++;
        if (GameCount == 4)
        {
            PersentageCalculation(GameCount-1);
            OnGameOver.Invoke();
            Debug.Log("Game Over");
        }
        else
        {
            _player1movements.Player1Life = 100;
            _player2movements.Player2Life = 100;
            UpdatePlayerHealth();
           
            StartCoroutine(CountDown(count));
            Debug.Log(GameCount.ToString() + " Round");
            roundText.text = "Round " + GameCount.ToString();

            PersentageCalculation(GameCount-1);

            EnableUI();
            
        }
    }

  


    #region ENABLE AND DISABLE
    //Call when UI need to enable
    private void EnableUI()
    {
        _CountDownText.enabled = true;
        roundText.enabled = true;
        roundWinnerText.enabled = true;
        roundScoreText.enabled = true;
    }
    private void DisableUI()
    {
        _CountDownText.enabled = false;
        roundText.enabled = false;
        roundWinnerText.enabled = false;
        roundScoreText.enabled = false;
    }

    #endregion

    // Called when the game is over
    public void GameOver()
    {
        ScorePannel.SetActive(true);
        ScoreText1.text = "Player 1 : " + _playerOneScore.ToString();
        scoreText2.text = "Player 2 : " + _playerTwoScore.ToString();
     


        StartCoroutine(OnGameIsOver());
    }

    // Coroutine for handling game over state
    IEnumerator OnGameIsOver()
    {
        float timer = 5;

        while (timer >= -1f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void PersentageCalculation(int GameCount)
    {
        switch(GameCount)
        {
            case 0: if(_playerOneScore == _playerTwoScore)
                {
                    roundWinnerText.text = $"Both of you are Round {GameCount} Winners" ;
                    roundScoreText.text = $"Player 1 and 2 has {50}% chance to win"; 


                }
                break;
            case 1:
                if (_playerOneScore == _playerTwoScore)
                {
                    roundWinnerText.text = $"Both of you are Round {GameCount} Winners";
                    roundScoreText.text = $"Player 1 and 2 has {50}% chance to win";


                }else if(_playerOneScore > _playerTwoScore)
                {
                    roundWinnerText.text = $"Player 1 is the Round {GameCount} Winner";
                    roundScoreText.text = $"Player 1 has {80}% and Player 2 has {50}% chance to win";

                }
                else
                {

                    roundWinnerText.text = $"Player 2 is the Round {GameCount} Winner";
                    roundScoreText.text = $"Player 2 has {80}% and Player 1 has {50}% chance to win";
                }
                break;
                case 2:
                if (_playerOneScore == _playerTwoScore)
                {
                    roundWinnerText.text = $"Both of you are {GameCount} Winners";
                    roundScoreText.text = $"Player 1 and 2 had {50}% chance to win";


                }
                else if (_playerOneScore > _playerTwoScore)
                {
                    roundWinnerText.text = $"Player 1 is the {GameCount} round Winner";
                    roundScoreText.text = $"Player 1 has {100}% and Player 2 has {20}% chance to win";

                }
                else
                {

                    roundWinnerText.text = $"Player 2 is the {GameCount} round Winner";
                    roundScoreText.text = $"Player 2 has {100}% and Player 1 has {20}% chance to win";
                }
                break;
            default:
                if (_playerOneScore == _playerTwoScore)
                {
                    roundWinnerText.text = $"Both of you are Winners";
                    roundScoreText.text = $"Player 1 and 2 had {50}% chance to win";


                }
                else if (_playerOneScore > _playerTwoScore)
                {
                    roundWinnerText.text = $"Player 1 is the Winner";
                    roundScoreText.text = $"Player 1 had {100}% and Player 2 had {20}% chance to win";

                }
                else
                {

                    roundWinnerText.text = $"Player 2 is the Winner";
                    roundScoreText.text = $"Player 2 had {100}% and Player 1 had {20}% chance to win";
                }
                break;

        }
    }
}

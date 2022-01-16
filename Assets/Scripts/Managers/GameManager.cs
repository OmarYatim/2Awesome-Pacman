using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Action = System.Action;


public enum GameState { StartGame = 0, GameOver = 1, Pause = 2 } //different in game states

/// <summary>Class responsible of logic for switching between game states</summary>
public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager Instance;
    [SerializeField] private Pacman pacman;
    [SerializeField] private Transform pellets;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text readyText;
    [SerializeField] private Text highScoreText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;
    [SerializeField] private int ghostMultiplier = 1;
    [SerializeField] private int score = 0;
    [SerializeField] private int lives = 3;
    private int oldHighScore;

    public static Action PacmanEatenEvent;
    public static Action PowerPelletEatenEvent;

    public static Action PelletResetEvent;
    public static Action ShowGhostsEvent;
    public static Action ObjectsEnabledEvent;

    [SerializeField] private AudioClip pacmanSoundtrack;
    [SerializeField] private GameObject pausePanel;
    private float prevTimeScale;
    private GameState state;

    private void Awake() => Instance = this;


    private void Start()
    {
        this.oldHighScore = PlayerPrefs.GetInt("HighScore");
        this.SetGameState(GameState.StartGame);
        SoundManager.Instance.PlayClip(this.pacmanSoundtrack);
    }

    private void Update()
    {
        if (InputManager.Instance.GetPauseButton)
            this.SetGameState(GameState.Pause);
    }

    public void SetGameState(GameState newState)
    {
        this.state = newState;
        switch(this.state)
        {
            case GameState.StartGame:
                this.NewGame();
                break;
            case GameState.GameOver:
                this.GameOver();
                break;
            case GameState.Pause:
                this.PauseGame();
                break;
        }
    }

    private void PauseGame() 
    {
        this.prevTimeScale = Time.timeScale;
        Time.timeScale = 0;
        this.pausePanel.SetActive(true);
        SoundManager.Instance.PauseSound();
    }

    public void Resume()
    {
        Time.timeScale = this.prevTimeScale;
        this.pausePanel.SetActive(false);
        SoundManager.Instance.UnPauseSound();
    }

    private void NewGame()
    {
        this.SetScore(this.score);
        this.SetLives(this.lives);
        if(this.lives >= 3) PelletResetEvent?.Invoke();
        this.ResetState();
    }

    private void ResetState()
    {
        Invoke(nameof(this.EnableGhosts), 3);
        Invoke(nameof(this.ResetObjects), 5);
    }
    private void EnableGhosts() => ShowGhostsEvent?.Invoke();

    private void ResetObjects() 
    {
        ObjectsEnabledEvent?.Invoke();
        this.readyText.enabled = false;
        Time.timeScale = 1;
    }

    private void GameOver()
    {
        this.gameOverPanel.SetActive(true);
        this.gameOverText.text = this.lives > 0 ? "YOU WON" : "GAME OVER";
        this.gameOverText.color = this.lives > 0 ? Color.yellow : Color.red;
        if(this.score > this.oldHighScore)
        {
            PlayerPrefs.SetInt("HighScore", this.score);
            this.highScoreText.text = "You have a new High score " + this.score;
        }
        else
            this.highScoreText.text = "Your score is " + this.score;
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        this.livesText.text = "x" + lives.ToString();
    }

    private void SetScore(int score)
    {
        this.score = score;
        this.scoreText.text = score.ToString().PadLeft(2, '0');
    }

    public void PacmanEaten()
    {
        PacmanEatenEvent?.Invoke();

        SetLives(this.lives - 1);

        if (this.lives > 0) {
            this.SetGameState(GameState.StartGame);
        } else {
            this.SetGameState(GameState.GameOver);
        }
    }

    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * this.ghostMultiplier;
        ghost.Particels.Play();
        SetScore(this.score + points);

        this.ghostMultiplier++;
    }

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);

        SetScore(this.score + pellet.points);

        if (!HasRemainingPellets())
        {
            this.pacman.gameObject.SetActive(false);
            this.SetGameState(GameState.GameOver);
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        PowerPelletEatenEvent?.Invoke();

        PelletEaten(pellet);
        CancelInvoke(nameof(ResetGhostMultiplier));
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in this.pellets)
        {
            if (pellet.gameObject.activeSelf) {
                return true;
            }
        }

        return false;
    }

    private void ResetGhostMultiplier() => this.ghostMultiplier = 1;
    public void Replay() => SceneManager.LoadScene(1);
    public void Menu() => SceneManager.LoadScene(0);
}

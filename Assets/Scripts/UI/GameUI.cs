using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [Header("Gameplay Main")]
    [SerializeField]
    private GameObject gameMain;
    [SerializeField]
    private TextMeshProUGUI livesText;
    [SerializeField]
    private TextMeshProUGUI remainingShotText;

    [Header("Free Mode")]
    [SerializeField]
    private GameObject freeModeGroup;

    [Header("Shooting Mode")]
    [SerializeField]
    private GameObject shootingModeGroup;
    [SerializeField]
    private Slider powerSlider;

    [Header("Pause Prompt")]
    [SerializeField]
    private GameObject pausePrompt;

    [Header("Win Prompt")]
    [SerializeField]
    private GameObject winPrompt;
    [SerializeField]
    private TextMeshProUGUI shotLeftText;
    [SerializeField]
    private TextMeshProUGUI timeElapsedText;
    [SerializeField]
    private GameObject starGroup;
    [SerializeField]
    private Color defaultStarColor;
    [SerializeField]
    private Color winStarColor;

    [Header("Lose Prompt")]
    [SerializeField]
    private GameObject losePrompt;

    private GameManager gameManager;
    private LevelManager levelManager;
    private AudioManager audioManager;
    private JsonSaving jsonSave;
    private Ball ball;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        levelManager = gameManager.transform.GetComponent<LevelManager>();
        ball = FindObjectOfType<Ball>();
        audioManager = FindObjectOfType<AudioManager>();
        jsonSave = FindObjectOfType<JsonSaving>();
    }

    private void Update() {
        if (ball.mode == (int)Ball.BallMode.Netral)
        {
            shootingModeGroup.SetActive(false);
            freeModeGroup.SetActive(true);
        }
        else if (ball.mode == (int)Ball.BallMode.Moving)
        {
            shootingModeGroup.SetActive(false);
            freeModeGroup.SetActive(false);
        }
        else if (ball.mode == (int)Ball.BallMode.Shoot)
        {
            UpdatePowerSlider();
            shootingModeGroup.SetActive(true);
            freeModeGroup.SetActive(false);
        }
    }

    private void OnEnable() {
        GameManager.isLiveDecrease += UpdateLivesText;
        Ball.isBallShot += UpdateShotRemaining;
        Goal.isGoal += ShowWinPrompt;
        GameManager.isGameOver += ShowLosePrompt;
        GameManager.isGamePrepared += InitialUI;
    }

    private void OnDisable() {
        GameManager.isLiveDecrease -= UpdateLivesText;
        Ball.isBallShot -= UpdateShotRemaining;
        Goal.isGoal -= ShowWinPrompt;
        GameManager.isGameOver -= ShowLosePrompt;
        GameManager.isGamePrepared -= InitialUI;
    }

    private void InitialUI() {
        UpdateLivesText();
        UpdateShotRemaining();
    }

    private void ShowWinPrompt() {
        // Check if level assets is not created yet
        if (levelManager.currentLevel == null)
        {
            Debug.LogError("Please create the level asset first");
            return;
        }

        // Open win prompt
        winPrompt.SetActive(true);
        gameMain.SetActive(false);

        // Update star requirement text
        int remainingShot = gameManager.GetRemainingShot();
        float timeElapsed = gameManager.GetTimeElapsed();
        shotLeftText.text = $"Shot Left   {remainingShot}";
        timeElapsedText.text = $"Time Elapsed   {FormatTime(timeElapsed)}";

        // Unlock next level
        if (levelManager.nextLevel != null)
            levelManager.nextLevel.UnlockLevel();

        // Update level total stars
        int finalScore = 1;
        if (remainingShot >= 0)
        {
            Image starImage = starGroup.transform.GetChild(1).GetComponent<Image>();
            starImage.color = winStarColor;
            finalScore++;
        }

        if (timeElapsed < levelManager.currentLevel.maxTime)
        {
            Image starImage = starGroup.transform.GetChild(2).GetComponent<Image>();
            starImage.color = winStarColor;
            finalScore++;
        }

        levelManager.currentLevel.UpdateTotalStar(finalScore);
        jsonSave.Save();
    }

    private void ShowLosePrompt() {
        losePrompt.SetActive(true);
        gameMain.SetActive(false);
    }

    private void UpdateLivesText() => livesText.text = $"Lives: {gameManager.GetLives()}";

    private void UpdateShotRemaining() => remainingShotText.text = $"Shots: {gameManager.GetRemainingShot()}";

    private void UpdatePowerSlider() => powerSlider.value = ball.GetShootForce() / ball.GetMaxShootForce();

    private string FormatTime(float time) {
        int seconds = (int) time % 60;
        int minutes = (int) time / 60;
        return string.Format("{0:00}:{1:00}", minutes, seconds);

    }

    public void ContinueButton() {
        if (levelManager.nextLevel == null)
        {
            MenuButton();
            return;
        }

        audioManager.PlaySFX(audioManager.UIClick);
        SceneManager.LoadScene(levelManager.nextLevel.scenePath);

    }

    public void RestartButton() {
        audioManager.PlaySFX(audioManager.UIClick);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    } 

    public void MenuButton() {
        audioManager.PlaySFX(audioManager.UIClick);
        SceneManager.LoadScene(0);
    }

    public void PauseButton() {
        audioManager.PlaySFX(audioManager.UIClick);
        gameManager.SetPause(true);
        pausePrompt.SetActive(true);
    }

    public void ResumeButton() {
        audioManager.PlaySFX(audioManager.UIClick);
        gameManager.SetPause(false);
        pausePrompt.SetActive(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class GameManager : MonoBehaviour
{
    public static Action isGameOver;
    public static Action isLiveDecrease;
    public static Action isGamePrepared;

    [SerializeField]
    private int lives = 3;
    [SerializeField]
    private float timeElapsed = 0f;

    public bool gameOver = false;
    public bool isPaused = false;

    private int remainingShot;
    private LevelManager levelManager;

    private void Awake() {
        Time.timeScale = 1;
        levelManager = GetComponent<LevelManager>();
    }

    private void Start() {
        remainingShot = levelManager.currentLevel.remainingShot;
        isGamePrepared?.Invoke();
    }

    private void OnEnable() {
        Ball.isBallShot += ReduceRemainingShot;
    }

    private void OnDisable() {
        Ball.isBallShot -= ReduceRemainingShot;
    }

    private void Update() {
        timeElapsed += Time.deltaTime;
    }

    public void DecreaseLive() {
        lives--;
        isLiveDecrease?.Invoke();
        if (lives == 0)
        {
            gameOver = true;
            isGameOver?.Invoke();
        }
    }

    private void ReduceRemainingShot() => remainingShot--;

    public int GetLives() => lives;

    public int GetRemainingShot() => remainingShot;

    public float GetTimeElapsed() => timeElapsed;

    public void SetPause(bool pause) {
        isPaused = pause;

        if (pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}

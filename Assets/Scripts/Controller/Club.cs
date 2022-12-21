using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Club : MonoBehaviour
{
    [SerializeField]
    private Transform club;
    [SerializeField]
    private Ball ball;

    private Transform clubParent;
    private Quaternion minRotation;
    private Quaternion maxRotation;
    private Quaternion defaultClubRotation = Quaternion.Euler(12, -90, 0);

    private AudioManager audioManager;

    private void Awake() {
        audioManager = FindObjectOfType<AudioManager>();
        clubParent = club.parent.transform;
    }

    private void Update() {
        DisplayClub();
    }

    private void DisplayClub()
    {
        if (ball == null) return;
        
        clubParent.gameObject.SetActive(ball.displayClub);
        
        if (ball.mode == (int)Ball.BallMode.Netral)
        {
            club.rotation = defaultClubRotation;
            
        clubParent.position = ball.transform.position;
        }
        if (!ball.displayClub)
        {
            clubParent.eulerAngles = Vector3.zero;
            return;
        }

        clubParent.LookAt(ball.mousePosition);
        clubParent.eulerAngles = new Vector3(0f, clubParent.eulerAngles.y, 0f);
    }

    public IEnumerator HitBall(Vector3 target) {
        minRotation = Quaternion.Euler(club.eulerAngles.x, club.eulerAngles.y, -60);
        maxRotation = Quaternion.Euler(club.eulerAngles.x, club.eulerAngles.y, 10);
        
        // Initial pull animation
        float timeElapsed = 0f;
        Quaternion currentRotation = Quaternion.Euler(club.eulerAngles.x, club.eulerAngles.y, 0);
        club.rotation = currentRotation;
        audioManager.PlaySFX(audioManager.pickUpSwoosh);
        while (timeElapsed < 1f)
        {  
            club.rotation = Quaternion.Lerp(currentRotation, minRotation, timeElapsed);
            timeElapsed += Time.deltaTime * 2;
            yield return null;
        }

        // Hit animation
        currentRotation = club.rotation;
        timeElapsed = 0;
        audioManager.PlaySFX(audioManager.hitSwoosh);
        while (timeElapsed < 1)
        {
            club.rotation = Quaternion.Lerp(currentRotation, maxRotation, timeElapsed);
            timeElapsed += Time.deltaTime * 3;
            yield return null;
        }
    }
}

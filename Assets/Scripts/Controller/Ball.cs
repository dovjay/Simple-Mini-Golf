using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static Action isBallShot;

    public enum BallMode {
        Netral,
        Shoot,
        Moving
    }
    public int mode = (int)BallMode.Netral;
    public Vector3 mousePosition;
    public bool displayClub = false;
    public bool playingShotAnimation = false;

    [SerializeField]
    private float maxForce = 2000f;
    [SerializeField]
    private float minForce = 200f;
    [SerializeField]
    private float moveModeDelay = 3f;
    [SerializeField]
    private Transform arrow;
    [SerializeField]
    private Club club;

    private Rigidbody rb;
    private GameManager manager;
    private bool isShootPressed = false;
    private Plane plane = new Plane(Vector3.up, 0);
    private float shootModeTime = 0f;
    private float moveModeRemaining = 0f;
    private Vector3 respawnPosition;
    private float shootForce = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        manager = FindObjectOfType<GameManager>();
    }


    private void Start() {
        respawnPosition = transform.position;
    }

    private void OnEnable() {
        GameManager.isGameOver += DestroyBall;
    }

    private void OnDisable() {
        GameManager.isGameOver -= DestroyBall;
    }

    void Update()
    {
        if (manager.isPaused) return;
        
        if (transform.position.y < -5)
        {
            rb.useGravity = false;
            transform.position = respawnPosition;
            manager.DecreaseLive();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = true;
            return;
        }

        // Check move remaining time and ball velocity
        float ballVelocity = Vector3.Distance(rb.velocity, Vector3.zero);
        if (moveModeRemaining > 0f)
        {
            moveModeRemaining -= Time.deltaTime;
            return;
        }
        else if (ballVelocity > .01f)
        {
            moveModeRemaining += Time.deltaTime;
            return;
        }
        else if (ballVelocity < .01f && moveModeRemaining <= 0f && mode == (int)BallMode.Moving)
        {
            respawnPosition = transform.position;
            mode = (int)BallMode.Netral;
        }

        // Check if Shoot key is pressed
        isShootPressed = Input.GetKeyDown(KeyCode.Space);
        if (isShootPressed && mode == (int)BallMode.Netral)
        {
            rb.useGravity = false;
            plane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));
            mode = (int)BallMode.Shoot;
        }
        else if (isShootPressed && mode == (int)BallMode.Shoot && !playingShotAnimation)
        {
            StartCoroutine(BallShot());
        }

        if (!playingShotAnimation && mode == (int)BallMode.Shoot)
        {
            rb.velocity = Vector3.zero;
            displayClub = true;

            // Calculate mouse position
            float distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out distance))
            {
                mousePosition = ray.GetPoint(distance);
            }
            shootModeTime += Time.deltaTime;
            if (shootModeTime > 1f)
            {
                float temp = maxForce;
                maxForce = minForce;
                minForce = temp;
                shootModeTime = 0;
            }
            shootForce = Mathf.Lerp(minForce, maxForce, shootModeTime);
        }
    }

    private IEnumerator BallShot()
    {
        playingShotAnimation = true;
        rb.useGravity = true;
        Vector3 target = (mousePosition - transform.position).normalized;
        target = new Vector3(target.x, 0.15f, target.z); // change shot elevation
        yield return club.HitBall(target);
        rb.AddForce(target * shootForce, ForceMode.Impulse);
        shootForce = Mathf.Min(minForce, maxForce);
        moveModeRemaining = moveModeDelay;
        mode = (int)BallMode.Moving;
        playingShotAnimation = false;
        isBallShot?.Invoke();
        yield return new WaitForSeconds(1f);
        displayClub = false;
    }

    private void DestroyBall() => Destroy(gameObject);

    public float GetShootForce() => shootForce;

    public float GetMaxShootForce() => Mathf.Max(minForce, maxForce);
}

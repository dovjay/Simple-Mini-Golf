using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    [SerializeField]
    private Vector3 cameraOffset = new Vector3(1, 1, 0);
    [SerializeField]
    private Quaternion cameraRotation = Quaternion.Euler(45, -75, 0);
    [SerializeField]
    private float dragSpeed = 2;
    [SerializeField]
    private float focusTime = 2;
    
    private float focusElapsed = 0;
    private bool alreadyFocused = false;
    private Vector3 dragOrigin;
    private Ball ball;
    private Plane plane = new Plane(Vector3.up, 0);

    private void Awake() {
        ball = FindObjectOfType<Ball>();
    }

    private void Start() {
        StartCoroutine(FocusBallMode());
    }

    private void OnEnable() {
        GameManager.isLiveDecrease += StartFocusBallMode;
    }

    private void OnDisable() {
        GameManager.isLiveDecrease -= StartFocusBallMode;
    }
    
    private void LateUpdate() {
        if (ball == null) 
            return;

        switch (ball.mode)
        {
            case (int)Ball.BallMode.Shoot:
                StartFocusBallMode();
                break;
            case (int)Ball.BallMode.Moving:
                FollowMode();
                break;
            case (int)Ball.BallMode.Netral:
                FreeMode();
                break;
            default:
                Debug.LogError("There's no such camera mode!");
                break;
        }
    }

    public IEnumerator FocusBallMode() {
        if (!alreadyFocused)
        {
            focusElapsed = 0;
            transform.rotation = cameraRotation;
            Vector3 currentPos = transform.position;
            while (focusElapsed < focusTime)
            {
                transform.position = Vector3.Lerp(currentPos, 
                                    ball.transform.position + cameraOffset, 
                                    focusElapsed / focusTime);
                focusElapsed += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            ZoomInOut();
        }

        alreadyFocused = true;
    }

    private void StartFocusBallMode() {
        StartCoroutine(FocusBallMode());
    }

    private void FollowMode() {
        transform.position = ball.transform.position + cameraOffset;
    }

    private void FreeMode() {
        alreadyFocused = false;
        ZoomInOut();

        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = GetMousePosition();
            return;
        }

         if (!Input.GetMouseButton(0)) return;

         Vector3 pos = dragOrigin - GetMousePosition();
         Vector3 move = new Vector3(pos.x, 0, pos.z);

        transform.Translate(move * Time.deltaTime * dragSpeed, Space.World);
    }

    private Vector3 GetMousePosition() {
        Vector3 result = Vector3.zero;
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            result = ray.GetPoint(distance);
        }
        return result;
    }

    private void ZoomInOut() {
        if (ball.playingShotAnimation) return;

        if (Input.mouseScrollDelta.y != 0)
        {
            transform.position -= new Vector3(1f, 1f, 0) * .1f * Input.mouseScrollDelta.y;
        }
    }
}

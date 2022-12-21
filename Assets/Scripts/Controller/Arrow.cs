using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    private Transform arrow;
    [SerializeField]
    private Ball ball;

    void Update()
    {
        DisplayArrow();
    }

    private void DisplayArrow() {
        if (ball.mode != (int)Ball.BallMode.Shoot)
        {
            arrow.gameObject.SetActive(false);
            return;
        }

        arrow.gameObject.SetActive(true);
        arrow.LookAt(ball.mousePosition);
        arrow.eulerAngles = new Vector3(0f, arrow.eulerAngles.y, 0f);
        arrow.position = ball.transform.position;
    }
}

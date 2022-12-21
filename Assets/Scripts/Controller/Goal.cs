using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public static Action isGoal;

    private BoxCollider goalCol;

    private void Awake() {
        goalCol = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Ball"))
        {
            isGoal?.Invoke();
        }
    }
}

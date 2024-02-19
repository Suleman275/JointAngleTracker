using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyTrackingStarter : MonoBehaviour {
    public static BodyTrackingStarter Instance;

    public event Action OnBodyStreamStarted;

    private void Awake() {
        Instance = this;
    }

    void Start() {
        // Ensure that the AstraSDKManager exists
        if (AstraSDKManager.Instance == null) {
            Debug.LogError("AstraSDKManager is not assigned!");
            return;
        }

        // Subscribe to the OnInitializeSuccess event
        AstraSDKManager.Instance.OnInitializeSuccess.AddListener(OnAstraSDKInitializeSuccess);
    }

    private void OnAstraSDKInitializeSuccess() {
        Debug.Log("Astra SDK initialized successfully!");

        // Activate body stream
        AstraSDKManager.Instance.IsBodyOn = true;

        OnBodyStreamStarted?.Invoke();
    }
}

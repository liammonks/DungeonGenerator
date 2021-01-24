using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private LineRenderer movementLR = null;

    private Camera mainCamera;

    private void Awake() {
        mainCamera = Camera.main;
    }

    private void OnFirstTouch(Touch touch)
    {
        movementLR.SetPosition(0, mainCamera.ScreenToWorldPoint(touch.position));
    }
}

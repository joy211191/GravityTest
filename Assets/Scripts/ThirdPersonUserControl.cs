using System;
using UnityEngine;

public class ThirdPersonUserControl : MonoBehaviour
{
    private CharacterAnimator character;
    private Transform cameraTransform;
    private Transform cameraTarget;
    private Vector3 cameraForward;
    private Vector3 moveVector;


    void Awake()
    {
        character = GetComponent<CharacterAnimator>();
        cameraTransform = Camera.main.transform;
        cameraTarget = transform.Find("CameraTarget");
    }

    private void Update() {
        if (InputManager.Instance.GravitySelection)
            return;
        cameraForward = transform.forward;
        Vector3 playerVector = transform.forward + transform.right;
        cameraForward = Vector3.Scale(cameraTransform.forward, playerVector).normalized;
        moveVector = InputManager.Instance.MovementVector.y * cameraForward + InputManager.Instance.MovementVector.x * cameraTransform.right;
        character.Move(moveVector);
    }
}

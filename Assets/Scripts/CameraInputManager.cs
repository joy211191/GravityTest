using Cinemachine;
using UnityEngine;

public class CameraInputManager : MonoBehaviour
{
    CinemachineInputProvider cinemachineInputProvider;

    private void Awake() {
        cinemachineInputProvider= GetComponent<CinemachineInputProvider>();
    }
    private void Update() {
        cinemachineInputProvider.enabled = !InputManager.Instance.GravitySelection;
    }
}

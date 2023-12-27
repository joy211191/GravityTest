using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem.Interactions;

public class InputManager : Singleton<InputManager> {
    Player player;

    private Vector2 movementVector;
    private Vector2 lookVector;
    private Vector2 gravityDirection;
    private bool gravitySelection;

    public Vector2 MovementVector => movementVector;
    public Vector2 LookVector => lookVector;
    public Vector2 GravityDirection => gravityDirection;
    public bool GravitySelection => gravitySelection;
    public bool gravityButtonCancelled;

    void Awake() {
        player = new Player();
    }

    private void OnEnable() {
        player.Default.Movement.Enable();
        player.Default.Camera.Enable();
        player.Default.GravitySelection.Enable();
        player.Default.GravityMenu.performed += GravityInput;
        player.Default.GravityMenu.canceled += GravityInput;
        player.Default.GravityMenu.Enable();
    }

    private void OnDisable() {
        player.Default.Movement.Disable();
        player.Default.Camera.Disable();
        player.Default.GravitySelection.Disable();
        player.Default.GravityMenu.Disable();
    }

    private void GravityInput(CallbackContext callbackContext) {
        gravitySelection = callbackContext.interaction is HoldInteraction;
        gravityButtonCancelled = false;
    }

    void Update() {
        movementVector = player.Default.Movement.ReadValue<Vector2>();
        lookVector = player.Default.Camera.ReadValue<Vector2>();
        gravityDirection = player.Default.GravitySelection.ReadValue<Vector2>();
    }

    public void LateUpdate() {
        if (player.Default.GravityMenu.WasReleasedThisFrame()) {
            gravitySelection = false;
            gravityButtonCancelled = true;
        }
    }
}
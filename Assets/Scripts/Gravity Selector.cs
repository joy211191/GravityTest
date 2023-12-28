using UnityEngine;

public enum GravityDirection {
    X_AXIS,
    N_X_AXIS,
    Y_AXIS, 
    N_Y_AXIS, 
    Z_AXIS,
    N_Z_AXIS
}


public class GravitySelector : Singleton<GravitySelector> {
    public GravityDirection direction;
    public Vector3 gravity;
    public const float GRAVITYVALUE = 9.81f;
    public GameObject holoExo;
    Vector3 rotationVector = Vector3.zero;
    [SerializeField]
    float lerpFactor = 1;

    private void Awake() {
        gravity = -transform.up * GRAVITYVALUE;
    }

    void Update() {
        transform.up = Vector3.up;
        holoExo.SetActive(InputManager.Instance.GravitySelection);
        if (InputManager.Instance.GravitySelection) {
            if (Mathf.Abs(InputManager.Instance.LookVector.y) > 1) {
                rotationVector.z = 0;
                rotationVector = holoExo.transform.rotation.eulerAngles + new Vector3(90 * InputManager.Instance.LookVector.normalized.y, 0f, 0);
                rotationVector.x -= rotationVector.x % 90;
            }
            else if (Mathf.Abs(InputManager.Instance.LookVector.x) > 1) {
                rotationVector.x = 0;
                rotationVector = holoExo.transform.rotation.eulerAngles - new Vector3(0f, 0f, 90 * InputManager.Instance.LookVector.normalized.x);
                rotationVector.y -= rotationVector.y % 90;
            }
            holoExo.transform.localRotation = Quaternion.Slerp(holoExo.transform.localRotation, Quaternion.Euler(rotationVector), Time.deltaTime * lerpFactor);
            transform.localRotation = Quaternion.Euler(rotationVector);
        }
        else {
            rotationVector = Vector3.zero;
            holoExo.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
        if (InputManager.Instance.gravityButtonCancelled) {
            Physics.gravity = -transform.up * GRAVITYVALUE;
            rotationVector.y = holoExo.transform.parent.rotation.eulerAngles.y;
            holoExo.transform.parent.rotation =  Quaternion.Euler(-rotationVector);
            InputManager.Instance.gravityButtonCancelled = false;
        }
    }
}
using UnityEngine;

public class GravitySelector : Singleton<GravitySelector> {
    public Vector3 gravity;
    public const float GRAVITYVALUE = -9.81f;
    public GameObject holoExo;
    Vector3 rotationVector = Vector3.zero;
    [SerializeField]
    float lerpFactor = 1;
    Transform playerTransform;

    private void Awake() {
        gravity = transform.up * GRAVITYVALUE;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() {
        transform.up = Vector3.up;
        holoExo.SetActive(InputManager.Instance.GravitySelection);
        if (InputManager.Instance.GravitySelection) {
            if (Mathf.Abs(InputManager.Instance.LookVector.y) > Mathf.Abs(InputManager.Instance.LookVector.x)) {
                rotationVector.z = 0;
                rotationVector += new Vector3(90 * InputManager.Instance.LookVector.normalized.y, 0, 0);
                rotationVector.x -= rotationVector.x % 90;
            }
            else if (Mathf.Abs(InputManager.Instance.LookVector.x) > Mathf.Abs(InputManager.Instance.LookVector.y)) {
                rotationVector.x = 0;
                rotationVector = -new Vector3(0f, 0f, 90 * InputManager.Instance.LookVector.normalized.x);
                rotationVector.z -= rotationVector.z % 90;
            }
            holoExo.transform.localRotation = Quaternion.Slerp(holoExo.transform.localRotation, Quaternion.Euler(rotationVector), Time.deltaTime * lerpFactor);
            transform.localRotation = Quaternion.Euler(rotationVector);
        }
        if (InputManager.Instance.gravityButtonCancelled) {
            transform.localRotation = Quaternion.Euler(rotationVector);
            Physics.gravity = transform.up * GRAVITYVALUE;
            rotationVector.y = -holoExo.transform.parent.rotation.eulerAngles.y;
            playerTransform.rotation = Quaternion.Euler(rotationVector);
            playerTransform.position= holoExo.transform.position;
            InputManager.Instance.gravityButtonCancelled = false;
            rotationVector = Vector3.zero;
        }
    }
}
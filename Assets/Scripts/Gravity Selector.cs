using UnityEngine;

public class GravitySelector : Singleton<GravitySelector> {
    public Vector3 gravity;
    public const float GRAVITYVALUE = -9.81f;
    public GameObject holoExo;
    Vector3 rotationVector = Vector3.zero;
    [SerializeField]
    float lerpFactor = 1;
    Transform playerTransform;
    Vector3 tempPosition;

    private void Awake() {
        gravity = transform.up * GRAVITYVALUE;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() {
        holoExo.SetActive(InputManager.Instance.GravitySelection);
        if (InputManager.Instance.GravitySelection) {
            if (Mathf.Abs(InputManager.Instance.LookVector.y) > Mathf.Abs(InputManager.Instance.LookVector.x)) {
                rotationVector.z = 0;
                rotationVector += new Vector3(90 * InputManager.Instance.LookVector.normalized.y, 0, 0);
                rotationVector.x = Mathf.Clamp(rotationVector.x, -360f, 360f);
                rotationVector.x -= rotationVector.x % 90;
            }
            else if (Mathf.Abs(InputManager.Instance.LookVector.x) > Mathf.Abs(InputManager.Instance.LookVector.y)) {
                rotationVector.x = 0;
                rotationVector = -new Vector3(0f, playerTransform.transform.rotation.eulerAngles.y, 90 * InputManager.Instance.LookVector.normalized.x);
                rotationVector.z -= rotationVector.z % 90;
            }
            holoExo.transform.rotation = Quaternion.Slerp(holoExo.transform.rotation, Quaternion.Euler(rotationVector), Time.deltaTime * lerpFactor);
            transform.up = playerTransform.up;
            transform.rotation = Quaternion.Euler(rotationVector);
            tempPosition = holoExo.transform.position;
        }
        if (InputManager.Instance.gravityButtonCancelled) {
            Physics.gravity = transform.up * GRAVITYVALUE;
            rotationVector.y = holoExo.transform.parent.rotation.eulerAngles.y;
            holoExo.transform.localRotation = Quaternion.Euler(Vector3.zero);
            playerTransform.rotation = transform.localRotation;
            playerTransform.position = tempPosition;
            InputManager.Instance.gravityButtonCancelled = false;
            rotationVector = transform.rotation.eulerAngles;
        }
        Debug.DrawRay(playerTransform.transform.position, rotationVector);
    }
}
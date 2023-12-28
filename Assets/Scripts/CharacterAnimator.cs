using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : ThirdPersonCharacter {
    [SerializeField]
    float defaultFixedDeltaTime;
    public Vector3 sensorOffset;
    public Vector3 forwardSensorOffset;
    public float forwardSensorLength;
    public float sensorLength;
    public Animator animator;
    [SerializeField]
    float animSpeedMultiplier = 1f;

    protected override void Awake() {
        base.Awake();
        animator = GetComponent<Animator>();

    }

    void Start() {

    }

    public bool GroundCheckSensor() {
        RaycastHit hit;
        Vector3 sensorPosition = transform.position;
        sensorPosition += transform.forward * sensorOffset.z;
        sensorPosition += transform.up * sensorOffset.y;
        Physics.Raycast(sensorPosition + sensorOffset, -transform.up, out hit, sensorLength);
        Debug.DrawRay(sensorPosition + sensorOffset, -transform.up * sensorLength, Color.red);
        if (hit.transform == null || hit.transform.tag == "Foot" || hit.transform.tag == "Player") {
            return false;
        }
        else
            return true;
    }

    void UpdateAnimator(Vector3 move) {
        animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
    }

    public void HandleGroundedMovement() {
        gravityMultiplier = 5;
        Vector3 jumpForceVector = transform.up * jumpOwer + transform.forward *  forwardAmount;
        rb.velocity = jumpForceVector;
    }

    public void HandleAirBorneMovement() {
        Vector3 extraGravityForce = new Vector3(0, 0, 0);
        extraGravityForce += new Vector3(0, Physics.gravity.y * gravityMultiplier, transform.forward.z * forwardAmount);
        rb.AddForce(extraGravityForce);
    }

    void Update() {
        UpdateAnimator(moveVector);
        //transform.up = -Physics.gravity;
    }

    public void FixedUpdate() {
        grounded = GroundCheckSensor();
        if (!grounded) {
            HandleAirBorneMovement();
        }
        rb.AddForce(Physics.gravity*gravityMultiplier);
    }

    public void OnAnimatorMove() {
        if (grounded && Time.deltaTime > 0) {
            Vector3 v = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;
            v.y = rb.velocity.y;
            rb.velocity = v;
        }
    }
}
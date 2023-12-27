using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]

public class ThirdPersonCharacter : MonoBehaviour {

    [SerializeField]
    protected float turnSpeed = 360;
    [SerializeField]
    protected float jumpOwer = 12f;
    [SerializeField]
    protected float moveSpeedMultiplier = 1f;
    [SerializeField] protected float groundCheckDistance = 2.5f;
    public float gravityMultiplier = 5f;
    [SerializeField]
    protected float turnAmount;
    [SerializeField]
    protected float forwardAmount;
    protected bool grounded;
    protected Vector3 moveVector;
    protected CapsuleCollider capsuleCollider;

    protected Rigidbody rb;
    Vector3 groundNormal;

    protected virtual void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        SetRigidbodyConstrains(false);
    }

    public void SetRigidbodyConstrains(bool yPos) {
        if (yPos)
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
        else
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    public void Move(Vector3 move) {
        if (move.magnitude > 1f)
            move.Normalize();
        moveVector = move;
        move = transform.InverseTransformDirection(move);
        move = Vector3.ProjectOnPlane(move, groundNormal);
        Debug.DrawRay(transform.position, move * 5, Color.red);
        forwardAmount = move.z;
        if (move.magnitude > 0) {
            turnAmount = Mathf.Atan2(move.x, move.z);
        }
        else
            turnAmount = 0;
        ApplyTurnRotation();
    }

    public void ApplyTurnRotation() {
        transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
    }
}

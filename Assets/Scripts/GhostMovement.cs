using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour {

    Rigidbody rb;
    
    Vector3[] directions = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
    int dirIndex = 0;
    Vector3 currDir;
    public LayerMask rayLayer;

    float speed;
    float rayDistance;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        speed = 1.5f;
        rayDistance = 5f;
        currDir = directions[dirIndex];
    }

    private void Update() {
        SpeedControl();

        bool hit = Physics.Raycast(transform.position, currDir, rayDistance, rayLayer);
        Debug.DrawRay(transform.position, currDir * rayDistance, Color.red);
        if (hit) {
            ChangeDirection();
        }
    }

    private void FixedUpdate() {
        MoveGhost();
    }

    private void SpeedControl() {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude > speed)
        {
            Vector3 limitVel = flatVel.normalized * speed;
            rb.velocity = new Vector3(limitVel.x, rb.velocity.y, limitVel.z);
        }
    }

    private void ChangeDirection() {
        if (Random.value < 0.5f) {
            dirIndex = (dirIndex + 1) % 4;
        }else {
            dirIndex = (dirIndex + 3) % 4;
        }
        currDir = directions[dirIndex];
        transform.rotation = Quaternion.LookRotation(currDir, Vector3.up);
    }

    private void MoveGhost() {
        rb.AddForce(currDir * speed, ForceMode.Impulse);
    }
}

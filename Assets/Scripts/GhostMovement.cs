using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour {

    Rigidbody rb;
    
    Vector3[] directions = {Vector3.forward, Vector3.right, Vector3.back, Vector3.left};
    int dirIndex;
    Vector3 currDir;

    public float rayDistance;
    public LayerMask rayLayer;

    public float speed;

    bool checkingNode;
    public Vector3 destination;
    
    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        checkingNode = false;
        dirIndex = 0;
        currDir = directions[dirIndex];
    }

    private void Update() {
        SpeedControl();
        Debug.DrawLine(transform.position, destination);
    }

    private void FixedUpdate() {
        MoveGhost();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Node")) {
            if (!checkingNode) {
                checkingNode = true;
                ChangeDirection();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Node")) {
            checkingNode = false;
        }
    }

    private void SpeedControl() {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude > speed) {
            Vector3 limitVel = flatVel.normalized * speed;
            rb.velocity = new Vector3(limitVel.x, rb.velocity.y, limitVel.z);
        }
    }

    private void ChangeDirection() {
        rb.velocity = Vector3.zero;

        Vector3 tmp = transform.position;
        tmp.x = Mathf.Round(tmp.x);
        tmp.z = Mathf.Round(tmp.z);
        transform.position = tmp;

        int turnRight = (dirIndex + 1) % 4;
        int turnLeft = (dirIndex + 3) % 4;

        int minIndex = (dirIndex + 2) % 4;
        float minDist = float.MaxValue;
        if (!Physics.Raycast(transform.position, directions[dirIndex], rayDistance, rayLayer)) {
            float dist = Vector3.Distance(transform.position + currDir * rayDistance, destination);
            if (dist < minDist) {
                minDist = dist;
                minIndex = dirIndex;
            }
        }
        if (!Physics.Raycast(transform.position, directions[turnRight], rayDistance, rayLayer)) {
            float dist = Vector3.Distance(transform.position + directions[turnRight] * rayDistance, destination);
            if (dist < minDist) {
                minDist = dist;
                minIndex = turnRight;
            }
        }
        if (!Physics.Raycast(transform.position, directions[turnLeft], rayDistance, rayLayer)) {
            float dist = Vector3.Distance(transform.position + directions[turnLeft] * rayDistance, destination);
            if (dist < minDist) {
                minIndex = turnLeft;
            }
        }

        dirIndex = minIndex;
        currDir = directions[minIndex];
        transform.rotation = Quaternion.LookRotation(currDir, Vector3.up);
    }

    private void MoveGhost() {
        rb.AddForce(currDir * speed, ForceMode.Impulse);
    }
}

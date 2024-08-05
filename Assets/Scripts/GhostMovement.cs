using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour {

    Rigidbody rb;

    public Transform spawn;
    public Transform home;

    public float maxSpeed;
    float currSpeed;

    Vector3[] directions = {Vector3.forward, Vector3.right, Vector3.back, Vector3.left};
    int dirIndex;
    Vector3 currDir;

    public float rayDistance;
    public LayerMask rayLayer;

    bool checkingNode;
    public Vector3 destination;

    Renderer ren;
    [HideInInspector]
    public Material matNormal;
    [HideInInspector]
    public Material matFrighten;

    public float spawnTime;
    public float frightenTime;
    float timer;

    AudioManager sounds;
    [HideInInspector]
    public string spawnSound;

    public enum GhostStates {
        SPAWNING,
        ALIVE,
        FRIGHTENED,
        SLEEP
    }
    public GhostStates state;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        ren = transform.GetChild(0).gameObject.GetComponent<Renderer>();
    }

    private void Start() {
        currSpeed = maxSpeed;
        checkingNode = false;
        dirIndex = 0;
        currDir = directions[dirIndex];
        timer = 0f;
        ren.material = matNormal;
        sounds = FindObjectOfType<AudioManager>();
    }

    private void Update() {
        SpeedControl();

        Debug.DrawLine(transform.position, destination);

        bool hit = Physics.Raycast(transform.position, currDir, rayDistance, rayLayer);
        if (hit) {
            ChangeDirection();
        }

        if (state == GhostStates.FRIGHTENED) {
            timer += Time.deltaTime;
            if (timer > frightenTime) {
                timer -= frightenTime;
                FrightenModeExit();
            }
        }else if (state == GhostStates.SPAWNING) {
            timer += Time.deltaTime;
            if (timer > spawnTime) {
                timer -= spawnTime;
                Spawn();
            }
        }
    }

    private void FixedUpdate() {
        if (state != GhostStates.SPAWNING && state != GhostStates.SLEEP) { 
            MoveGhost();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Node")) {
            if (!checkingNode) {
                checkingNode = true;
                if (state == GhostStates.FRIGHTENED) {
                    RandomDirection();
                }else { 
                    ChangeDirection();
                }
            }
        }else if (other.gameObject.layer == LayerMask.NameToLayer("Player") && other.GetType() == typeof(CapsuleCollider)) {
            if (state == GhostStates.FRIGHTENED) {
                Die();
            }else {
                // Game Over
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
        if (flatVel.magnitude > currSpeed) {
            Vector3 limitVel = flatVel.normalized * currSpeed;
            rb.velocity = new Vector3(limitVel.x, rb.velocity.y, limitVel.z);
        }
    }

    public void FrightenModeEnter() {
        if (state != GhostStates.SPAWNING && state != GhostStates.SLEEP) {
            currSpeed = maxSpeed - 1;
            timer = 0;
            ren.material = matFrighten;
            state = GhostStates.FRIGHTENED;
        }
    }

    public void FrightenModeExit() {
        currSpeed = maxSpeed;
        ren.material = matNormal;
        state = GhostStates.ALIVE;
    }

    private void Die() {
        transform.position = home.position;
        rb.velocity = Vector3.zero;
        currSpeed = maxSpeed;
        ren.material = matNormal;
        timer = 0;
        state = GhostStates.SPAWNING;
    }

    public void Spawn() {
        sounds.Play(spawnSound);
        state = GhostStates.ALIVE;
    }

    private void SetDirection(int index) {
        if (state != GhostStates.SPAWNING) {
            rb.velocity = Vector3.zero;
            Vector3 tmp = transform.position;
            tmp.x = Mathf.Round(tmp.x);
            tmp.z = Mathf.Round(tmp.z);
            transform.position = tmp;

            dirIndex = index;
            currDir = directions[index];
            transform.rotation = Quaternion.LookRotation(currDir, Vector3.up);
        }
    }

    public void TurnAround() {
        SetDirection((dirIndex + 2) % 4);
    }

    private void ChangeDirection() {
        int turnRight = (dirIndex + 1) % 4;
        int turnLeft = (dirIndex + 3) % 4;

        int minIndex = (dirIndex + 2) % 4;
        float minDist = float.MaxValue;
        if (!Physics.Raycast(transform.position, currDir, rayDistance, rayLayer)) {
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
        SetDirection(minIndex);
    }

    private void RandomDirection() {
        List<int> dirIndexes = new List<int>();

        int turnRight = (dirIndex + 1) % 4;
        int turnLeft = (dirIndex + 3) % 4;

        if (!Physics.Raycast(transform.position, directions[dirIndex], rayDistance, rayLayer)) {
            dirIndexes.Add(dirIndex);
        }
        if (!Physics.Raycast(transform.position, directions[turnRight], rayDistance, rayLayer)) { 
            dirIndexes.Add(turnRight);
        }
        if (!Physics.Raycast(transform.position, directions[turnLeft], rayDistance, rayLayer)) {
            dirIndexes.Add(turnLeft);
        }
        SetDirection(dirIndexes[Random.Range(0, dirIndexes.Count)]);
    }

    private void MoveGhost() {
        rb.AddForce(currDir * currSpeed, ForceMode.Impulse);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour {

    public Transform player;
    public Transform spawn;
    public Transform bottomLeft;
    public Transform bottomRight;
    public Transform topLeft;
    public Transform topRight;

    public GhostMovement blinky;
    public GhostMovement pinky;
    public GhostMovement inky;
    public GhostMovement clyde;

    public enum GhostStates {
        CHASE,
        SCATTER,
        FRIGHTENED,
        DEAD
    }
    public GhostStates state;

    float waitTime;
    float timer;
    bool isChasing;

    private void Start() {
        state = GhostStates.SCATTER;
        waitTime = 20f;
        timer = 0f;
        isChasing = false;

        blinky.destination = spawn.position;
        pinky.destination = spawn.position;
        inky.destination = spawn.position;
        clyde.destination = spawn.position;

        InvokeRepeating("UpdatePaths", 20f, 0.5f);
    }

    private void Update() {
        timer += Time.deltaTime;
        if (timer > waitTime) {
            if (isChasing) {
                state = GhostStates.SCATTER;
            }else {
                state = GhostStates.CHASE;
            }
            isChasing = !isChasing;
            timer -= waitTime;
        }
    }

    private void UpdatePaths() {
        if (state == GhostStates.CHASE) {
            blinky.destination = player.position;
            Vector3 front = new Vector3(player.forward.x, 0f, player.forward.z).normalized * 20f;
            pinky.destination = player.position + front * 2f;
            inky.destination = front + (front - blinky.transform.position);
            if (Vector3.Distance(clyde.transform.position, player.position) < 80f) {
                clyde.destination = bottomLeft.position;
            }else {
                clyde.destination = player.transform.position;
            }
        }else if (state == GhostStates.SCATTER) {
            blinky.destination = topRight.position;
            pinky.destination = topLeft.position;
            inky.destination = bottomRight.position;
            clyde.destination = bottomLeft.position;
        }
    }
}
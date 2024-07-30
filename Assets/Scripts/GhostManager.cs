using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour {

    public Transform player;
    public Transform spawn;

    public GhostMovement blinky;
    public GhostMovement pinky;
    public GhostMovement inky;
    public GhostMovement clyde;

    private void Start() {
        blinky.destination = spawn.position;
        pinky.destination = spawn.position;
        inky.destination = spawn.position;
        clyde.destination = spawn.position;

        InvokeRepeating("UpdatePaths", 10f, 0.5f);
    }
    private void UpdatePaths() {
        blinky.destination = player.position;

        pinky.destination = player.position + new Vector3(player.forward.x, 0f, player.forward.z).normalized * 20f;

        inky.destination = player.position + (player.position - blinky.transform.position);

        if (Vector3.Distance(clyde.transform.position, player.position) < 40f) {
            clyde.destination = spawn.position;
        }else {
            clyde.destination = player.transform.position;
        }
    }
}

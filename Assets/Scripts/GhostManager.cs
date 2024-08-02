using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour {

    public static GhostManager instance;

    public Transform player;

    public Transform spawn;
    public Transform home;

    public Transform bottomLeft;
    public Transform bottomRight;
    public Transform topLeft;
    public Transform topRight;

    public Material matBlinky;
    public Material matPinky;
    public Material matInky;
    public Material matClyde;

    public GhostMovement blinky;
    public GhostMovement pinky;
    public GhostMovement inky;
    public GhostMovement clyde;

    public enum GameStates {
        CHASE,
        SCATTER,
    }
    public GameStates state;

    public float chaseTime;
    public float scatterTime;
    float waitTime;
    float timer;

    private void Awake() {
        instance = this;

        blinky.spawn = spawn;
        pinky.spawn = spawn;
        inky.spawn = spawn;
        clyde.spawn = spawn;

        blinky.home = home;
        pinky.home = home;
        inky.home = home;
        clyde.home = home;

        blinky.matNormal = matBlinky;
        pinky.matNormal = matPinky;
        inky.matNormal = matInky;
        clyde.matNormal = matClyde;

        blinky.state = GhostMovement.GhostStates.ALIVE;
        pinky.state = GhostMovement.GhostStates.ALIVE;
        inky.state = GhostMovement.GhostStates.ALIVE;
        clyde.state = GhostMovement.GhostStates.ALIVE;
    }
    private void Start() {
        state = GameStates.SCATTER;
        waitTime = scatterTime;
        timer = 0f;

        InvokeRepeating("UpdatePaths", 0, 1f);
    }

    private void Update() {
        timer += Time.deltaTime;
        if (timer > waitTime) {
            timer -= waitTime;
            if (state == GameStates.CHASE) {
                ScatterMode();
            }else {
                ChaseMode();
            }
        }
    }

    private void UpdatePaths() {
        if (state == GameStates.CHASE) {
            blinky.destination = player.position;
            Vector3 front = new Vector3(player.forward.x, 0f, player.forward.z).normalized;
            pinky.destination = player.position + front * 40f;
            inky.destination = player.position + (player.position + front * 2f - blinky.transform.position);
            if (Vector3.Distance(clyde.transform.position, player.position) < 80f) {
                clyde.destination = bottomLeft.position;
            }else {
                clyde.destination = player.transform.position;
            }
        }else if (state == GameStates.SCATTER) {
            blinky.destination = topRight.position;
            pinky.destination = topLeft.position;
            inky.destination = bottomRight.position;
            clyde.destination = bottomLeft.position;
        }
    }

    private void ScatterMode() {
        state = GameStates.SCATTER;
        waitTime = scatterTime;
        blinky.TurnAround();
        pinky.TurnAround();
        inky.TurnAround();
        clyde.TurnAround();
    }

    private void ChaseMode() {
        state = GameStates.CHASE;
        waitTime = chaseTime;
    }

    public void FrightenMode() {
        blinky.FrightenModeEnter();
        pinky.FrightenModeEnter();
        inky.FrightenModeEnter();
        clyde.FrightenModeEnter();
    }
}

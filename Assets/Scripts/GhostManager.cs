using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour {

    public static GhostManager instance;

    public Transform player;

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

        blinky.home = home;
        pinky.home = home;
        inky.home = home;
        clyde.home = home;

        blinky.matNormal = matBlinky;
        pinky.matNormal = matPinky;
        inky.matNormal = matInky;
        clyde.matNormal = matClyde;

        blinky.spawnSound = "BlinkySpawn";
        pinky.spawnSound = "PinkySpawn";
        inky.spawnSound = "InkySpawn";
        clyde.spawnSound = "ClydeSpawn";

        blinky.state = GhostMovement.GhostStates.SLEEP;
        pinky.state = GhostMovement.GhostStates.SLEEP;
        inky.state = GhostMovement.GhostStates.SLEEP;
        clyde.state = GhostMovement.GhostStates.SLEEP;
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
                state = GameStates.SCATTER;
                waitTime = scatterTime;
                blinky.TurnAround();
                pinky.TurnAround();
                inky.TurnAround();
                clyde.TurnAround();
            }
            else {
                state = GameStates.CHASE;
                waitTime = chaseTime;
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

    public void FrightenMode() {
        blinky.FrightenModeEnter();
        pinky.FrightenModeEnter();
        inky.FrightenModeEnter();
        clyde.FrightenModeEnter();
    }

    public void WakeBlinky() {
        blinky.Spawn();
    }

    public void WakePinky() {
        pinky.Spawn();
    }

    public void WakeInky() {
        inky.Spawn();
    }

    public void WakeClyde() {
        clyde.Spawn();
    }
}

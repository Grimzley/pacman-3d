using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class GhostManager : MonoBehaviour {

    public Transform player;
    public Transform spawn;

    public GhostMovement blinky; // -> player
    public GhostMovement pinky; // -> 2 units in front of player
    public GhostMovement inky; // -> yes...
    public GhostMovement clyde; // -> player if far, spawn if close

    private void Start() {

    }

    private void Update() {
        
    }

    private void FixedUpdate() {
        
    }
}

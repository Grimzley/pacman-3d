using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

    public static PlayerMovement instance;

    Rigidbody rb;

    public float speed;
    public float groundDrag;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    AudioManager sounds;
    bool isWalking;
    bool isHeartBeating;

    List <Collider> proximityList = new List<Collider>();

    private void Awake() {
        instance = this;
    }

    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.drag = groundDrag;
        sounds = FindObjectOfType<AudioManager>();
        isWalking = false;
        isHeartBeating = false;
    }

    private void Update() {
        MyInput();
        SpeedControl();
    }

    private void FixedUpdate() {
        MovePlayer();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Dot")) {
            other.gameObject.SetActive(false);
            GameManager.instance.AddScore(1);
        }else if (other.gameObject.layer == LayerMask.NameToLayer("PowerDot")) {
            other.gameObject.SetActive(false);
            GameManager.instance.AddScore(1);
            GhostManager.instance.FrightenMode();
        }else if (other.gameObject.layer == LayerMask.NameToLayer("Ghost")) {
            proximityList.Add(other);
            if (!isHeartBeating) {
                isHeartBeating = true;
                sounds.Play("Heartbeat");
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ghost") && isHeartBeating) {
            proximityList.Remove(other);
            if (proximityList.Count == 0) {
                isHeartBeating = false;
                sounds.Pause("Heartbeat");
            }
        }
    }

    private void MyInput() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("Menu");
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if ((horizontalInput != 0 || verticalInput != 0) && !isWalking) {
            isWalking = true;
            sounds.Play("Walking");
        }
        if (horizontalInput == 0 && verticalInput == 0 && isWalking) {
            isWalking = false;
            sounds.Pause("Walking");
        }
    }

    private void SpeedControl() {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude > speed) {
            Vector3 limitVel = flatVel.normalized * speed;
            rb.velocity = new Vector3(limitVel.x, rb.velocity.y, limitVel.z);
        }
    }

    private void MovePlayer() {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * speed, ForceMode.Force);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    public float sensX;
    public float sensY;
    public Transform orientation;
    float xRotation;
    float yRotation;

    private void Start() {
        xRotation = 0f;
        yRotation = 0f;
    }

    private void Update() {
        if (!GameManager.instance.isPaused) {
            float mouseX = Input.GetAxisRaw("Mouse X") * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * sensX;

            yRotation += mouseX;
            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        }
    }
    private void LateUpdate() {
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}

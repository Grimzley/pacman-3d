using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform cameraPos;

    private void Update() {
        transform.position = cameraPos.position;
    }
}

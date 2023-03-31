using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private GameObject player;
    private Camera camera;
   [NonSerialized] public bool isStopped = false;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        camera = Camera.main;
    }

    private void Update() {
        if (!isStopped) {
            camera.transform.position = player.transform.position + new Vector3(0, 0, -10);
        }
    }
}

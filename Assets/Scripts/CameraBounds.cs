using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    private Camera mainCam;
    private BoxCollider2D boxCollider;

    private void Start() {
        boxCollider = GetComponent<BoxCollider2D>();
        mainCam = Camera.main;
    }

    private void LateUpdate() {
        float vertExtent = mainCam.orthographicSize;
        float horizExtent = vertExtent * Screen.width / Screen.height;
        
        Vector3 linkedCameraPos = mainCam.transform.position;
        Bounds areaBounds = boxCollider.bounds;
        
        mainCam.transform.position = new Vector3(
            Mathf.Clamp(linkedCameraPos.x, areaBounds.min.x + horizExtent, areaBounds.max.x - horizExtent),
            Mathf.Clamp(linkedCameraPos.y, areaBounds.min.y + vertExtent, areaBounds.max.y - vertExtent),
            linkedCameraPos.z);
    }
}

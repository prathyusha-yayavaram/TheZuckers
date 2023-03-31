using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShaker : MonoBehaviour {
    private GameObject player;
    
    // Start is called before the first frame update
    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public IEnumerator Shake(float duration, float magnitude) {
        Vector3 origPos = transform.localPosition;
        float elapsed = 0.0f;
        while (elapsed < duration) {
            Vector3 transformPosition = player.transform.position;
            float xOff = Random.Range(-1f, 1f) * magnitude;
            float yOff = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(transformPosition.x + xOff, transformPosition.y + yOff, origPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorTracking : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform parent;
    void Start()
    {
        parent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LateUpdate()
    {
        //Vector2 xy = cam.ScreenToWorldPoint(Input.mousePosition);
        //transform.position = new Vector3(xy.x, xy.y, 0.0f);
        
        transform.SetParent(null);
        transform.SetParent(parent);
        transform.position = Input.mousePosition;

    }
}

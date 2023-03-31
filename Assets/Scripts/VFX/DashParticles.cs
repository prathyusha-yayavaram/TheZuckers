using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashParticles : MonoBehaviour
{
    // Start is called before the first frame update
    private float mLifetime = 0.5f;
    private float elapsedTime = 0.0f;
    private Color startColor = Color.green;
    private Color endColor = Color.yellow;
    private Material myMaterial;
    private Dash mScript;
    void Start()
    {
    }

    void Awake()
    {
        myMaterial = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        elapsedTime = Mathf.Min(elapsedTime, mLifetime);
        myMaterial.SetColor("_Color", Color.Lerp(startColor, endColor, elapsedTime / mLifetime));
        if (elapsedTime >= mLifetime)
        {
            if (mScript.images.Count > 0)
            {
                mScript.images.Dequeue();
            }
            Destroy(this.gameObject, 0.1f);
        }
    }

    
    
    public void InitializeParticle(Sprite cSprite, Dash pScript, Color pStart, Color pEnd)
    {
        GetComponent<SpriteRenderer>().sprite = cSprite;
        mScript = pScript;
        startColor = pStart;
        endColor = pEnd;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField] private int numberImages = 3;
    [SerializeField] private float recordInLastSeconds = 0.1f;
    [SerializeField] private Color startColor = Color.green;
    [SerializeField] private Color endColor = Color.yellow;
    [SerializeField] private GameObject dashParticle;
    
    public Queue<GameObject> images;

    private float mDashTime = 0.0f;
    private bool mTriggered = false;
    private float mElapsedTime = 0.0f;
    private float mSingleFrameTime = 0.0f;
    private int numAdded = 0;
    private Sprite mSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        mSprite = GetComponent<SpriteRenderer>().sprite;
        images = new Queue<GameObject>(numberImages);
        mSingleFrameTime = recordInLastSeconds / numberImages;
        mDashTime = GetComponent<PlayerController>().dashingTime;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (mTriggered)
        {
            mElapsedTime += Time.deltaTime;
            if (mElapsedTime >= mSingleFrameTime)
            {
                mElapsedTime -= mSingleFrameTime;
                CreateParticles(mSprite);
            }
        }
    }

    public void StartParticles()
    {
        StopParticles();
        mTriggered = true;
        mElapsedTime = 0.0f;
        numAdded = 0;
    }

    private void CreateParticles(Sprite curSprite)
    {
        if (images.Count < numberImages && numAdded < numberImages)
        {
            GameObject obj = Instantiate(dashParticle, transform.position, transform.rotation);
            obj.transform.localScale = transform.localScale;
            obj.GetComponent<DashParticles>().InitializeParticle(curSprite, this, startColor, endColor);
            images.Enqueue(obj);
            numAdded++;
        }
        else
        {
            mTriggered = false;
        }
    }
    
    public void StopParticles()
    {
        images.Clear();
    }
}

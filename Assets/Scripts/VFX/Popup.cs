using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Popup : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera cam;
    public bool clicked = false;
    private Vector3 origScale;

    [SerializeField] private Vector3 startingScale;
    [SerializeField] private float growthTimer = 0.4f;
    [SerializeField] private float fullGrowthTimer = 0.5f;
    [SerializeField] private float fullScale = 2f;
    [SerializeField] private float deathTimer = 0.15f;
    [SerializeField] private float popTimer = 0.03f;

    public bool growing = false;
    public bool decaying = false;

    void Start()
    {
        cam = Camera.main;
        origScale = startingScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Grow()
    {
        growing = true;
        decaying = false;
        float elapsed = 0.0f;
        float diff = fullGrowthTimer - growthTimer;
        while (elapsed < fullGrowthTimer)
        {
            //Debug.Log(elapsed);
            if (elapsed < growthTimer)
            {
                transform.localScale = Vector3.Lerp(Vector3.zero, origScale * fullScale, elapsed / growthTimer);
            }
            else
            {
                transform.localScale = Vector3.Lerp(origScale * fullScale, origScale, (elapsed - growthTimer) / diff);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = origScale;
    }

    public IEnumerator Die()
    {
        decaying = true;
        growing = false;
        float elapsed = 0.0f;
        float diff = deathTimer - popTimer;
        while (elapsed < deathTimer)
        {
            if (elapsed < popTimer)
            {
                transform.localScale = Vector3.Lerp(origScale, origScale * fullScale, elapsed / popTimer);
            }
            else
            {
                transform.localScale = Vector3.Lerp(origScale * fullScale, Vector3.zero, (elapsed - popTimer) / diff);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class ScreenTransition : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float transitionInDuration = 1.5f;
    [SerializeField] private float transitionOutDuration = 0.75f;
    private Material myMaterial;
    [SerializeField] private Texture textureIn;
    [SerializeField] private Texture textureOut;
    private bool transitioningIn = false;
    private bool transitioningOut = false;
    private float forgiveDuration = 0.15f;
    void Start()
    {
        myMaterial = GetComponent<SpriteRenderer>().material;
        myMaterial.SetFloat("_Cutoff", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator TransitionIn(Action call)
    {
        if (transitioningOut) yield break;
        transitioningIn = true;
        myMaterial.SetTexture("_SecondaryTex", textureIn);
        float elapsed = 0.0f;
        while (elapsed < transitionInDuration + forgiveDuration)
        {
            myMaterial.SetFloat("_Cutoff", Mathf.Lerp(1.0f, 0.0f, elapsed/transitionInDuration));
            elapsed += Time.deltaTime;
            yield return null;
        }
        myMaterial.SetFloat("_Cutoff", 0.0f);
        transitioningIn = false;
        call.Invoke();
    }

    public IEnumerator TransitionOut() {
        yield return new WaitForSeconds(0.5f);
        if (transitioningIn) yield break;
        transitioningOut = true;
        myMaterial.SetTexture("_SecondaryTex", textureOut);
        float elapsed = 0.0f;
        while (elapsed < transitionOutDuration + forgiveDuration)
        {
            myMaterial.SetFloat("_Cutoff", Mathf.Lerp(0.0f, 1.0f, elapsed/transitionOutDuration));
            elapsed += Time.deltaTime;
            yield return null;
        }
        myMaterial.SetFloat("_Cutoff", 1.0f);
        transitioningOut = false;
    }
}

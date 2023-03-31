using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private void Start() {
        DontDestroyOnLoad(this);
    }

    public void SwitchToStartMap() {
        SceneManager.LoadScene("StartingAreaScene");
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpecialGoController : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GenerateNextLevel();
        }
    }

    private void GenerateNextLevel()
    {
        SceneManager.LoadScene("MainGame");
    }
    
}

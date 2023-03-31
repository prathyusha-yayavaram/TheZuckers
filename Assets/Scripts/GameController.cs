using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameObject player;
    private MapGenerator map;
    private GameObject canvas;

    private ScreenTransition transition;
    // private GameObject camera;
    private void Awake() {
        transition = Camera.main.gameObject.GetComponentInChildren<ScreenTransition>();
        player = GameObject.FindWithTag("Player");
        map = GameObject.FindWithTag("Map").GetComponent<MapGenerator>();
        StartLevel();
    }

    public void StartLevel()
    {
        map.StartUpMapGenerator();
        Vector2 mapPlayerSpawn = map.playerSpawn;
        player.transform.position = mapPlayerSpawn;
    }

    private void RefreshLevel()
    {
        map.ClearMapGenerator();
        GameObject[] findGameObjectsWithTag = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject obj in findGameObjectsWithTag) {
            Destroy(obj);
        }

        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        for (int i = 0; i < canvas.transform.childCount; i++) {
            Destroy(canvas.transform.GetChild(i).gameObject);
        }
        

        StartLevel();
        StartCoroutine(transition.TransitionOut());
    }

    public void RefreshLevelExposed() {
        StartCoroutine(transition.TransitionIn(RefreshLevel));
    }
    
    
}

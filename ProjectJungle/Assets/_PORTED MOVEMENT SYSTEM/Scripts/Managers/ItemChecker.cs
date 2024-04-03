using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChecker : MonoBehaviour
{
    private LevelLoader levelLoader;
    // Start is called before the first frame update
    void Start()
    {
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (NewInventoryManager.Instance.AllTrinketsCollected() && other.CompareTag("Player"))
        {
            levelLoader.LoadNextLevel();
        }
    }
}

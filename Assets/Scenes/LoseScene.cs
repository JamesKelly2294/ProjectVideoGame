using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoseScene : MonoBehaviour
{

    public TextMeshProUGUI shots, xp;

    // Start is called before the first frame update
    void Start()
    {
        GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
        shots.text = "" + gameManager.pastState.timesShot + " Shots Fired";
        xp.text = "" + gameManager.pastState.totalXPGained + " XP Gained";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToMainMenu()
    {
        GameObject.FindObjectOfType<GameManager>().ShowMainWindow();
    }
}

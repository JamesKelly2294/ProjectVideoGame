using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    public void LoadGame()
    {
        if (GameManager.instance)
        {
            GameManager.instance.LoadGame();
        }
    }
}

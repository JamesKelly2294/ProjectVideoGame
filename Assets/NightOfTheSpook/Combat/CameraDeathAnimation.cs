using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDeathAnimation : MonoBehaviour
{

    private float time = 0, totalTime = 8f, speed = 1f;

    // Start is called before the first frame update
    void Start() {
    }

    bool _showedLoseScreen;

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = 1;
        time += Time.deltaTime;
        if (time <= totalTime) {
            gameObject.transform.localPosition += new Vector3(0, speed * Time.deltaTime, -speed * Time.deltaTime);
        } else {
            // End the game...
            if (_showedLoseScreen)
            {
                return;
            }
            else
            {
                _showedLoseScreen = true;
                GameObject.FindObjectOfType<GameManager>().ShowLoseScreen(GameObject.FindObjectOfType<SpookyGameManager>().state);
            }
        }
    }
}

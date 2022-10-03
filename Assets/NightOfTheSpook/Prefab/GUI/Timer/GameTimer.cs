using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI text;

    // Lol this should not be calculated here...
    private float _gameTime = 0;

    private WaveManager waveManager;

    // Start is called before the first frame update
    void Start()
    {
        waveManager = GameObject.FindObjectOfType<WaveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        _gameTime += Time.deltaTime;

        var minutes = Mathf.FloorToInt(_gameTime / 60.0f);
        var seconds = Mathf.FloorToInt(_gameTime - (minutes * 60.0f));

        text.text = string.Format("{0:0}:{1:00}", minutes, seconds);

        if (waveManager != null) {
            text.text += " - Wave " + (waveManager.waveIndex + 1);
        }
    }
}

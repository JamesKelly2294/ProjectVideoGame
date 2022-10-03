using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Wave
{
    public GameObject WaveGO;
    public float EndTime;
}

public class WaveManager : MonoBehaviour
{
    public Wave ActiveWave;
    public float ElapsedTime;

    public int waveIndex = 0;
    public List<Wave> Waves = new List<Wave>();

    // Start is called before the first frame update
    void Awake()
    {
        foreach(var wave in Waves)
        {
            wave.WaveGO.SetActive(false);
        }

        ActiveWave = Waves[0];
        ActiveWave.WaveGO.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        ElapsedTime += Time.deltaTime;

        if (ElapsedTime > ActiveWave.EndTime && waveIndex < Waves.Count - 1)
        {
            ActiveWave.WaveGO.SetActive(false);
            waveIndex++;
            ActiveWave = Waves[waveIndex];
            ActiveWave.WaveGO.SetActive(true);
        }
    }
}

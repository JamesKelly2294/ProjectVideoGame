using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyGameManager : MonoBehaviour
{

    public float timerTime = 0f;
    public float timerTotalTime = 10f;

    public int money = 231;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timerTime += Time.deltaTime;
        if (timerTime > timerTotalTime) {
            timerTime -= timerTotalTime;
            Fire();
        }
    }

    void Fire()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyGameManager : MonoBehaviour
{

    public float timerTime = 0f;
    public float timerTotalTime = 10f;

    public int money = 0;

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
        this.GetComponent<PubSubSender>().Publish("timer.didFire");
    }

    public void IDiedHandler(PubSubListenerEvent e) {

        Enemy enemy = (e.value as GameObject).GetComponent<Enemy>();
        if (enemy != null) {
            money += enemy.experienceValue;
        }
    }
}

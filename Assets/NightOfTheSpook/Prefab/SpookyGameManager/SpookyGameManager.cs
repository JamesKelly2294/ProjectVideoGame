using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyGameManager : MonoBehaviour
{

    public float timerTime = 0f;
    public float timerTotalTime = 10f;

    public float xp = 0;
    public float xpGoal = 100;

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
            xp += enemy.experienceValue;
            if (xp >= xpGoal) {
                BeginUpgrade();
            }
        }
    }

    public void BeginUpgrade() {
        Time.timeScale = 0;
        this.GetComponent<PubSubSender>().Publish("upgradeTime.shouldBegin");
    }

    public void EndUpgrade() {
        Time.timeScale = 1;
        this.GetComponent<PubSubSender>().Publish("upgradeTime.hasEnded");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyGameManager : MonoBehaviour
{

    public float timerTime = 0f;
    public float timerTotalTime = 10f;
    public bool alive = true;

    public float xp = 0;
    public float xpGoal = 50;

    // Used to tell the end screens what happened.
    public SpookyGameSummaryState state = new SpookyGameSummaryState();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!alive) { return; }
        timerTime += Time.deltaTime;
        if (timerTime > timerTotalTime) {
            timerTime -= timerTotalTime;
            Fire();
        }
    }

    void Fire()
    {
        state.timesShot += 1;
        this.GetComponent<PubSubSender>().Publish("timer.didFire");
    }

    public void IDiedHandler(PubSubListenerEvent e) {
        if (!alive) { return; }
        Enemy enemy = (e.value as GameObject).GetComponent<Enemy>();
        if (enemy != null) {
            xp += enemy.experienceValue;
            state.totalXPGained += enemy.experienceValue;
            if (xp >= xpGoal) {
                BeginUpgrade();
            }
        }
    }

    public void BeginUpgrade() {
        if (!alive) { return; }
        Time.timeScale = 0;
        this.GetComponent<PubSubSender>().Publish("upgradeTime.shouldBegin");
    }

    public void ApplyUpgrade(PubSubListenerEvent e) {
        UpgradeOption option = (e.value as PubSubSender).gameObject.GetComponent<UpgradeButton>().upgradeOption;
        GetComponent<UpgradeManager>().PurchaseUpgrade(option);
        EndUpgrade();
    }

    public void EndUpgrade() {
        xp -= xpGoal;
        xpGoal *= 2;
        Time.timeScale = 1;
        this.GetComponent<PubSubSender>().Publish("upgradeTime.hasEnded");
    }

    public void HandlePlayerDeath() {
        alive = false;
        timerTime = 0;
    }
}

public class SpookyGameSummaryState {
    public int timesShot = 0;
    public float totalXPGained = 0;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyGameManager : MonoBehaviour
{

    public float timerTime = 0f;
    public float timerTotalTime = 10f;
    public bool alive = true;

    public float xp = 0;
    public int level = 0;

    // Used to tell the end screens what happened.
    public SpookyGameSummaryState state = new SpookyGameSummaryState();

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.StopMusic();

        AudioManager.Instance.Play("Music/Background", true, volumeMin: 0.25f, volumeMax: 0.25f, isMusic: true);
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
            if (xp >= XPGoal) {
                xp -= XPGoal;
                BeginUpgrade();
                level++;
            }
        }
    }

    public float XPGoal
    {
        get {
            switch(level)
            {
                case 0:
                    return 100;
                case 1:
                    return 500;
                case 2:
                    return 1250;
                case 3:
                    return 2500;
                case 4:
                    return 3500;
                case 5:
                    return 4000;
                case 6:
                    return 5000;
                case 7:
                    return 5500;
                case 8:
                    return 6750;
                case 9:
                    return 7000;
                case 10:
                    return 7250;
                case 11:
                    return 7500;
                default:
                    return 7500;
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
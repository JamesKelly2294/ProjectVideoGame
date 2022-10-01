using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStateHeader : MonoBehaviour
{

    public ProgressBar healthBar;
    public ProgressBar chargeBar;
    public TextMeshProUGUI moneyTextField;

    private SpookyGameManager spookyGameManager;
    private Attackable playerAttackable;

    // Start is called before the first frame update
    void Start()
    {
        spookyGameManager = GameObject.FindObjectOfType<SpookyGameManager>();
        playerAttackable = GameObject.FindObjectOfType<Attackable>();
    }

    // Update is called once per frame
    void Update()
    {
        chargeBar.SetProgress(spookyGameManager.timerTime / spookyGameManager.timerTotalTime);
        healthBar.SetProgress(playerAttackable.Health / playerAttackable.TotalHealth);
        moneyTextField.text = "" + spookyGameManager.money;
    }
}

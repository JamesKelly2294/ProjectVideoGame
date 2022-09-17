using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    public GameObject menuPanel;

    public bool IsPresentingSubview { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsPresentingSubview && Input.GetKeyDown(KeyCode.Escape))
        {
            menuPanel.SetActive(!menuPanel.activeSelf);
        }
    }
}

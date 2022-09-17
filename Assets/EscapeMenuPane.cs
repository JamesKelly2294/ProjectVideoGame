using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenuPane : MonoBehaviour
{
    public GameObject Background, PaneContent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Background.SetActive(!Background.activeInHierarchy);
            PaneContent.SetActive(!PaneContent.activeInHierarchy);
        }       
    }

    public void Hide()
    {
        Background.SetActive(false);
        PaneContent.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        GameObject.FindObjectOfType<GameManager>().ShowMainWindow();
    }
}

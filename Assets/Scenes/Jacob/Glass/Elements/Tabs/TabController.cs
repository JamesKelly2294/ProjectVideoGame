using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{

    public List<GameObject> tabs;
    public Image borderImage;

    public TabButton currentTab;

    // Start is called before the first frame update
    void Start()
    {
        SetTab(currentTab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTab(TabButton tab)
    {
        currentTab.image.enabled = false;
        currentTab = tab;
        tab.image.enabled = true;
        for (int i = 0; i < tabs.Count; i++)
        {
            tabs[i].SetActive(i == currentTab.tab);
            if (i == currentTab.tab) {
                 borderImage.color = tab.image.color;
            }
        }
    }
}

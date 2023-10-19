using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BottomBarTabHandler : MonoBehaviour
{
    public GameObject[] panels;
    public GameObject newPanel;
    public Image[] tabs;
    public TMP_Text[] texts;
    public Image newTab;
    public TMP_Text newText;

    public void ChangeTabs()
    {
        foreach (GameObject obj in panels)
        {
            obj.SetActive(false);
        }

        newPanel.SetActive(true);
        
        foreach (Image img in tabs)
        {
            img.color = Utils.TabColors["TAB_UNUSED"];
        }

        foreach (TMP_Text text in texts)
        {
            text.color = Utils.TabColors["TAB_UNUSED"];
        }

        newTab.color = Utils.TabColors["TAB_USED"];
        newText.color = Utils.TabColors["TAB_USED"];
    }
}

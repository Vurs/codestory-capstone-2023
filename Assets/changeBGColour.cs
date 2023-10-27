using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class changeBGColour : MonoBehaviour
{
    public Toggle toggle;
    public TextMeshProUGUI label;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (toggle.isOn == true)
        {
            //Camera.main.backgroundColor = newColour;
            //Debug.Log("Checked");
            Camera.main.backgroundColor = new Color(0.145098f, 0.145098f, 0.1490196f);

            label.color = new Color(1f, 1f, 1f);

        }
        else {
            Camera.main.backgroundColor = new Color(0.929f, 0.929f, 0.929f);
            //Debug.Log("Unchecked");
            label.color = new Color(0.102f, 0.102f, 0.102f);
        }

    }



    
}

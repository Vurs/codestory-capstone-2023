using System;
using Unity.VisualScripting;
using UnityEngine;

public class movement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 ) {
            Debug.Log("Button Clicked");
            //Vector3 position = this.transform.position;
            //position.y++;
            //this.transform.position = position;
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimateSpinner : MonoBehaviour
{
    public Image spinnerImage;
    public float rotationSpeed = 30.0f;
    public static bool isSpinning = false;

    private void Update()
    {
        if (isSpinning == true)
        {
            spinnerImage.enabled = true;
            transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);
        } else
        {
            spinnerImage.enabled = false;
        }
    }
}

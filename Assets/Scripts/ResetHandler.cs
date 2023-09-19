using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetHandler : MonoBehaviour
{
    public AnswerPositionHandler positionHandler;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        positionHandler.RandomizeStartPositions();
    }
}

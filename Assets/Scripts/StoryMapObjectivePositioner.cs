using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryMapObjectivePositioner : MonoBehaviour
{
    public GameObject mapObjectivePrefab;
    public float prefabHeight;
    public float subtractor;
    public GameObject parent;
    public int numOfObjectives;
    public float spacing;
    public float multiplier;
    public float sineMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numOfObjectives; i++)
        {
            GameObject clone = Instantiate(mapObjectivePrefab);
            clone.transform.SetParent(parent.transform, false);
            RectTransform rectTransform = clone.GetComponent<RectTransform>();
            rectTransform.SetLocalPositionAndRotation(new Vector3(Mathf.Sin(i * sineMultiplier) * multiplier, ((i * prefabHeight * -1) - (spacing * i)) + subtractor, 0), Quaternion.identity);
            // rectTransform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        }
    }
}

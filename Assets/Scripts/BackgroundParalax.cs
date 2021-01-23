using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParalax : MonoBehaviour
{
    private GameObject[][][] backgroundImageGroups;
    private GameObject background;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "Backgrounds")
            {
                background = child.gameObject;
            }
        }
        CreateChildGroups();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateChildGroups()
    {
        int childCount = 0;
        foreach (Transform child in background.transform)
        {
            childCount += 1;
        }
        if (childCount == 0)
        {
            return;
        }

        backgroundImageGroups = new GameObject[childCount][][];
        foreach (Transform child in background.transform) 
        {
            GameObject clone = Instantiate(child.gameObject);
            SpriteRenderer renderer = child.GetComponent<SpriteRenderer>();
            //columnCount and rowCount will need to scale with size of graphic eventualy
            int columnCount = 2; //at least 1 more than needed to fill screen (2 is sufficiant for large images)
            GameObject[][] childColumns = new GameObject[columnCount][];
            int rowCount = 2;
            float columnOffset = renderer.bounds.extents.x * 2;
            float rowOffset = renderer.bounds.extents.y * 2;
            for (int i = 0; i < columnCount; i++)
            {
                GameObject[] column = new GameObject[rowCount];
                string columnName = "Column " + i;
                GameObject columnGroup = new GameObject(columnName);


                for (int j = 0; j < rowCount; j++)
                {
                    GameObject newSprite = Instantiate(clone.gameObject);
                    newSprite.name = "R" + j;
                    newSprite.transform.position -= new Vector3(columnOffset * i, rowOffset * j);
                    column[j] = newSprite;
                    newSprite.transform.parent = columnGroup.transform;
                }
                columnGroup.transform.parent = child.transform;
            }
            renderer.enabled = false;
            GameObject.Destroy(clone);
        }
    }
}

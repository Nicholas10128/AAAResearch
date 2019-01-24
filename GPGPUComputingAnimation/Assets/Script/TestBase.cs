using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBase : MonoBehaviour
{
    public GameObject testModel;
    public Transform[] characterPoints;

    public float spaceX = -0.1f;
    public float spaceY = -0.1f;
    public int numInRow = 40;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void CreateTestGameObjects(int num)
    {
        characterPoints = new Transform[num];
        for (int i = 0; i < num; i++)
        {
            characterPoints[i] = GameObject.Instantiate(testModel).transform;
            characterPoints[i].position = new Vector3(0, 0, spaceY) * (i / numInRow);
            characterPoints[i].position += new Vector3(spaceX, 0, 0) * (i % numInRow);
        }
    }
}

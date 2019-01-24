using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputWindow : MonoBehaviour
{
    public GameObject m_TestObject;
    private bool m_Start = false;
    private string m_InputNumber;

    private static float width = 200;
    private static float height = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGUI()
    {
        if (m_Start)
        {
            return;
        }

        m_InputNumber = GUI.TextArea(new Rect((Screen.width - width) / 2, Screen.height / 2 - height, width, height), m_InputNumber);
        if (GUI.Button(new Rect((Screen.width - width) / 2, Screen.height / 2, width, height), "start"))
        {
            TestBase testBase = m_TestObject.GetComponent<TestBase>();
            if (null != testBase)
            {
                testBase.CreateTestGameObjects(Convert.ToInt32(m_InputNumber));
            }

            m_TestObject.SetActive(true);
            m_Start = true;
        }
    }
}

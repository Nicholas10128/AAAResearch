using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enter : MonoBehaviour
{
    // Start is called before the first frame update
    private float width = 400;
    private float height = 200;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        GUIStyle gUIStyle = new GUIStyle();
        gUIStyle.fontSize = 60;
        if(GUI.Button(new Rect((Screen.width - width) / 2, Screen.height / 2 - height * 2f, width, height), "VFShader", gUIStyle)){
            SceneManager.LoadScene("VFTest");
        }

        if (GUI.Button(new Rect((Screen.width - width) / 2, Screen.height / 2 - height * 1f, width, height), "CSShader01", gUIStyle))
        {
            SceneManager.LoadScene("CSTest");
        }

        if (GUI.Button(new Rect((Screen.width - width) / 2, Screen.height / 2 + height * 0f, width, height), "Animation", gUIStyle))
        {
            SceneManager.LoadScene("Animation");
        }

        if (GUI.Button(new Rect((Screen.width - width) / 2, Screen.height / 2 + height * 1f, width, height), "VertexAnim", gUIStyle))
        {
            SceneManager.LoadScene("VertexAnim");
        }
    }
}

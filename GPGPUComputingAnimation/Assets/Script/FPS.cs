using UnityEngine;
using System.Collections;

public class FPS : MonoBehaviour
{
    public float updateInterval = 0.5F;
    private double lastInterval;
    private int frames = 0;
    private float fps;
    void Awake()
    {
        //		Application.runInBackground = true;
        //		Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //		Application.targetFrameRate = 30;
    }
    void Start()
    {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
    }
    void OnGUI()
    {
        GUIStyle gUIStyle = new GUIStyle();
        gUIStyle.normal.textColor = Color.red;
        gUIStyle.fontSize = 60;
        GUILayout.BeginArea(new Rect(Screen.width / 2 - 400, 100, 300, 100));
        GUILayout.Label("fps:" + fps.ToString("f2"), gUIStyle);
        GUILayout.EndArea();
    }
    void Update()
    {
        ++frames;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > lastInterval + updateInterval)
        {
            fps = (float)(frames / (timeNow - lastInterval));
            frames = 0;
            lastInterval = timeNow;
        }
    }
}
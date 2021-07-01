using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounterBehaviour : MonoBehaviour
{
    private FPSCounter _fps;

    // Start is called before the first frame update
    void Start()
    {
        _fps = new FPSCounter();
    }

    // Update is called once per frame
    void Update()
    {
        _fps?.OnUpdate();
    }

    private void OnGUI()
    {
        if (_fps != null)
        {
            if (GUILayout.Button(_fps.FPS.ToString("f2"), GUILayout.Width(90), GUILayout.Height(50)))
            { }
        }
    }

}


//Ö¡ÂÊ¼ÆÊý
internal class FPSCounter
{
    private float _lastTime;
    private float _fpsCount = 0;
    private float _fps;

    public float FPS { get { return _fps; } }


    public FPSCounter()
    {
        _lastTime = Time.realtimeSinceStartup;
        _fpsCount = 0;
        _fps = 0;
    }

    public void OnUpdate()
    {
        float intervalTime = Time.realtimeSinceStartup - _lastTime;
        _fpsCount++;
        if (intervalTime > 1)
        {
            _fps = _fpsCount / intervalTime;
            _fpsCount = 0;
            _lastTime = Time.realtimeSinceStartup;
        }
    }
}

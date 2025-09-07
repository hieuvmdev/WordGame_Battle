using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FpsDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;

    public Text FpsTxt;

    private float minFps = 60;
    private float maxFps = 0;
    private bool isShow = false;

    float msec = 0;
    float fps = 0;

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (isShow)
            {
                OnHide();
            }
            else
            {
                OnShow();
            }
        }

        if (!isShow)
        {
            return;
        }

        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        msec = deltaTime * 1000.0f;
        fps = 1.0f / deltaTime;
        FpsTxt.text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
    }

    void OnGUI()
    {
        /*if (!isShow)
        {
            return;
        }
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 10 / 100;
        style.normal.textColor = new Color(1f, 1f, 1f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);*/
    }

    public void OnShow()
    {
        isShow = true;
    }
    public void OnHide()
    {
        isShow = false;
        FpsTxt.text = "";
    }
}
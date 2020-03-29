using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EditorDebugTool
{
     static  bool overDraw = false;
    [MenuItem("调试/OverDraw")]
    public static void OverDraw()
    {
        if (overDraw == false)
        {
            Camera.main.clearFlags = CameraClearFlags.Color;
            Camera.main.SetReplacementShader(Shader.Find("Debug/DebugOverDraw"), "");
        }
        else
        {
            Camera.main.clearFlags = CameraClearFlags.Skybox;
            Camera.main.SetReplacementShader(null, "");
        }
        overDraw = !overDraw;
    }
  
}

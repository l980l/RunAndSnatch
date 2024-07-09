using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Logging
{
    [System.Diagnostics.Conditional("ENABLE_LOG")]  
    static public void Log(object message)
    {
        UnityEngine.Debug.Log(message);
    }
}

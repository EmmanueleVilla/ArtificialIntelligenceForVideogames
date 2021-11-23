using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using ILogger = Core.Utils.Log.ILogger;

public class TextLogger : MonoBehaviour, ILogger
{
    public Text Output;
    System.Threading.Thread mainThread;
    public void Start()
    {
        mainThread = System.Threading.Thread.CurrentThread;
    }

    bool isMainThread()
    {
        return mainThread.Equals(System.Threading.Thread.CurrentThread);
    }
    public void WriteLine(string message)
    {
        if (isMainThread())
        {
            var text = Output.text;
            var lines = string.Join("\n", text.Split('\n').ToList().Take(19));
            lines += "\n" + message;
            Output.text = lines;
        }
    }
}

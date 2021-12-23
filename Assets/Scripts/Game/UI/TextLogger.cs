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
            Debug.Log(message);
            var text = Output.text;
            var split = text.Split('\n');
            var diff = Mathf.Max(0, split.Count() - 20);
            var lines = string.Join("\n", split.Skip(diff).Take(20));
            lines += "\n" + message;
            Output.text = lines;
        }
    }
}

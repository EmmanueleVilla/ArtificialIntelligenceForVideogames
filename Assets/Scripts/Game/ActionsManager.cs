using Logic.Core.Battle;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ActionsManager : MonoBehaviour
{
    public GameManager GameManager;
    public GameObject[] Buttons;

    public void Start()
    {
        foreach (var button in Buttons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void SetActions(AvailableActions actions)
    {
        foreach(var button in Buttons)
        {
            button.gameObject.SetActive(false);
        }
        if(actions.Movements.Any(x => x.Item2 > 0))
        {
            Buttons[0].gameObject.SetActive(true);
            Buttons[0].gameObject.GetComponentInChildren<Text>().text = "Move";
        }
    }

    public void SelectAction(int index)
    {
        
    }
}

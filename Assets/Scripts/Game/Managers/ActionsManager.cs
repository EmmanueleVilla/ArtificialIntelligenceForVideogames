using Logic.Core.Battle;
using Logic.Core.Battle.Actions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ActionsManager : MonoBehaviour
{
    public GameManager GameManager;
    public GameObject[] Buttons;

    List<IAvailableAction> Actions;
    public void Start()
    {
        foreach (var button in Buttons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void SetActions(List<IAvailableAction> actions)
    {
        Actions = actions;
        foreach (var button in Buttons)
        {
            button.gameObject.SetActive(false);
        }

        for (int i = 0; i < actions.Count && i < Buttons.Length; i++)
        {
            Buttons[i].gameObject.SetActive(true);
            Buttons[i].gameObject.GetComponentInChildren<Text>().text = actions[i].Description;
        }
    }

    public void SelectAction(int index)
    {
        switch(Actions[index].ActionType)
        {
            case ActionsTypes.Movement:
                SetActions(new List<IAvailableAction>() {
                    new CancelMovementAction()
                    });
                GameManager.TriggerMovementMode();
                break;
            case ActionsTypes.EndTurn:
                GameManager.NextTurn();
                break;
        }
    }
}

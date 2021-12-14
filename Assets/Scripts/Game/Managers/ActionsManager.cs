using Logic.Core.Battle;
using Logic.Core.Battle.Actions;
using Logic.Core.Battle.Actions.Attacks;
using Logic.Core.Battle.Actions.Movement;
using Logic.Core.Battle.Actions.Spells;
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
            case ActionsTypes.RequestMovement:
                GameManager.EnterMovementMode();
                break;
            case ActionsTypes.CancelMovement:
                GameManager.ExitMovementMode();
                break;
            case ActionsTypes.ConfirmMovement:
                var action = Actions[index] as ConfirmMovementAction;
                this.StartCoroutine(GameManager.ConfirmMovement(action.DestinationX, action.DestinationY, action.Damage, action.Speed));
                break;
            case ActionsTypes.EndTurn:
                GameManager.NextTurn();
                break;
            case ActionsTypes.RequestAttack:
                GameManager.EnterAttackMode(Actions[index] as RequestAttackAction);
                break;
            case ActionsTypes.CancelAttack:
                GameManager.ExitAttackMode();
                break;
            case ActionsTypes.ConfirmAttack:
                var confirmAttackAction = Actions[index] as ConfirmAttackAction;
                this.StartCoroutine(GameManager.ConfirmAttack(confirmAttackAction));
                break;
            case ActionsTypes.RequestSpell:
                GameManager.EnterSpellMode(Actions[index] as RequestSpellAction);
                break;
            case ActionsTypes.ConfirmSpell:
                this.StartCoroutine(GameManager.ConfirmSpell(Actions[index] as ConfirmSpellAction));
                break;
            case ActionsTypes.CancelSpell:
                GameManager.ExitSpellMode();
                break;
            default:
                GameManager.UseAbility(Actions[index]);
                break;
        }
    }
}

using Core.DI;
using Logic.Core.Battle;
using Logic.Core.Creatures;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class InitiativeUIManager : MonoBehaviour
{
    public GameManager GameManager;
    public Text InitiativeText;

    void Start()
    {
        GameManager.InitiativesRolled += GameManager_InitiativesRolled;
    }

    private void GameManager_InitiativesRolled(object sender, List<ICreature> initiatives)
    {
        var battle = DndModule.Get<IDndBattle>();
        var creatureInTurn = battle.GetCreatureInTurn();

        var builder = new StringBuilder();
        foreach (var creature in initiatives)
        {
            if (creatureInTurn == creature)
            {
                builder.Append("> ");
            }
            builder.AppendLine(creature.GetType().ToString().Split('.').Last());
        }

        InitiativeText.text = builder.ToString();
    }

}

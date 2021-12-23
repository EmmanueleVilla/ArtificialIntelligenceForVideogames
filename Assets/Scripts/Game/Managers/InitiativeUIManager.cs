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

    List<int> _initiatives;

    void Start()
    {
        GameManager.InitiativesRolled += GameManager_InitiativesRolled;
    }

    void Update()
    {
        //TODO: don't do this at every frame
        if (_initiatives != null)
        {
            var battle = DndModule.Get<IDndBattle>();
            var creatureInTurn = battle.GetCreatureInTurn();

            var builder = new StringBuilder();
            foreach (var creatureId in _initiatives)
            {
                if (creatureInTurn.Id == creatureId)
                {
                    builder.Append("> ");
                }
                var creature = battle.GetCreatureById(creatureId);
                builder.Append(creature.GetType().ToString().Split('.').Last());
                builder.AppendLine(string.Format(" {0}/{1} + {2}", creature.CurrentHitPoints, creature.HitPoints, creature.TemporaryHitPoints));
            }

            InitiativeText.text = builder.ToString();
        }
    }

    private void GameManager_InitiativesRolled(object sender, List<int> initiatives)
    {
        _initiatives = initiatives;
    }

}

using Core.DI;
using Logic.Core.Battle;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesManager : MonoBehaviour
{
    public Text ResourcesText;

    IDndBattle Battle;

    void Start()
    {
        Battle = DndModule.Get<IDndBattle>();
    }
    void Update()
    {
        //TODO: don't do this at every frame
        try
        {
            var creature = Battle.GetCreatureInTurn();

            var builder = new StringBuilder();
            if (creature != null && creature.Loyalty == Loyalties.Ally)
            {
                builder.AppendLine(String.Format("{0}/{1} HP, {2} CA", creature.CurrentHitPoints, creature.HitPoints, creature.ArmorClass));
                var monk = creature as IMonk;
                if (monk != null)
                {
                    builder.AppendLine(String.Format("{0}/{1} Ki points", monk.RemainingKiPoints, monk.KiPoints));
                }

                builder.AppendLine(string.Format("{0} Action {1} Bonus Action",
                    creature.ActionUsedNotToAttack || creature.ActionUsedToAttack ? "0" : "1",
                    creature.BonusActionUsedNotToAttack || creature.BonusActionUsedToAttack ? "0" : "1"
                    ));

                builder.AppendLine(string.Format("Action attacks: {0}", creature.RemainingAttacksPerAction));
            }
            ResourcesText.text = builder.ToString();
        } catch(Exception e)
        {

        }
    }
}

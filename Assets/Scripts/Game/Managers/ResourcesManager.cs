using Core.DI;
using Logic.Core.Battle;
using Logic.Core.Battle.Actions.Abilities;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Abilities;
using Logic.Core.Creatures.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                var monk = creature as IKiPointsOwner;
                if (monk != null)
                {
                    builder.AppendLine(String.Format("{0}/{1} Ki points", monk.RemainingKiPoints, monk.KiPoints));
                }
                var fightingSpirit = creature as IFightingSpirit;
                if(fightingSpirit != null)
                {
                    builder.AppendLine(String.Format("{0}/{1} Fighting Spirit", fightingSpirit.FightingSpiritRemaining, fightingSpirit.FightingSpiritUsages));
                }

                var caster = creature as ISpellCaster;
                if(caster != null)
                {
                    var slots = caster.RemainingSpellSlots.ToList().OrderBy(x => x.Key);
                    foreach(var slot in slots)
                    {
                        if(slot.Key == 0)
                        {
                            continue;
                        }
                        builder.AppendLine(string.Format("Level {0} slot: {1}", slot.Key, slot.Value));
                    }
                }

                var secondWind = creature as ISecondWind;
                if (secondWind != null)
                {
                    builder.AppendLine(String.Format("{0}/{1} Second Wind", secondWind.SecondWindRemaining, secondWind.SecondWindUsages));
                }
                builder.AppendLine(string.Format("{0} Action {1} Bonus Action",
                    creature.ActionUsedNotToAttack || creature.ActionUsedToAttack ? "0" : "1",
                    creature.BonusActionUsedNotToAttack || creature.BonusActionUsedToAttack ? "0" : "1"
                    ));

                builder.AppendLine(string.Format("Action attacks: {0}", creature.RemainingAttacksPerAction));
                builder.AppendLine(string.Format("Bonus action attacks: {0}", creature.RemainingAttacksPerBonusAction));
            }
            ResourcesText.text = builder.ToString();
        } catch(Exception e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Core.DI;
using Logic.Core.Actions;
using Logic.Core.Creatures.Scores;
using Logic.Core.Dice;
using Logic.Core.Movements;

namespace Logic.Core.Creatures
{
    public abstract class ACreature : ICreature
    {
        private readonly IDiceRoller roller;
        private readonly Random random;
        public int Id { get; set; }
        public List<Speed> remainingMovement { get; set; }

        protected ACreature(IDiceRoller roller = null, Random random = null, int id = -1)
        {
            this.roller = roller ?? DndModule.Get<IDiceRoller>();
            this.random = random ?? DndModule.Get<Random>();
            this.Id = id;
        }
        public ICreature Init()
        {
            Id = this.random.Next(0, int.MaxValue);
            remainingMovement = new List<Speed>(Movements);
            RemainingAttacksPerAction = AttacksPerAction;
            CurrentHitPoints = HitPoints;
            RemainingAttacksPerBonusAction = 0;
            RollInitiative();
            return this;
        }
        
        public virtual void ResetTurn()
        {
            remainingMovement = new List<Speed>(Movements);
            RemainingAttacksPerAction = AttacksPerAction;
            CurrentHitPoints = HitPoints;
            LastAttackUsed = null;
            RemainingAttacksPerBonusAction = 0;
            ActionUsedNotToAttack = false;
            ActionUsedToAttack = false;
            BonusActionUsedNotToAttack = false;
            BonusActionUsedToAttack = false;
            Disangaged = false;
        }

        //Abstract fields
        public abstract Loyalties Loyalty { get; }
        public abstract int Size { get; }
        public abstract int CriticalThreshold { get; }
        public abstract List<Attack> Attacks { get; }
        public abstract List<Speed> Movements { get; }
        public abstract int InitiativeModifier { get; }
        public abstract RollTypes InitiativeRollType { get; }
        public abstract int AttacksPerAction { get; }
        public abstract int HitPoints { get; }
        public abstract int ArmorClass { get; }
        public abstract AbilityScores AbilityScores { get; }

        //Implemented fields
        public virtual int RolledInitiative { get; protected set; }
        public List<Speed> RemainingMovement
        {
            get {
                var minus = 0;
                if(TemporaryEffectsList.FirstOrDefault(x => x.Item3 == TemporaryEffects.SpeedReducedByTwo) != null)
                {
                    minus = 2;
                }
                var result = remainingMovement.Select(x => new Speed(x.Movement, x.Square - minus)).ToList();
                return result;
            }
            set {
                remainingMovement = value;
            }
        }
        public bool Disangaged { get; set; }
        public int RemainingAttacksPerAction { get; set; }
        public bool ActionUsedNotToAttack { get; set; }
        public bool ActionUsedToAttack { get; set; }
        public bool BonusActionUsedNotToAttack { get; set; }
        public bool BonusActionUsedToAttack { get; set; }
        public bool ReactionUsed { get; set; }
        public int CurrentHitPoints { get; set; }
        public int TemporaryHitPoints { get; set; }
        public string LastAttackUsed { get; set; }
        public int RemainingAttacksPerBonusAction { get; set; }
        public List<Tuple<int, int, TemporaryEffects>> TemporaryEffectsList { get; set; } = new List<Tuple<int, int, TemporaryEffects>>();
        public bool DashUsed { get; set; }
        public bool DodgeUsed { get; set; }

        private int RollInitiative()
        {
            RolledInitiative = roller.Roll(InitiativeRollType, 1, 20, InitiativeModifier);
            return RolledInitiative;
        }

        public override bool Equals(object obj)
        {
            if(obj as ICreature == null)
            {
                return false;
            }
            return Id == (obj as ICreature).Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public abstract ICreature Copy();
    }
}

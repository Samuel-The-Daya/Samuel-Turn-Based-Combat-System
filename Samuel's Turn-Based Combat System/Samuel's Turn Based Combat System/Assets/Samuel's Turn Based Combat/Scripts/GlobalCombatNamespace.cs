using System.Collections.Generic;
using System;

namespace SamuelCombat
{
    public enum DamageType
    {
        Basic,
        Physical,
        Magical,
    }

    public enum CharacterType
    {
        Player,
        Enemy
    }

    public enum EnemySignificance
    {
        MobEnemy,
        EliteEnemy,
    }

    public enum Targeting
    {
        Self,
        Allies,
        Enemies,
        RandomAlly,
        RandomEnemy,
        AllAllies,
        AllEnemies,
    }

    public enum Stat
    {
        MaxHealth,
        Initiative,
        BaseAttack,
        PhysicalAttack,
        MagicalAttack,
        BaseDefense,
        PhysicalDefense,
        MagicalDefense,
    }

    public enum Status
    {
        Normal,
        Dead,
        ManWhoCanReflectPhysicalDamage,
    }

    public enum InputSelection
    {
        None,
        Basic,
        Skill1,
        Skill2,
    }

    public enum Operator
    {
        add,
        multiply
    }
    
    public class SortDescending : IComparer<CharacterManager>
    {
        public int Compare(CharacterManager x, CharacterManager y)
        {
            return y.CompareTo(x);
        }
    }

    [Serializable]
    public class StatDictionary : Dictionary<Stat, float>
    {
        public StatDictionary(IDictionary<Stat, float> dictionary) : base(dictionary)
        {
        }

        public float Get(Stat reference)
        {
            if (TryGetValue(reference, out float value))
                return value;
            else
                return 0;
        }
    }
}

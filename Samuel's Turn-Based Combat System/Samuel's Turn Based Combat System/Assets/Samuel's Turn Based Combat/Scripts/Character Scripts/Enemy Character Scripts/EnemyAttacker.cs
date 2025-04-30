using UnityEngine;
using SamuelCombat;

public class EnemyAttacker : StatsTemplate
{
    //basic attack function
    public override void Basic()
    {
        combatSystem.TargetCharacter.GetStats.TakeDamage(stats[Stat.BaseAttack], DamageType.Basic);
    }

    public override void BasicPrep()
    {
        combatSystem.ChangeTargeting(Targeting.RandomAlly);
    }

    //first skill function
    public override void Skill1()
    {
        combatSystem.TargetCharacter.GetStats.TakeDamage(stats[Stat.MagicalAttack], DamageType.Magical);
    }

    public override void Skill1Prep()
    {
        combatSystem.ChangeTargeting(Targeting.RandomAlly);
    }

    //enemies don't use skill 2
    //these functions wont be called but are needed for the template

    public override void Skill2()
    {
        throw new System.NotImplementedException();
    }

    public override void Skill2Prep()
    {
        throw new System.NotImplementedException();
    }
}

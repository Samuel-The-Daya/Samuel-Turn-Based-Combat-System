using UnityEngine;
using SamuelCombat;

public class Defender : StatsTemplate
{
    [SerializeField] StatModifier magicalDefense;
    [SerializeField] StatModifier physicalDefense;
    //basic attack function
    public override void Basic()
    {
        combatSystem.TargetCharacter.GetStats.TakeDamage(stats[Stat.BaseAttack], DamageType.Basic);
    }

    public override void BasicPrep()
    {
        combatSystem.ChangeTargeting(Targeting.Enemies);
    }

    //first skill function
    public override void Skill1()
    {
        combatSystem.TargetCharacter.StatManager.AddStatMod(magicalDefense);
    }

    public override void Skill1Prep()
    {
        combatSystem.ChangeTargeting(Targeting.Allies);
    }

    //second skill function
    public override void Skill2()
    {
        combatSystem.TargetCharacter.StatManager.AddStatMod(physicalDefense);
    }

    public override void Skill2Prep()
    {
        combatSystem.ChangeTargeting(Targeting.Allies);
    }
}

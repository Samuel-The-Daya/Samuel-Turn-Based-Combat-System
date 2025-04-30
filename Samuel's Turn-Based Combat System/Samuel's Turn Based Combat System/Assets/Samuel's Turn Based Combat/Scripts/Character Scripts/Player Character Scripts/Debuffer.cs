using UnityEngine;
using SamuelCombat;

public class Debuffer : StatsTemplate
{
    [SerializeField] StatModifier magicalDebuff;
    [SerializeField] StatModifier physicalDebuff;
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
        combatSystem.TargetCharacter.StatManager.AddStatMod(magicalDebuff);
        combatSystem.TargetCharacter.GetStats.TakeDamage(stats[Stat.MagicalAttack], DamageType.Magical);
    }

    public override void Skill1Prep()
    {
        combatSystem.ChangeTargeting(Targeting.Enemies);
    }

    //second skill function
    public override void Skill2()
    {
        combatSystem.TargetCharacter.StatManager.AddStatMod(physicalDebuff);
        combatSystem.TargetCharacter.GetStats.TakeDamage(stats[Stat.PhysicalAttack], DamageType.Physical);
    }

    public override void Skill2Prep()
    {
        combatSystem.ChangeTargeting(Targeting.Enemies);
    }
}

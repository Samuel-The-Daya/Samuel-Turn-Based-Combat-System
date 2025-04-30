using UnityEngine;
using SamuelCombat;

public class Healer : StatsTemplate
{
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
        combatSystem.TargetCharacter.GetStats.Heal(50);
    }

    public override void Skill1Prep()
    {
        combatSystem.ChangeTargeting(Targeting.Allies);
    }

    //second skill function
    public override void Skill2()
    {
        foreach (CharacterManager character in combatSystem.PlayerCharacters)
        {
            character.GetStats.Heal(25);
        }
    }

    public override void Skill2Prep()
    {
        combatSystem.ChangeTargeting(Targeting.AllAllies);
    }
}

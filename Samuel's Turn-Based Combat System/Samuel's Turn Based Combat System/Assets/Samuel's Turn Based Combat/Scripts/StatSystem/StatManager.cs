using UnityEngine;
using System.Collections.Generic;
using SamuelCombat;
public class StatManager : MonoBehaviour
{
    [SerializeField] private List<StatModifier> mods;
    private CharacterManager thisChar;

    public void AddStatMod(StatModifier statmod)
    {
        //if it doesn't exist add the stat, if it does reset the duration instead
        foreach (StatModifier modifier in mods)
        {
            if (modifier.ModName == statmod.ModName)
                mods[mods.IndexOf(modifier)].ResetDuration = statmod.Duration;
            return;
        }
             mods.Add(new StatModifier(statmod));
             
        //calculate once its been added
        CalculateStatModifiers();
    }

    private void Awake()
    {
        thisChar = GetComponent<CharacterManager>();
    }

    //to calculate stats when it is not on the turn of the character
    //this will not decrease the modifier's duration
    public void CalculateStatModifiers()
    {
        thisChar.GetStats.stats = new StatDictionary(thisChar.GetStats.referenceStats.stats);
        UpdateStats();
    }

    //since it is the turn of the character
    //the duration of the modifiers will decrease
    public void OnTurnCalculateStatModifiers()
    {
        thisChar.GetStats.stats = new StatDictionary(thisChar.GetStats.referenceStats.stats);
        PassTurn();
    }

    private void PassTurn()
    {

        if (mods.Count > 0)
        {
            for (int i = 0; i < mods.Count; i++)
            {
                //removes the modifier if it reaches 0
                if (mods[i].Duration <= 0)
                {
                    mods.RemoveAt(i);
                }
            }
            foreach (StatModifier mod in mods)
            {
                mod.TurnPass();
            }

            UpdateStats();
        }
    }

    private void UpdateStats()
    {
        foreach(StatModifier mod in mods)
            {
                switch (mod.Operator)
                {
                    case Operator.add:
                        switch (mod.TargetStat)
                        {
                            case Stat.Initiative:
                            thisChar.GetStats.initiative += (int)mod.Value;
                            break;
                            default:
                            thisChar.GetStats.stats[mod.TargetStat] += mod.Value;
                            break;
                        }
                    break;
                    case Operator.multiply:
                    switch (mod.TargetStat)
                        {
                            case Stat.Initiative:
                            thisChar.GetStats.initiative += (int)thisChar.GetStats.initiative * (int)mod.Value;
                            break;
                            default:
                            thisChar.GetStats.stats[mod.TargetStat] += thisChar.GetStats.stats[mod.TargetStat] * mod.Value;
                            break;
                        }
                    break;
                }
            }
    }
}

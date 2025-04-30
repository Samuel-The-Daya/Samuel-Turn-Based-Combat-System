using System;
using SamuelCombat;
using UnityEngine;

[Serializable]
public class StatModifier
{
    [SerializeField] private string modName, modEffect;
    [SerializeField] private int value, duration;
    [SerializeField] private Stat targetStat;
    [SerializeField] private Operator op;

    public StatModifier(StatModifier statmod)
    {
        modName = statmod.modName;
        modEffect = statmod.modEffect;
        value = statmod.value;
        duration = statmod.duration;
        targetStat = statmod.targetStat;
        op = statmod.op;
    }

    public void TurnPass()
    {
        duration--;
    }

    public int ResetDuration
    {
        set { duration = value; }
    }

    public string ModName
    {
        get { return modName; }
    }

    public string ModEffect
    {
        get { return modEffect; }
    }

    public int Value
    {
        get { return value; }
    }

    public int Duration
    {
        get { return duration; }
    }

    public Stat TargetStat
    {
        get { return targetStat; }
    }

    public Operator Operator
    {
        get { return op; }
    }
}

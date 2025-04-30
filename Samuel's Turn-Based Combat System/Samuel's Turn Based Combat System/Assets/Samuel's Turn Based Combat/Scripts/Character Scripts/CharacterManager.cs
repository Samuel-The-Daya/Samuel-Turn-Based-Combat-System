using UnityEngine;
using SamuelCombat;

//this script is the central hub for all character scripts and functions
//everything that all the system needs to grab can be accessed here
public class CharacterManager : MonoBehaviour
{
    public int GetInitiative
    {
        get { return GetComponent<StatsTemplate>().initiative; }
    }

    public CharacterType Type
    {
        get { return GetComponent<StatsTemplate>().referenceStats.type; }
    }

    //stats accessor
    public StatsTemplate GetStats
    {
        get { return GetComponent<StatsTemplate>(); }
    }
    public Target Target
    {
        get { return GetComponent<Target>(); }
    }

    public CharacterAnimator CharAnimator
    {
        get { return GetComponent<CharacterAnimator>(); }
    }

    public StatManager StatManager
    {
        get { return GetComponent<StatManager>(); }
    }


    //for the custom sorter in the turn order system
    //these set of functions override the object's hashcode


    public int CompareTo(CharacterManager other)
    {
        if (other == null)
            return 1;
        else
            return this.GetInitiative.CompareTo(other.GetInitiative);
    }

    public override int GetHashCode()
    {
        return this.GetInitiative;
    }

    public bool Equals(CharacterManager other)
    {
        if (other == null) return false;
        return this.GetInitiative.Equals(other.GetInitiative);
    }
}

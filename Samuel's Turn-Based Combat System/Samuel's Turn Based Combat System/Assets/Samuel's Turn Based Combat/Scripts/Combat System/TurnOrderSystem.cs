using UnityEngine;
using SamuelCombat;
using System.Collections.Generic;

public class TurnOrderSystem : MonoBehaviour
{

    [Header("Turn Order")]
    //the turn order
    [SerializeField] private List<CharacterManager> order;

    //public accessor of turn order
    public List<CharacterManager> GetOrder
    {
        get { return order; }
    }

    //the active character
    [SerializeField] private CharacterManager active;

    //active character accessor
    public CharacterManager GetActiveCharacter
    {
        get
        {
            if (order.Count > 0)
                return active = order[curTurn];
            else
                return null;
        }
    }


    [Header("Turn Order Values")]
    //the current turn index
    [SerializeField] private int cT;

    //the public accessor of the turn index with a clamp from the first to the last of the order
    public int curTurn
    {
        get { return cT; }
        set { cT = Mathf.Clamp(value, 0, order.Count - 1); }
    }

    //round counter
    [SerializeField] private int curRound;

    //the current initiative
    public int currentInitiative
    {
        get
        {
            return GetActiveCharacter.GetInitiative;
        }
    }

    //Turn Order Management Functions

    public void AddGroup(List<CharacterManager> list)
    {
        order.AddRange(list);
        SortInitiative();
    }

    public void ClearGroup()
    {
        order.Clear();
    }


    //Turn Management Functions

    private void SortInitiative()
    {
        SortDescending sD = new SortDescending();
        order.Sort(sD);
        GetComponent<TurnBasedCombatSystem>().CombatUI.UpdateDisplayOrder(order);
    }

    public void FirstTurn()
    {
        curTurn = 0;
        curRound = 0;
        SortInitiative();
    }

    public void NextTurn()
    {
        SortInitiative();
        curTurn = order.IndexOf(active);

        if (curTurn >= order.Count - 1)
        {
            NewRound();
            // you may add a return here 
            // if you want something to happen 
            // before you start the new round
        }
        else
        {
            curTurn++;
        }

        GetComponent<TurnBasedCombatSystem>().StartTurn();
    }

    // this function is separated
    // just in case if you want to have special functions
    // happen in between rounds
    private void NewRound()
    {
        curTurn = 0;
        curRound++;

        // insert cool function here if you want
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SamuelCombat;

public class PartyManager : MonoBehaviour
{
    private TurnBasedCombatSystem TurnBasedCombatSystem
    {
        get { return GetComponent<TurnBasedCombatSystem>(); }
    }
    [SerializeField] public Transform playerSide;
    [SerializeField] public Transform enemySide;

    //rearanges the layout groups
    public void RearrangeParty(List<CharacterManager> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].GetStats.status == Status.Dead)
            {
                // plays the death animation and 
                // removes the target from the character lists
                list[i].CharAnimator.PlayDeath();
                list.RemoveAt(i);
            }
        }

        StartCoroutine(WaitForAnimationToFinish());
    }

    private IEnumerator WaitForAnimationToFinish()
    {
        //reinitalize the turn order
        TurnBasedCombatSystem.InitializeTurnOrder();

        //waits for the death animation to finish
        yield return new WaitForSeconds(3);
        TurnBasedCombatSystem.CombatUI.RestartStatDisplay(TurnBasedCombatSystem.PlayerCharacters);
        TurnBasedCombatSystem.CombatUI.UpdateStatDisplay(TurnBasedCombatSystem.PlayerCharacters);


        //checks if either no enemies left or no players left to see who wins
        if (TurnBasedCombatSystem.EnemyCharacters.Count == 0)
            TurnBasedCombatSystem.Victory();
        else if (TurnBasedCombatSystem.PlayerCharacters.Count == 0)
            TurnBasedCombatSystem.Lose();
        else
            TurnBasedCombatSystem.TurnOrderSystem.NextTurn();
    }
}

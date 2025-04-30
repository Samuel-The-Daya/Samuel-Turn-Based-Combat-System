using System.Collections.Generic;
using UnityEngine;
using SamuelCombat;

public class TargetingSystem : MonoBehaviour
{
    private List<CharacterManager> playerCharacters
    {
        get { return GetComponent<TurnBasedCombatSystem>().PlayerCharacters; }
    }
    private List<CharacterManager> enemyCharacters
    {
        get { return GetComponent<TurnBasedCombatSystem>().EnemyCharacters; }
    }


    public CharacterManager targetCharacter;
    public CharacterManager GetTargetCharacter
    {
        get { return targetCharacter; }
    }


    private int i;
    private int index
    {
        get
        {
            //locks index within bounds when it tries to go out
            //depends on targeting side
            switch (targetingType)
            {
                case Targeting.Allies:
                    if (i > playerCharacters.Count - 1)
                        return playerCharacters.Count - 1;
                    else
                        return i;
                case Targeting.Enemies:
                    if (i > enemyCharacters.Count - 1)
                        return i = enemyCharacters.Count - 1;
                    else
                        return i;
                case Targeting.RandomAlly:
                    if (i > playerCharacters.Count - 1)
                        return playerCharacters.Count - 1;
                    else
                        return i;
                case Targeting.RandomEnemy:
                    if (i > enemyCharacters.Count - 1)
                        return i = enemyCharacters.Count - 1;
                    else
                        return i;
            }

            return i;
        }
        set
        {
            //adds clamps to the index based on the targeting type
            //also adds a clamp when theres only one character on either side
            switch (targetingType)
            {
                case Targeting.Allies:
                    i = Mathf.Clamp(value, 0, playerCharacters.Count - 1);
                    break;
                case Targeting.Enemies:
                    i = Mathf.Clamp(value, 0, enemyCharacters.Count - 1);
                    break;
                case Targeting.RandomAlly:
                    i = Mathf.Clamp(value, 0, playerCharacters.Count - 1);
                    break;
                case Targeting.RandomEnemy:
                    i = Mathf.Clamp(value, 0, enemyCharacters.Count - 1);
                    break;
            }
        }
    }

    private Targeting targetingType;

    public void ResetTargetting()
    {
        //deselect all characters
        foreach (CharacterManager characters in GetComponent<TurnBasedCombatSystem>().TurnOrderSystem.GetOrder)
        {
            characters.Target.Deselect();
        }

        //hide all player characters
        foreach (CharacterManager character in playerCharacters)
        {
            character.gameObject.SetActive(false);
        }
    }

    public void UpdateTargetingType(Targeting targeting)
    {
        ResetTargetting();
        GetComponent<TurnBasedCombatSystem>().InputManager.DisableSelection();
        // ResetIndex();
        targetingType = targeting;

        switch (targetingType)
        {
            case Targeting.Self:
                SelfTargeting();
                break;
            case Targeting.Allies:
                AlliesTargeting();
                break;
            case Targeting.Enemies:
                EnemiesTargeting();
                break;
            case Targeting.AllAllies:
                AllAlliesTargeting();
                break;
            case Targeting.AllEnemies:
                AllEnemiesTargeting();
                break;
            case Targeting.RandomAlly:
                RandomAllyTargeting();
                break;
            case Targeting.RandomEnemy:
                RandomEnemyTargeting();
                break;

        }

        //if the camera was called at the bottom of this function
        //the camera might call too early
        //camera is called at the end of each function
    }

    //target only self, the active character
    private void SelfTargeting()
    {
        foreach (CharacterManager character in playerCharacters)
        {
            character.gameObject.SetActive(true);
        }

        targetCharacter = GetComponent<TurnBasedCombatSystem>().ActiveCharacter;
        targetCharacter.Target.Select();

        GetComponent<CameraController>().CallCamera(targetingType);
    }

    //targeting one of the player characters
    private void AlliesTargeting()
    {
        foreach (CharacterManager character in playerCharacters)
        {
            character.gameObject.SetActive(true);
        }
        targetCharacter = playerCharacters[index];
        targetCharacter.Target.Select();

        if (GetComponent<TurnBasedCombatSystem>().ActiveCharacter.Type == CharacterType.Player && playerCharacters.Count > 1)
            GetComponent<TurnBasedCombatSystem>().InputManager.EnableSelection();

        GetComponent<CameraController>().CallCamera(targetingType);
    }

    //targeting the enemy characters
    private void EnemiesTargeting()
    {
        GetComponent<TurnBasedCombatSystem>().ActiveCharacter.gameObject.SetActive(true);

        targetCharacter = enemyCharacters[index];
        targetCharacter.Target.Select();

        if (GetComponent<TurnBasedCombatSystem>().ActiveCharacter.Type == CharacterType.Player && enemyCharacters.Count > 1)
            GetComponent<TurnBasedCombatSystem>().InputManager.EnableSelection();

        GetComponent<CameraController>().CallCamera(targetingType);
    }

    //targeting all player characters
    private void AllAlliesTargeting()
    {
        foreach (CharacterManager character in playerCharacters)
        {
            character.gameObject.SetActive(true);
        }

        foreach (CharacterManager player in playerCharacters)
        {
            player.Target.Select();
        }

        GetComponent<CameraController>().CallCamera(targetingType);
    }

    //targeting all enemy characters
    private void AllEnemiesTargeting()
    {
        foreach (CharacterManager enemy in enemyCharacters)
        {
            enemy.Target.Select();
        }

        GetComponent<CameraController>().CallCamera(targetingType);
    }

    //targeting random player character
    private void RandomAllyTargeting()
    {
        index = Random.Range(0, playerCharacters.Count - 1);
        targetCharacter = playerCharacters[index];
        targetCharacter.gameObject.SetActive(true);
        targetCharacter.Target.Select();

        GetComponent<CameraController>().CallCamera(targetingType);
    }

    //targeting random enemy character
    private void RandomEnemyTargeting()
    {
        index = Random.Range(0, enemyCharacters.Count - 1);
        targetCharacter = enemyCharacters[index];
        targetCharacter.Target.Select();

        GetComponent<CameraController>().CallCamera(targetingType);
    }

    //scrolling through target list either player or enemy
    public void ScrollTargets(int i)
    {
        ResetTargetting();
        index += i;

        switch (targetingType)
        {
            case Targeting.Allies:
                foreach (CharacterManager character in playerCharacters)
                {
                    character.gameObject.SetActive(true);
                }
                targetCharacter.Target.Deselect();
                targetCharacter = playerCharacters[index];
                targetCharacter.Target.Select();
                break;
            case Targeting.Enemies:
                GetComponent<TurnBasedCombatSystem>().ActiveCharacter.gameObject.SetActive(true);
                targetCharacter.Target.Deselect();
                targetCharacter = enemyCharacters[index];
                targetCharacter.Target.Select();
                break;
        }

        GetComponent<CameraController>().CallCamera(targetingType);
    }
}

using UnityEngine;
using SamuelCombat;
using System.Collections.Generic;
using System.Collections;


//the central hub for the systems of the game
//for anything other than ui or player related, this is the public accessor for the game
public class TurnBasedCombatSystem : MonoBehaviour
{
    [Header("PreFabs")]
    [SerializeField] private List<GameObject> players;
    [SerializeField] private List<GameObject> enemies;

    [Header("Characters")]
    [SerializeField] private List<CharacterManager> playerCharacters;
    public List<CharacterManager> PlayerCharacters
    {
        get { return playerCharacters; }
    }

    [SerializeField] private List<CharacterManager> enemyCharacters;
    public List<CharacterManager> EnemyCharacters
    {
        get { return enemyCharacters; }
    }

    //accessor for the active character
    [SerializeField] public CharacterManager ActiveCharacter
    {
        get { return GetComponent<TurnOrderSystem>().GetActiveCharacter; }
    }

    [SerializeField] public CharacterManager TargetCharacter
    {
        get { return GetComponent<TargetingSystem>().GetTargetCharacter; }
    }

    //system scripts

    public InputManager InputManager
    {
        get { return GetComponent<InputManager>(); }
    }
    //the accessor for the turn order system
    public TurnOrderSystem TurnOrderSystem
    {
        get { return GetComponent<TurnOrderSystem>(); }
    }
    public TargetingSystem TargetingSystem
    {
        get { return GetComponent<TargetingSystem>(); }
    }
    public PartyManager PartyManager
    {
        get { return GetComponent<PartyManager>(); }
    }

    public CombatUI CombatUI
    {
        get { return GetComponent<CombatUI>(); }
    }

    //functions

    void Start()
    {
        InitializeGame();
    }

    public void InitializeGame()
    {
        foreach (GameObject player in players)
        {
            //spawn players inside the layout group for players
            playerCharacters.Add(Instantiate(player, PartyManager.playerSide).GetComponent<CharacterManager>());
        }
        foreach (GameObject enemy in enemies)
        {
            //spawn enemies inside the layout group for enemies
            enemyCharacters.Add(Instantiate(enemy, PartyManager.enemySide).GetComponent<CharacterManager>());
        }

        CombatUI.InitializeStatDisplay(PlayerCharacters);

        TurnOrderSystem.AddGroup(playerCharacters);
        TurnOrderSystem.AddGroup(enemyCharacters);

        GetComponent<CameraController>().IntroCam();
        StartCoroutine(Intro());
    }

    //plays the intro animation for the characters
    IEnumerator Intro()
    {
        foreach (CharacterManager character in TurnOrderSystem.GetOrder)
        {
            character.CharAnimator.Intro();
        }

        yield return new WaitForSeconds(4.5f);
        StartGame();
    }

    //starts the game
    private void StartGame()
    {
        TurnOrderSystem.FirstTurn();

        StartTurn();
    }

    public void StartTurn()
    {
        //update UI for the turn order
        CombatUI.UpdateActive(TurnOrderSystem.GetOrder, ActiveCharacter);
        CombatUI.UpdateInitiativeUI(ActiveCharacter.GetInitiative);

        //calculate any stat modifiers
        ActiveCharacter.StatManager.OnTurnCalculateStatModifiers();

        //check if player's turn or enemy's turn
        switch (ActiveCharacter.Type)
        {
            case CharacterType.Player:
                //hide all player characters except the active
                foreach (CharacterManager character in playerCharacters)
                {
                    character.gameObject.SetActive(false);
                }

                ActiveCharacter.gameObject.SetActive(true);

                //at start of turn automatically select Basic Attack
                if (InputManager.SelectedInput == InputSelection.None)
                    InputManager.StartOfTurnInput();
                
                InputManager.EnableAllInputs();
                break;
            case CharacterType.Enemy:
                //enemies have their actions automatically done
                ActiveCharacter.CharAnimator.Enemy();
                break;
        }

        //update UI for the active character
        CombatUI.UIObjectUpdater(ActiveCharacter);
        CombatUI.DisplayTextUpdater(ActiveCharacter, InputManager.SelectedInput);
    }

    //changes the targeting of the attack
    public void ChangeTargeting(Targeting targetingType)
    {
        TargetingSystem.UpdateTargetingType(targetingType);
        CombatUI.DisplayTextUpdater(ActiveCharacter, InputManager.SelectedInput);
    }

    //reinitialize the turn order when a character has died
    public void InitializeTurnOrder()
    {
        TurnOrderSystem.ClearGroup();
        TurnOrderSystem.AddGroup(playerCharacters);
        TurnOrderSystem.AddGroup(enemyCharacters);
    }


    public void EndTurn()
    {
        CombatUI.TurnOffUI();

        //checks if theres any enemies supposed to be dead
        foreach (CharacterManager character in enemyCharacters)
        {
            if (character.GetStats.status == Status.Dead)
            {
                PartyManager.RearrangeParty(enemyCharacters);
                return;
            }
        }

        //checks if theres any players supposed to be dead
        foreach (CharacterManager character in playerCharacters)
        {
            if (character.GetStats.status == Status.Dead)
            {
                PartyManager.RearrangeParty(playerCharacters);
                return;
            }
        }

        //calls the next turn
        TurnOrderSystem.NextTurn();
    }

    //Winning Function
    public void Victory()
    {
        InputManager.DisableAllInputs();

        //bring back the characters
        foreach (CharacterManager character in PlayerCharacters)
        {
            character.gameObject.SetActive(true);
        }
        
        //reset their direction
        foreach (CharacterManager character in TurnOrderSystem.GetOrder)
        {
            character.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }

        StartCoroutine(VictoryAnimation());
    }

    IEnumerator VictoryAnimation()
    {
        foreach (CharacterManager character in PlayerCharacters)
        {
            character.CharAnimator.Win();
        }
        GetComponent<CameraController>().WinCam();
        yield return new WaitForSeconds(3);
        Debug.Log("You Win");
        //Scene Switch
    }

    //Losing Function
    public void Lose()
    {
        InputManager.DisableAllInputs();
        StartCoroutine(LoseAnimation());
    }

    IEnumerator LoseAnimation()
    {
        //uses the same camera style as the win camera
        GetComponent<CameraController>().WinCam();
        yield return new WaitForSeconds(3);
        Debug.Log("You Lose");
        //Scene Switch
    }
}


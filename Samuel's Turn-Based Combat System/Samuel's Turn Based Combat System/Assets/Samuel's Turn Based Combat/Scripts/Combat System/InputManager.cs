using SamuelCombat;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Inputs")]
    private InputSystem input;
    private InputAction basic;
    private InputAction skill1;
    private InputAction skill2;
    private InputAction left;
    private InputAction right;
    private InputAction reset;
    private InputAction leave;

    private InputSelection selectedInput;
    public InputSelection SelectedInput
    {
        get { return selectedInput; }
    }

    // Connect inputs to functions
    private void Awake()
    {
        input = new InputSystem();
        
        basic = input.TurnBased.Basic;
        basic.performed += Basic;

        skill1 = input.TurnBased.Skill1;
        skill1.performed += Skill1;

        skill2 = input.TurnBased.Skill2;
        skill2.performed += Skill2;

        left = input.TurnBased.Left;
        left.performed += Left;

        right = input.TurnBased.Right;
        right.performed += Right;

        reset = input.TurnBased.Reset;
        reset.performed += ResetGame;

        leave = input.Inventory.Escape;
        leave.performed += Escape;

        selectedInput = InputSelection.None;
    }
    public void EnableActions()
    {
        basic.Enable();
        skill1.Enable();
        skill2.Enable();
    }
    public void DisableActions()
    {
        basic.Disable();
        skill1.Disable();
        skill2.Disable();
    }
    public void EnableSelection()
    {
        left.Enable();
        right.Enable();
    }

    public void DisableSelection()
    {
        left.Disable();
        right.Disable();
    }

    public void EnableAllInputs()
    {
        EnableSelection();
        EnableActions();
    }

    public void DisableAllInputs()
    {
        DisableSelection();
        DisableActions();
    }

    void OnEnable()
    {
        EnableAllInputs();
    }

    void OnDisable()
    {
        DisableAllInputs();
    }

    public void StartOfTurnInput()
    {
        selectedInput = InputSelection.Basic;
        GetComponent<TurnBasedCombatSystem>().ActiveCharacter.GetStats.BasicPrep();
    }

    //for each input, it follows this format
    private void Basic(InputAction.CallbackContext context)
    {
        //checks which is the current active input
        switch (selectedInput)
        {

            //if input was selected already then activate
            case InputSelection.Basic:
                DisableAllInputs();
                GetComponent<TurnBasedCombatSystem>().ActiveCharacter.CharAnimator.BasicAnim();
                selectedInput = InputSelection.None;
                GetComponent<TurnBasedCombatSystem>().CombatUI.TurnOffUI();
                break;

            //if input was not selected yet or on another input, select this one
            default:
                selectedInput = InputSelection.Basic;
                GetComponent<TurnBasedCombatSystem>().ActiveCharacter.GetStats.BasicPrep();
                break;
        }
    }

    private void Skill1(InputAction.CallbackContext context)
    {
        switch (selectedInput)
        {
            case InputSelection.Skill1:
                DisableAllInputs();
                GetComponent<TurnBasedCombatSystem>().ActiveCharacter.CharAnimator.Skill1Anim();
                selectedInput = InputSelection.None;
                GetComponent<TurnBasedCombatSystem>().CombatUI.TurnOffUI();
                break;

            default:
                selectedInput = InputSelection.Skill1;
                GetComponent<TurnBasedCombatSystem>().ActiveCharacter.GetStats.Skill1Prep();
                break;
        }
    }

    private void Skill2(InputAction.CallbackContext context)
    {
        switch (selectedInput)
        {
            case InputSelection.Skill2:
                DisableAllInputs();
                GetComponent<TurnBasedCombatSystem>().ActiveCharacter.CharAnimator.Skill2Anim();
                selectedInput = InputSelection.None;
                GetComponent<TurnBasedCombatSystem>().CombatUI.TurnOffUI();
                break;

            default:
                selectedInput = InputSelection.Skill2;
                GetComponent<TurnBasedCombatSystem>().ActiveCharacter.GetStats.Skill2Prep();
                break;
        }
    }

    private void Left(InputAction.CallbackContext context)
    {
        GetComponent<TurnBasedCombatSystem>().TargetingSystem.ScrollTargets(1);
    }

    private void Right(InputAction.CallbackContext context)
    {
        GetComponent<TurnBasedCombatSystem>().TargetingSystem.ScrollTargets(-1);
    }

    private void ResetGame(InputAction.CallbackContext context)
    {
        //reset game
    }
    
    private void Escape(InputAction.CallbackContext context)
    {
        //leave game
    }
}

using UnityEngine;
using Unity.Cinemachine;
using SamuelCombat;

//Camera System is similar to targeting system
public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerSide;
    [SerializeField] private Transform enemySide;
    [SerializeField] private Transform middle;
    [SerializeField] private CinemachineCamera cam;

    public TurnBasedCombatSystem TurnBasedCombatSystem
    {
        get { return GetComponent<TurnBasedCombatSystem>(); }
    }

    public void WinCam()
    {
        cam.Follow = middle.transform;
        cam.LookAt = playerSide.transform;
    }

    public void IntroCam()
    {
        cam.LookAt = enemySide.transform;
        cam.Follow = playerSide.transform;
    }

    //switch camera style to match the targeting style
    public void CallCamera(Targeting targeting)
    {
        //resets rotation of all characters
        foreach (CharacterManager character in TurnBasedCombatSystem.TurnOrderSystem.GetOrder)
        {
            character.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
        
        switch (targeting)
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
                AlliesTargeting();
                break;
            case Targeting.RandomEnemy:
                EnemiesTargeting();
                break;
        }
    }

    //only the target character from an front third person perspective
    private void SelfTargeting()
    {
        cam.LookAt = TurnBasedCombatSystem.ActiveCharacter.transform;
        cam.Follow = middle.transform;
    }

    //all the allies from the front third person perspective
    private void AlliesTargeting()
    {
        switch (TurnBasedCombatSystem.ActiveCharacter.Type)
        {
            case CharacterType.Player:
                cam.LookAt = playerSide.transform;
                cam.Follow = middle.transform;
                break;
            case CharacterType.Enemy:
                cam.LookAt = TurnBasedCombatSystem.ActiveCharacter.Target.CamTarget;
                cam.Follow = TurnBasedCombatSystem.TargetCharacter.transform;

                //rotation to face each other
                TurnBasedCombatSystem.TargetCharacter.transform.LookAt(TurnBasedCombatSystem.ActiveCharacter.transform);
                TurnBasedCombatSystem.ActiveCharacter.transform.LookAt(TurnBasedCombatSystem.TargetCharacter.transform);
                break;
        }
    }

    //all the enemies from the third person perspective of a player
    //if its an enemy selecting, front third person perspective of all enemies
    private void EnemiesTargeting()
    {
        switch (TurnBasedCombatSystem.ActiveCharacter.Type)
        {
            case CharacterType.Player:
                cam.LookAt = TurnBasedCombatSystem.TargetCharacter.Target.CamTarget;
                cam.Follow = TurnBasedCombatSystem.ActiveCharacter.transform;

                //have all enemies face the active character
                foreach (CharacterManager character in TurnBasedCombatSystem.EnemyCharacters)
                {
                    character.transform.LookAt(TurnBasedCombatSystem.ActiveCharacter.transform);
                }
                TurnBasedCombatSystem.ActiveCharacter.transform.LookAt(TurnBasedCombatSystem.TargetCharacter.transform);
                break;
            case CharacterType.Enemy:
                cam.LookAt = TurnBasedCombatSystem.ActiveCharacter.Target.CamTarget;
                cam.Follow = middle.transform;
                break;
        }
    }

    //front third person perspective of allies
    private void AllAlliesTargeting()
    {
        cam.LookAt = playerSide.transform;
        cam.Follow = middle.transform;
    }

    //front third person perspective of enemies
    private void AllEnemiesTargeting()
    {
        cam.LookAt = enemySide.transform;
        cam.Follow = middle.transform;
    }
}

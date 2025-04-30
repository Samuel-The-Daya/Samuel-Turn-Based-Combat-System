using UnityEngine;
using SamuelCombat;
public class CharacterAnimator : MonoBehaviour
{
    private Animator anim;
    private CharacterManager charManager;
    [SerializeField] private TurnBasedCombatSystem combatSystem;

    void Awake()
    {
        anim = GetComponent<Animator>();
        charManager = GetComponent<CharacterManager>();
        combatSystem = GameObject.FindGameObjectWithTag("Manager").GetComponent<TurnBasedCombatSystem>();
    }

    // Update is called once per frame
    public void BasicAnim()
    {
        anim.SetTrigger("Basic");
    }
    public void Skill1Anim()
    {
        anim.SetTrigger("Skill1");
    }
    public void Skill2Anim()
    {
        anim.SetTrigger("Skill2");
    }

    public void ResetTriggers()
    {
        anim.ResetTrigger("Basic");
        anim.ResetTrigger("Skill1");
        anim.ResetTrigger("Skill2");
    }

    //animation event to signify that the animation has ended
    //time to move on to the next turn
    public void EndTurn()
    {
        ResetTriggers();
        combatSystem.EndTurn();
    }

    //calls the functions through animation events

    public void BasicAttackCode()
    {
        charManager.GetStats.Basic();
    }
    public void SkillAttackCode()
    {
        charManager.GetStats.Skill1();
    }
    public void Skill2AttackCode()
    {
        charManager.GetStats.Skill2();
    }

    public void Enemy()
    {
        if (charManager.Type == CharacterType.Enemy)
        {
            GetComponent<EnemyBehaviour>().EnemyAction();
        }
    }

    public void PlayDeath()
    {
        anim.SetTrigger("Die");
    }

    //you may delete the object upon death, but sometimes its fun to keep it there
    public void DeleteObject()
    {
        Destroy(gameObject);
    }

    //plays
    public void Intro()
    {
        anim.SetTrigger("Intro");
    }

    //plays the hip hop dance again
    //can replace with any animation
    public void Win()
    {
        anim.ResetTrigger("Intro");
        anim.SetTrigger("Intro");
    }
}

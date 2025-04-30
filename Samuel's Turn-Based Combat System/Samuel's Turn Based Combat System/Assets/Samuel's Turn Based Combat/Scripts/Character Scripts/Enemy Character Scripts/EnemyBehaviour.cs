using SamuelCombat;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    private EnemySignificance enemySignificance
    {
        get { return GetComponent<StatsTemplate>().referenceStats.lvl; }
    }

    //the cooldown that is set for the skill
    [SerializeField] private int cooldown;
    //the cooldown timer that counts down from the cooldown
    [SerializeField] private int cooldownTimer;
    private void Start()
    {
        ResetCooldowns();
    }

    public void EnemyAction()
    {
        //only elite enemies use a second skill
        if (cooldownTimer <= 0 && enemySignificance == EnemySignificance.EliteEnemy)
        {
            //plays the prep to set up the camera
            GetComponent<CharacterManager>().GetStats.Skill1Prep();
            SkillAttack();
            ResetCooldowns();
        }
        else
        {
            //if not using skill, cooldown counts down
            if (enemySignificance == EnemySignificance.EliteEnemy)
                cooldownTimer--;

            GetComponent<CharacterManager>().GetStats.BasicPrep();
            BasicAttack();
        }
    }

    private void BasicAttack()
    {
        GetComponent<CharacterManager>().CharAnimator.BasicAnim();
    }

    private void SkillAttack()
    {
        GetComponent<CharacterManager>().CharAnimator.Skill1Anim();
    }

    private void ResetCooldowns()
    {
        cooldownTimer = cooldown;
    }
}

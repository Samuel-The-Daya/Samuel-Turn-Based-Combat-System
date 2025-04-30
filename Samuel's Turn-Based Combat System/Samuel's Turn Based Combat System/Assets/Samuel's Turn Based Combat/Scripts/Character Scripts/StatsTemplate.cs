using SamuelCombat;
using UnityEngine;

public abstract class StatsTemplate : MonoBehaviour
{
    [SerializeField] protected TurnBasedCombatSystem combatSystem;

    [Header("Character Stats")]
    [SerializeField] public StatsScriptableObject referenceStats;
    [SerializeField] public StatDictionary stats;
    [SerializeField] public Status status;

    //the health counter of the character
    [SerializeField] private float hp;

    //the public accessor of the health counter with the ability to clamp from 0 to the max health
    public float health
    {
        get { return hp; }
        set { hp = Mathf.Clamp(value, 0, stats.Get(Stat.MaxHealth)); }
    }

    //the public accessor of the initiative 
    public int initiative
    {
        get { return (int)stats.Get(Stat.Initiative); }
        set { initiative = Mathf.Clamp((int)stats.Get(Stat.Initiative), 0, 20); }
    }

    private float defenseCalculationValue;

    void Awake()
    {
        referenceStats.InitializeDictionary();
        stats = new StatDictionary(referenceStats.stats);

        combatSystem = GameObject.FindGameObjectWithTag("Manager").GetComponent<TurnBasedCombatSystem>();
    }

    void Start()
    {
        health = stats.Get(Stat.MaxHealth);

        //update the enemy's health bar, maxing it out
        if (GetComponent<CharacterManager>().Type == CharacterType.Enemy)
            GetComponent<CharacterManager>().Target.UpdateHealthPoints(health, stats.Get(Stat.MaxHealth));

        //update the player's stat display    
        if (GetComponent<CharacterManager>().Type == CharacterType.Player)
            combatSystem.CombatUI.UpdateStatDisplay(combatSystem.PlayerCharacters);
    }

    //required functions characters must have

    public abstract void Basic();

    public abstract void BasicPrep();

    public abstract void Skill1();

    public abstract void Skill1Prep();

    public abstract void Skill2();

    public abstract void Skill2Prep();

    private void CheckDamage(DamageType income)
    {
        switch (income)
        {
            case DamageType.Basic:
                defenseCalculationValue = 0;
                break;
            case DamageType.Physical:
                defenseCalculationValue = stats[Stat.PhysicalDefense];
                break;
            case DamageType.Magical:
                defenseCalculationValue = stats[Stat.MagicalDefense];
                break;

        }
    }

    public void TakeDamage(float damageInflicted, DamageType income)
    {
        //check the damage type before calculating defense
        CheckDamage(income);

        //calculate defense (each 1 value is 0.01 percentage increase)
        health -= damageInflicted * (1 - ((defenseCalculationValue + stats[Stat.BaseDefense]) / 1000));

        //update the enemy's health bar
        if (GetComponent<CharacterManager>().Type == CharacterType.Enemy)
            GetComponent<CharacterManager>().Target.UpdateHealthPoints(health, stats.Get(Stat.MaxHealth));

        //update the player's stat display    
        if (GetComponent<CharacterManager>().Type == CharacterType.Player)
            combatSystem.CombatUI.UpdateStatDisplay(combatSystem.PlayerCharacters);

        if (health <= 0)
            status = Status.Dead;
    }

    public void Heal(float healValue)
    {
        health += healValue;
        
        //update the enemy's health bar
        if (GetComponent<CharacterManager>().Type == CharacterType.Enemy)
            GetComponent<CharacterManager>().Target.UpdateHealthPoints(health, stats.Get(Stat.MaxHealth));

        //update the player's stat display    
        if (GetComponent<CharacterManager>().Type == CharacterType.Player)
            combatSystem.CombatUI.UpdateStatDisplay(combatSystem.PlayerCharacters);
    }
}

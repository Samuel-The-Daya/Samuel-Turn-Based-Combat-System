using SamuelCombat;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StatsScriptableObject", menuName = "Scriptable Objects/StatsTemplate")]
public class StatsScriptableObject : ScriptableObject
{
    public GameObject refChar;
    public string insertnamehere;
    public Sprite icon;
    public CharacterType type;
    public EnemySignificance lvl;

    public string basicName;
    public string basicEffect;
    public string skill1name;
    public string skill1effect;
    public string skill2name;
    public string skill2Effect;

    public StatDictionary stats;
    public List<float> unModularStats;

    //Because unity doesn't serialize generic scripts like dictionaries,
    //this function is to initialize the dictionary 
    //using the values within the list
    public void InitializeDictionary()
    {
        //this if statement is crucial
        //multiple characters may use the same scriptable object reference
        //so all of them on awake may reference this same function
        //this keeps it from overloading the dictionary
        if (stats.Count == 0)
        {
            for (int i = 0; i < unModularStats.Count; i++)
            {
                stats.Add((Stat)i, unModularStats[i]);
            }
        }
    }
}

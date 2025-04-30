using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using SamuelCombat;

//Central Hub of All UI
public class CombatUI : MonoBehaviour
{
    [Header("Initiative Order UI")]
    [SerializeField] private List<GameObject> orderDisplay;
    [SerializeField] private GameObject indicator;
    [SerializeField] private Sprite baseIcon;
    [SerializeField] private TextMeshProUGUI initiativeNumber;

    [Header("Player Stats UI")]
    [SerializeField] private GameObject playerDisplayPreFab;
    [SerializeField] private List<GameObject> playerDisplay;
    [SerializeField] private Transform playerDisplayTransform;

    [Header("Skills UI")]
    [SerializeField] private GameObject UIObjectsParent;
    [SerializeField] private UIObject basic, skill1, skill2;

    [Header("Action Display UI")]
    //[SerializeField] private GameObject damageText;
    [SerializeField] private GameObject actionDisplay;
    [SerializeField] private TextMeshProUGUI actionDisplayText, actionDisplayDescription;

    //simply turn off the ui
    public void TurnOffUI()
    {
        actionDisplay.SetActive(false);
        UIObjectsParent.SetActive(false);
    }

    //updates the UI which displays the buttons a player can select
    //for simplicity only keyboard input and no mouse UI input
    public void UIObjectUpdater(CharacterManager person)
    {
        if (person.GetStats.referenceStats.type == CharacterType.Player)
        { UIObjectsParent.SetActive(true); UpdateUIObject(person.GetStats); }
        else
            UIObjectsParent.SetActive(false);
    }

    private void UpdateUIObject(StatsTemplate person)
    {
        basic.UpdateSkillUI(person.referenceStats.basicName);
        skill1.UpdateSkillUI(person.referenceStats.skill1name);
        skill2.UpdateSkillUI(person.referenceStats.skill2name);
    }

    //updates the display text for the player
    public void DisplayTextUpdater(CharacterManager person, InputSelection input)
    {
        if (person.GetStats.referenceStats.type == CharacterType.Player)
        { actionDisplay.SetActive(true); UpdateDisplayText(person.GetStats, input); }
        else
            actionDisplay.SetActive(false);
    }

    //displays the attack and effect for the selected input the player made
    private void UpdateDisplayText(StatsTemplate person, InputSelection input)
    {
        switch (input)
        {
            case InputSelection.None:
                TurnOffUI();
                break;
            case InputSelection.Basic:
                actionDisplayText.text = "Attack: " + person.referenceStats.basicName;
                actionDisplayDescription.text = "Effect: " + person.referenceStats.basicEffect;
                break;
            case InputSelection.Skill1:
                actionDisplayText.text = "Attack: " + person.referenceStats.skill1name;
                actionDisplayDescription.text = "Effect: " + person.referenceStats.skill1effect;
                break;
            case InputSelection.Skill2:
                actionDisplayText.text = "Attack: " + person.referenceStats.skill2name;
                actionDisplayDescription.text = "Effect: " + person.referenceStats.skill2Effect;
                break;
        }
    }

    //updates the UI for the turn order
    public void UpdateDisplayOrder(List<CharacterManager> order)
    {
        foreach (GameObject obj in orderDisplay)
        {
            obj.GetComponent<Image>().sprite = baseIcon;
        }

        //max set to 8 characters in an order
        for (int i = 0; i < 8; i++)
        {
            if (i < order.Count)
            {
                orderDisplay[i].GetComponent<Image>().sprite = order[i].GetStats.referenceStats.icon;
            }
        }
    }

    public void UpdateInitiativeUI(int initiative)
    {
        initiativeNumber.text = "" + initiative;
    }
    
    //indicate the whos turn it is using the turn indicator
    public void UpdateActive(List<CharacterManager> order, CharacterManager active)
    { if (active != null) indicator.transform.position = orderDisplay[order.IndexOf(active)].transform.position; }

    //initializes the UI for the player's display
    public void InitializeStatDisplay(List<CharacterManager> order)
    {
        foreach (CharacterManager player in order)
        {
            playerDisplay.Add(Instantiate(playerDisplayPreFab, playerDisplayTransform));
        }
    }

    //sets the values for the display
    public void UpdateStatDisplay(List<CharacterManager> order)
    {
        for (int i = 0; i < order.Count; i++)
            playerDisplay[i].GetComponent<PlayerStatDisplay>().SetStuff
            (order[i].GetStats.referenceStats.insertnamehere,
            order[i].GetStats.referenceStats.icon,
            order[i].GetStats.health, order[i].GetStats.stats.Get(Stat.MaxHealth));
    }

    //restarts the display when a player dies
    public void RestartStatDisplay(List<CharacterManager> order)
    {
        for (int i = 0; i < playerDisplay.Count; i++)
        {
            Destroy(playerDisplay[i]);
        }
        playerDisplay.Clear();
        foreach (CharacterManager player in order)
        {
            playerDisplay.Add(Instantiate(playerDisplayPreFab, playerDisplayTransform));
        }
    }
}

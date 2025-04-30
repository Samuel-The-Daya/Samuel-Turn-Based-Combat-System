using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIObject : MonoBehaviour
{
    //simple name changer for the UI
    [SerializeField] private TextMeshProUGUI skillname;
    public void UpdateSkillUI(string name)
    {
        skillname.text = name;
    }
}

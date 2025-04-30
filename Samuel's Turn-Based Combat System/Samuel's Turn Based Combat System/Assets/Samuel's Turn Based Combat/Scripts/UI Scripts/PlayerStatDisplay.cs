using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerStatDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI insertname;
    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI hp;

    public void SetStuff(string name, Sprite icon, float h, float maxh)
    {
        img.sprite = icon;
        insertname.text = name;
        hp.text = "HP " + (int)h + "/" + maxh;
    }
}

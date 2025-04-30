using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public void Select()
    {
        target.SetActive(true);
    }
    public void Deselect()
    {
        target.SetActive(false);
    }
    [SerializeField] private GameObject target;

    //position of game object may not match to center of object
    //create a different camera target
    [SerializeField] private Transform camtarget;
    public Transform CamTarget
    {
        get { return camtarget; }
    }

    [SerializeField] private Slider hpBar;
    [SerializeField] private GameObject hpBarObject;

    public void UpdateHealthPoints(float hp, float maxHp)
    {
        hpBar.maxValue = maxHp;
        hpBar.value = hp;
    }

    public void LookAtPlayer(Transform target)
    {
        hpBarObject.transform.LookAt(target);
    }
}

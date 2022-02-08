using UnityEngine;
using UnityEngine.UI;

public class MonsterHpBar : MonoBehaviour
{
    [SerializeField] private Image back;
    [SerializeField] private Image front;

    private float backValue;
    private float frontValue;

    public float Back
    {
        get => backValue;
        set 
        { 
            back.fillAmount = value;
            backValue = value;
        }
    }

    public float Front
    {
        get => frontValue;
        set
        {
            front.fillAmount = value;
            frontValue = value;
        }
    }
}
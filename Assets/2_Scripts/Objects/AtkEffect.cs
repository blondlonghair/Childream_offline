using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AtkEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer grid;
    [SerializeField] private SpriteRenderer act;
    [SerializeField] private TextMeshPro text;
    
    [Header("Sprite")]
    [SerializeField] private Sprite knife;
    [SerializeField] private Sprite shield;
    [SerializeField] private Sprite randomAtk;
    [SerializeField] private Sprite randomAct;

    public enum Type
    {
        Left,
        Middle,
        Right,
        Random,
        All,
        Shield,
        Buff
    }

    private Type _actType;

    public void Effect(MonsterSkill.Skill skill)
    {
        int damage = 0;
        Type actType = Type.Buff;
        
        switch (skill.id)
        {
            case 1:
                if (skill.attackPos == FootPos.Left)
                    actType = Type.Left;
                else if (skill.attackPos == FootPos.Middle)
                    actType = Type.Middle;
                else if (skill.attackPos == FootPos.Right)
                    actType = Type.Right;

                damage = skill.power;
                break;
            case 2 :
                actType = Type.Random;
                damage = skill.power;
                break;
            case 3 :
                actType = Type.All;
                damage = skill.power;
                break;
            case 4 :
                actType = Type.Shield;
                break;
            default:
                actType = Type.Buff;
                break;
        }
        
        switch (actType)
        {
            case Type.Left :
                grid.gameObject.SetActive(true);
                text.gameObject.SetActive(true);
                act.transform.localPosition = new Vector3(0.75f, 0, 0);
                text.transform.localPosition = new Vector3(0.91f, -0.2f, 0); 
                act.sprite = knife;
                text.text = damage.ToString();
                break;
            case Type.Middle :
                grid.gameObject.SetActive(true);
                text.gameObject.SetActive(true);
                act.transform.localPosition = new Vector3(0, 0, 0);
                text.transform.localPosition = new Vector3(0.16f, -0.2f, 0); 
                act.sprite = knife;
                text.text = damage.ToString();
                break;
            case Type.Right :
                grid.gameObject.SetActive(true);
                text.gameObject.SetActive(true);
                act.transform.localPosition = new Vector3(-0.75f, 0, 0);
                text.transform.localPosition = new Vector3(-0.59f, -0.2f, 0); 
                text.text = damage.ToString();
                break;
            case Type.Random : 
                grid.gameObject.SetActive(false);
                text.gameObject.SetActive(true);
                act.sprite = randomAtk;
                text.text = damage.ToString();
                break;
            case Type.All : 
                grid.gameObject.SetActive(false);
                text.gameObject.SetActive(true);
                act.sprite = knife;
                text.text = damage.ToString();
                break;
            case Type.Shield : 
                grid.gameObject.SetActive(false);
                text.gameObject.SetActive(false);
                act.sprite = shield;
                break;
            case Type.Buff : 
                grid.gameObject.SetActive(false);
                text.gameObject.SetActive(false);
                act.sprite = randomAct;
                break;
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
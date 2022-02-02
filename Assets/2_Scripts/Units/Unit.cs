using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Hp")]
    public int maxHp;
    public int curHp;
    public int armor;
    
    [Header("State")]
    public int strength = 0;
    public int agility = 0;
    public int weakness = 0;
    public int thorn = 0;
}
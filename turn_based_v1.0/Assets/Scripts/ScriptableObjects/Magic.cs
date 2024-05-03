using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Magic", menuName = "ScriptableObjects/Magic", order = 2)]
public class Magic : ScriptableObject
{
    public int id;
    public int damage;
    public int manaCost;
    public string spellName;
    public Sprite sprite;
}

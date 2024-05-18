using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy", order = 1)]
public class NpcObject : ScriptableObject
{
    public int HP;
    public int App;
    public string Name;
    public int Att;
    public Sprite sprite;
    // public GameObject enemyGO;
}
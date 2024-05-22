using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public List<HeroStats> heroStats;
    public void Meow()
    {
        int i;
        for(i = 0; i < heroStats.Count; i++)
        {
            heroStats[i].SaveStats();
        }
        Debug.Log("Meow");
    }
}

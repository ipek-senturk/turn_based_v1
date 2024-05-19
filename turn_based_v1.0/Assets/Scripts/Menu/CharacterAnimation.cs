using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public Transform target; // Kameranýn takip edeceði hedef (karakter)
    public Vector3 offset; // Hedef ile kamera arasýndaki mesafe

    void LateUpdate()
    {
        transform.position = target.position + offset;
    }

}

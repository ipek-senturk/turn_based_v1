using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public Transform target; // Kameran�n takip edece�i hedef (karakter)
    public Vector3 offset; // Hedef ile kamera aras�ndaki mesafe

    void LateUpdate()
    {
        transform.position = target.position + offset;
    }

}

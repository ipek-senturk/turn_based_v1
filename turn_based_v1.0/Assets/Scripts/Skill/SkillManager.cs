using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject EnemyLocation;
    public GameObject HeroLocation;
    public Animator animator;

    void Start()
    {

        

    }

    public IEnumerator Icicle()
    {
        // HeroLocation'un transform deðerlerini kendi transformunuza yazýn
        GetComponent<SpriteRenderer>().enabled = true;
        animator.SetTrigger("Ice1Start");
        transform.position = HeroLocation.transform.position;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = EnemyLocation.transform.position;
        yield return new WaitForSeconds(0.5f);


        float duration = 1.5f; // Hareket süresi (1 saniye)
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Hareket sonunda tam hedef pozisyon ve rotasyona set edilir
        transform.position = endPosition;

        
        animator.SetTrigger("Ice1Hit");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
        GetComponent<SpriteRenderer>().enabled = false;

    }
    public IEnumerator Fireball()
    {
        // HeroLocation'un transform deðerlerini kendi transformunuza yazýn
        animator.SetTrigger("Fire1Start");
        GetComponent<SpriteRenderer>().enabled = true;
        
        transform.position = HeroLocation.transform.position;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = EnemyLocation.transform.position;
        yield return new WaitForSeconds(0.5f);


        float duration = 1.5f; // Hareket süresi (1 saniye)
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Hareket sonunda tam hedef pozisyon ve rotasyona set edilir
        transform.position = endPosition;

        
        animator.SetTrigger("Fire1Hit");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
        GetComponent<SpriteRenderer>().enabled = false;

    }
    public IEnumerator Earthhit()
    {
        // HeroLocation'un transform deðerlerini kendi transformunuza yazýn
        GetComponent<SpriteRenderer>().enabled = true;
        animator.SetTrigger("Earth1Start");
        transform.position = HeroLocation.transform.position;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = EnemyLocation.transform.position;
        yield return new WaitForSeconds(0.5f);


        float duration = 1.5f; // Hareket süresi (1 saniye)
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Hareket sonunda tam hedef pozisyon ve rotasyona set edilir
        transform.position = endPosition;


        animator.SetTrigger("Earth1Hit");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
        GetComponent<SpriteRenderer>().enabled = false;

    }
    public IEnumerator Waterball()
    {
        // HeroLocation'un transform deðerlerini kendi transformunuza yazýn
        GetComponent<SpriteRenderer>().enabled = true;
        animator.SetTrigger("Water1Start");
        transform.position = HeroLocation.transform.position;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = EnemyLocation.transform.position;
        yield return new WaitForSeconds(0.8f);


        float duration = 1.5f; // Hareket süresi (1 saniye)
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Hareket sonunda tam hedef pozisyon ve rotasyona set edilir
        transform.position = endPosition;


        animator.SetTrigger("Water1Hit");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
        GetComponent<SpriteRenderer>().enabled = false;

    }


}

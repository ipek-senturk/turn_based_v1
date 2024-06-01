using System.Collections;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject EnemyLocation;
    public GameObject HeroLocation;
    public Animator animator;

    public IEnumerator Icicle()
    {
        yield return StartCoroutine(ExecuteSkill("Ice1Start", "Ice1Hit", 0.5f));
    }

    public IEnumerator Fireball()
    {
        yield return StartCoroutine(ExecuteSkill("Fire1Start", "Fire1Hit", 0.5f));
    }

    public IEnumerator Earthhit()
    {
        yield return StartCoroutine(ExecuteSkill("Earth1Start", "Earth1Hit", 0.5f));
    }

    public IEnumerator Waterball()
    {
        yield return StartCoroutine(ExecuteSkill("Water1Start", "Water1Hit", 0.8f));
    }

    private IEnumerator ExecuteSkill(string startTrigger, string hitTrigger, float initialWait)
    {
        GetComponent<SpriteRenderer>().enabled = true;
        animator.SetTrigger(startTrigger);
        transform.position = HeroLocation.transform.position;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = EnemyLocation.transform.position;
        yield return new WaitForSeconds(initialWait);

        float duration = 1.5f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
        animator.SetTrigger(hitTrigger);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
        GetComponent<SpriteRenderer>().enabled = false;
    }
}

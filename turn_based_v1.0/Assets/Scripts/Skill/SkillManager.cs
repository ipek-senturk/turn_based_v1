using System.Collections;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject EnemyLocation;
    public GameObject HeroLocation;
    public GameObject Level2;
    public GameObject Level1;

    public IEnumerator Icicle()
    {
        yield return StartCoroutine(ExecuteSkill("Ice1Start", "Ice1Hit", 0.5f));
    }

    public IEnumerator Fireball()
    {
        yield return StartCoroutine(ExecuteSkill("Fire1Start", "Fire1Hit", 0.5f));
    }

    public IEnumerator EarthHit()
    {
        yield return StartCoroutine(ExecuteSkill("Earth1Start", "Earth1Hit", 0.5f));
    }

    public IEnumerator Waterball()
    {
        yield return StartCoroutine(ExecuteSkill("Water1Start", "Water1Hit", 0.8f));
    }

    public IEnumerator IceBlast()
    {
        yield return StartCoroutine(ExecuteSecondSkill("Ice2Start", "Ice2Hit", 1f));
    }

    public IEnumerator FireBlast()
    {
        yield return StartCoroutine(ExecuteSecondSkill("Fire2Start", "Fire2Hit", 1f));
    }

    public IEnumerator EarthBlast()
    {
        yield return StartCoroutine(ExecuteSecondSkill("Earth2Start", "Earth2Hit", 1f));
    }

    public IEnumerator WaterBlast()
    {
        yield return StartCoroutine(ExecuteSecondSkill("Water2Start", "Water2Hit", 1f));
    }

    private IEnumerator ExecuteSkill(string startTrigger, string hitTrigger, float initialWait)
    {
        Level1.GetComponent<SpriteRenderer>().enabled = true;
        Debug.Log("Starting skill with trigger: " + startTrigger);
        Level1.GetComponent<Animator>().SetTrigger(startTrigger);
        Level1.transform.position = HeroLocation.transform.position;
        Debug.Log("Level1 start position: " + Level1.transform.position);
        Vector3 startPosition = Level1.transform.position;
        Vector3 endPosition = EnemyLocation.transform.position;
        Debug.Log("Enemy position: " + endPosition);
        yield return new WaitForSeconds(initialWait);

        float duration = 1.5f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            Level1.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            Debug.Log("Level1 position: " + Level1.transform.position);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Level1.transform.position = endPosition;
        Debug.Log("Hitting with trigger: " + hitTrigger);
        Level1.GetComponent<Animator>().SetTrigger(hitTrigger);
        yield return new WaitForSeconds(Level1.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
        Debug.Log("Skill animation complete for trigger: " + hitTrigger);
        Level1.GetComponent<SpriteRenderer>().enabled = false;
    }

    private IEnumerator ExecuteSecondSkill(string startTrigger, string hitTrigger, float initialWait)
    {
        Level2.GetComponent<SpriteRenderer>().enabled = true;
        Debug.Log("Starting skill with trigger: " + startTrigger);
        Level2.GetComponent<Animator>().SetTrigger(startTrigger);
        Level2.transform.position = EnemyLocation.transform.position;
        Debug.Log("Level2 start position: " + Level2.transform.position);
        Debug.Log("Enemy position: " + EnemyLocation.transform.position);
        yield return new WaitForSeconds(initialWait);

        Level2.transform.position = EnemyLocation.transform.position;
        Debug.Log("Hitting with trigger: " + hitTrigger);
        Level2.GetComponent<Animator>().SetTrigger(hitTrigger);
        yield return new WaitForSeconds(initialWait);
        Debug.Log("Skill animation complete for trigger: " + hitTrigger);
        Level2.GetComponent<SpriteRenderer>().enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fly_randmoving : MonoBehaviour
{

    Vector2 target_pos;
    [Range(10, 100)]
    public float speed_fly;
    care_effect care_effect_script;

    // Start is called before the first frame update
    void Start()
    {
        target_pos = new Vector2(Random.Range(600, 1700), Random.Range(300, 1200));
        care_effect_script = GameObject.Find("care_effect_player").GetComponent<care_effect>();
        StartCoroutine(repeat_fly_Sound(7.3f));
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, target_pos) > 50)
            transform.position = Vector2.MoveTowards(transform.position, target_pos, speed_fly);
        else
            target_pos = new Vector2(Random.Range(600, 1700), Random.Range(300, 1200));

        
    }

    IEnumerator repeat_fly_Sound(float interval)
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            care_effect_script.sound_flies_59723();
            yield return new WaitForSeconds(interval);
        }
    }
}

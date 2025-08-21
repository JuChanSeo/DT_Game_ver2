using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public GameObject notePrefab;
    public RectTransform spawnPoint;
    public RectTransform targetPoint;

    public float minInterval = 3f;
    public float maxInterval = 4f;
    public float noteSpeed = 300f; // 픽셀/초

    [Header("ges_Images")]
    //0:fist, 1:palm, 2:one, 3:dislike, 4:rock, 5:two_up
    public Sprite[] ges_sprites;

    public System.Action<NoteMover> OnNoteSpawned;

    public void Enable_spawn()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnOne();
            float wait = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(wait);
        }
    }

    void SpawnOne()
    {
        var go = Instantiate(notePrefab, spawnPoint.parent); // 같은 Canvas에 생성
        var ges_idx = Random.Range(0 ,6);
        notePrefab.GetComponent<Image>().sprite = ges_sprites[ges_idx];
        var rect = go.GetComponent<RectTransform>();
        rect.anchoredPosition = spawnPoint.anchoredPosition;

        var mover = go.GetComponent<NoteMover>();
        mover.target = targetPoint;
        mover.speed = noteSpeed;

        OnNoteSpawned?.Invoke(mover);
    }
}

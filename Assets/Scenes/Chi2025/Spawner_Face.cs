using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Spawner_Face : MonoBehaviour
{
    public GameObject notePrefab;
    public RectTransform spawnPoint;
    public RectTransform targetPoint;

    public float minInterval = 3f;
    public float maxInterval = 4f;
    public float noteSpeed = 300f;

    [Header("face_Images")]
    //0:normal, 1:happy, 2:surprised, 3:sad, 4:angry
    public Sprite[] face_sprites;

    public System.Action<NoteMoverFace> OnNoteSpawned;

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
        var go = Instantiate(notePrefab, spawnPoint.parent); // ???? Canvas?? ????
        var face_idx = Random.Range(0 ,5);
        notePrefab.GetComponent<Image>().sprite = face_sprites[face_idx];
        var rect = go.GetComponent<RectTransform>();
        rect.anchoredPosition = spawnPoint.anchoredPosition;

        var mover = go.GetComponent<NoteMoverFace>();
        mover.target = targetPoint;
        mover.speed = noteSpeed;

        OnNoteSpawned?.Invoke(mover);
    }
}

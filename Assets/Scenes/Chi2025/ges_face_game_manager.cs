using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;


public class ges_face_game_manager : MonoBehaviour
{
    [Header("Refs")]
    public Spawner_Ges spawner_ges;
    public Spawner_Face spawner_face;


    [Header("Rule")]
    public int requiredTaps = 5;

    [Header("UI")]
    public TextMeshProUGUI statusText_ges;
    public TextMeshProUGUI statusText_face; 
    public TextMeshProUGUI counterText; 


    Queue<NoteMoverFace> _queueFace = new Queue<NoteMoverFace>();
    Queue<NoteMoverGes> _queueGes = new Queue<NoteMoverGes>();
    int _tapCount = 0;
    int _successGes = 0, _failGes = 0;
    int _successFace = 0, _failFace = 0;

    public TextMeshProUGUI ges_text;
    public TextMeshProUGUI face_text;

    void Awake()
    {
        spawner_ges.OnNoteSpawned += OnNoteSpawnedGes;
        spawner_face.OnNoteSpawned += OnNoteSpawnedFace;
        //UpdateTapUI();
        UpdateCounterUI();
        Debug.Log("안녕하세요");
    }

    void OnDestroy()
    {
        if (spawner_ges != null) spawner_ges.OnNoteSpawned -= OnNoteSpawnedGes;
        if (spawner_face != null) spawner_face.OnNoteSpawned -= OnNoteSpawnedFace;
    }

    void Update()
    {
        //if (Input.GetMouseButtonDown(0)) RegisterTap();

        //if (Input.touchCount > 0)
        //    for (int i = 0; i < Input.touchCount; i++)
        //        if (Input.touches[i].phase == TouchPhase.Began)
        //            RegisterTap();
    }

    //void RegisterTap()
    //{
    //    if (_queue.Count == 0) return;
    //    _tapCount++;
    //    UpdateTapUI();
    //}

    void OnNoteSpawnedGes(NoteMoverGes mover)
    {
        mover.OnArrived += OnNoteArrivedGes;
        _queueGes.Enqueue(mover);
    }

    void OnNoteSpawnedFace(NoteMoverFace mover)
    {
        mover.OnArrived += OnNoteArrivedFace;
        _queueFace.Enqueue(mover);
    }

    void OnNoteArrivedGes(NoteMoverGes mover)
    {
        if (_queueGes.Count > 0 && _queueGes.Peek() == mover)
        {
            var cur_ges = mover.GetComponent<Image>().sprite.name.Substring(4);
            if (ges_text.text == cur_ges)
            {
                _successGes++;
                statusText_ges.text = "Gesture 맞추기 성공";
            }
            else
            {
                _failGes++;
                statusText_ges.text = "Gesture 맞추기 실패";
            }
            UpdateCounterUI();
            _queueGes.Dequeue();
            _tapCount = 0;
        }

        mover.OnArrived -= OnNoteArrivedGes;
        if (mover) Destroy(mover.gameObject);
    }

    void OnNoteArrivedFace(NoteMoverFace mover)
    {
        if (_queueFace.Count > 0 && _queueFace.Peek() == mover)
        {
            var cur_face = mover.GetComponent<Image>().sprite.name.Substring(5);
            if (face_text.text == cur_face)
            {
                _successFace++;
                statusText_face.text = "Face 맞추기 성공";
            }
            else
            {
                _failFace++;
                statusText_face.text = "Face 맞추기 실패";
            }
            UpdateCounterUI();
            _queueFace.Dequeue();
            _tapCount = 0;                 
        }

        mover.OnArrived -= OnNoteArrivedFace;
        if (mover) Destroy(mover.gameObject);
    }

    //void Success(string msg)
    //{
    //    SetStatus(msg);
    //    UpdateCounterUI();
    //}

    //void Fail(string msg)
    //{
    //    SetStatus(msg);
    //    UpdateCounterUI();
    //}

    //void UpdateTapUI()
    //{
    //    if (tapText) tapText.text = $"TAPS: {_tapCount}/{requiredTaps}";
    //}

    void UpdateCounterUI()
    {
        if (counterText) counterText.text = $"Ges:{_successGes}, {_failGes}\t Face:{_successFace}, {_failFace}";
    }

}

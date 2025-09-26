using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
// TextMeshPro 쓰면 주석 해제 후, 인스펙터 연결
// using TMPro;

public class ges_face_game_manager : MonoBehaviour
{
    [Header("Refs")]
    public Spawner_ges spawner_ges;
    public Spawner_face spawner_face;


    [Header("Rule")]
    public int requiredTaps = 5;

    [Header("UI (옵션)")]
    public TextMeshProUGUI tapText;         // 예: "TAPS: 0/5"
    public TextMeshProUGUI statusText;      // 예: 상태 표시
    public TextMeshProUGUI counterText;     // 예: "Success: 0 / Fail: 0"
    // public TMP_Text tapText, statusText, counterText; // TMP 쓰면 위 Text 대신 이걸로

    Queue<NoteMover> _queueGes = new Queue<NoteMover>();
    Queue<NoteMover> _queueFace = new Queue<NoteMover>();
    int _tapCount = 0;
    int _success = 0, _fail = 0;

    public TextMeshProUGUI ges_text;
    public TextMeshProUGUI face_text;

    void Awake()
    {
        spawner_ges.OnNoteSpawned += OnNoteSpawnedGes;
        spawner_face.OnNoteSpawned += OnNoteSpawnedFace;
        //UpdateTapUI();
        UpdateCounterUI();
        SetStatus("READY");
    }

    void OnDestroy()
    {
        if (spawner_ges != null) spawner_ges.OnNoteSpawned -= OnNoteSpawnedGes;
        if (spawner_face != null) spawner_face.OnNoteSpawned-= OnNoteSpawnedFace;
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
    //    if (_queue.Count == 0) return; // 대기 노트 없으면 무시
    //    _tapCount++;
    //    UpdateTapUI();
    //}

    void OnNoteSpawnedGes(NoteMover mover)
    {
        mover.OnArrived += OnNoteArrivedGes;
        _queueGes.Enqueue(mover);
        //SetStatus($"NOTE INCOMING... (queue: {_queue.Count})");
        // ★ 스폰 시점엔 절대 리셋하지 않음
    }

    void OnNoteSpawnedFace(NoteMover mover)
    {
        mover.OnArrived += OnNoteArrivedGes;
        _queueFace.Enqueue(mover);
        //SetStatus($"NOTE INCOMING... (queue: {_queue.Count})");
        // ★ 스폰 시점엔 절대 리셋하지 않음
    }


    void OnNoteArrivedGes(NoteMover mover)
    {
        // 큐 맨 앞 노트가 도착했을 때만 판정
        if (_queueGes.Count > 0 && _queueGes.Peek() == mover)
        {
            //Debug.Log(mover.GetComponent<Image>().sprite.name.Substring(4));
            var cur_ges = mover.GetComponent<Image>().sprite.name.Substring(4);
            if (ges_text.text == cur_ges)
            {
                Success("손동작 맞추기 성공!");
            }
            else Fail("손동작 맞추기 실패!");

            _queueGes.Dequeue();
            _tapCount = 0;                 // ★ 도착 판정 후에만 리셋
            //UpdateTapUI();
        }

        mover.OnArrived -= OnNoteArrivedGes;
        Debug.Log("mover 지웁니다");
        if (mover) Destroy(mover.gameObject);
    }

    void OnNoteArrivedFace(NoteMover mover)
    {
        // 큐 맨 앞 노트가 도착했을 때만 판정
        if (_queueFace.Count > 0 && _queueFace.Peek() == mover)
        {
            //Debug.Log(mover.GetComponent<Image>().sprite.name.Substring(4));
            var cur_face = mover.GetComponent<Image>().sprite.name.Substring(5);
            if (face_text.text == cur_face)
            {
                Success("표정 맞추기 성공!");
            }
            else Fail("표정 맞추기 실패!");

            _queueFace.Dequeue();
            _tapCount = 0;                 // ★ 도착 판정 후에만 리셋
            //UpdateTapUI();
        }

        mover.OnArrived -= OnNoteArrivedFace;
        Debug.Log("mover 지웁니다");
        if (mover) Destroy(mover.gameObject);
    }

    void Success(string msg)
    {
        _success++;
        SetStatus(msg);
        UpdateCounterUI();
    }

    void Fail(string msg)
    {
        _fail++;
        SetStatus(msg);
        UpdateCounterUI();
    }

    void UpdateTapUI()
    {
        if (tapText) tapText.text = $"TAPS: {_tapCount}/{requiredTaps}";
    }

    void UpdateCounterUI()
    {
        if (counterText) counterText.text = $"Success: {_success} / Fail: {_fail}";
    }

    void SetStatus(string msg)
    {
        if (statusText) statusText.text = msg;
        Debug.Log(msg);
    }
}

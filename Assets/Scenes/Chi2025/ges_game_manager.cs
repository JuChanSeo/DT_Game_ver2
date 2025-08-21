using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
// TextMeshPro 쓰면 주석 해제 후, 인스펙터 연결
// using TMPro;

public class ges_game_manager : MonoBehaviour
{
    [Header("Refs")]
    public Spawner spawner;


    [Header("Rule")]
    public int requiredTaps = 5;

    [Header("UI (옵션)")]
    public TextMeshProUGUI tapText;         // 예: "TAPS: 0/5"
    public TextMeshProUGUI statusText;      // 예: 상태 표시
    public TextMeshProUGUI counterText;     // 예: "Success: 0 / Fail: 0"
    // public TMP_Text tapText, statusText, counterText; // TMP 쓰면 위 Text 대신 이걸로

    Queue<NoteMover> _queue = new Queue<NoteMover>();
    int _tapCount = 0;
    int _success = 0, _fail = 0;

    public TextMeshProUGUI ges_text;

    void Awake()
    {
        spawner.OnNoteSpawned += OnNoteSpawned;
        UpdateTapUI();
        UpdateCounterUI();
        SetStatus("READY");
    }

    void OnDestroy()
    {
        if (spawner != null) spawner.OnNoteSpawned -= OnNoteSpawned;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) RegisterTap();

        if (Input.touchCount > 0)
            for (int i = 0; i < Input.touchCount; i++)
                if (Input.touches[i].phase == TouchPhase.Began)
                    RegisterTap();
    }

    void RegisterTap()
    {
        if (_queue.Count == 0) return; // 대기 노트 없으면 무시
        _tapCount++;
        UpdateTapUI();
    }

    void OnNoteSpawned(NoteMover mover)
    {
        mover.OnArrived += OnNoteArrived;
        _queue.Enqueue(mover);
        //SetStatus($"NOTE INCOMING... (queue: {_queue.Count})");
        // ★ 스폰 시점엔 절대 리셋하지 않음
    }

    void OnNoteArrived(NoteMover mover)
    {
        // 큐 맨 앞 노트가 도착했을 때만 판정
        if (_queue.Count > 0 && _queue.Peek() == mover)
        {
            //Debug.Log(mover.GetComponent<Image>().sprite.name.Substring(4));
            var cur_ges = mover.GetComponent<Image>().sprite.name.Substring(4);
            if (ges_text.text == cur_ges)
            {
                Success("손동작 맞추기 성공!");
            }
            else Fail("손동작 맞추기 실패!");

            _queue.Dequeue();
            _tapCount = 0;                 // ★ 도착 판정 후에만 리셋
            //UpdateTapUI();
        }

        mover.OnArrived -= OnNoteArrived;
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

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
// TextMeshPro ���� �ּ� ���� ��, �ν����� ����
// using TMPro;

public class ges_game_manager : MonoBehaviour
{
    [Header("Refs")]
    public Spawner spawner;


    [Header("Rule")]
    public int requiredTaps = 5;

    [Header("UI (�ɼ�)")]
    public TextMeshProUGUI tapText;         // ��: "TAPS: 0/5"
    public TextMeshProUGUI statusText;      // ��: ���� ǥ��
    public TextMeshProUGUI counterText;     // ��: "Success: 0 / Fail: 0"
    // public TMP_Text tapText, statusText, counterText; // TMP ���� �� Text ��� �̰ɷ�

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
        if (_queue.Count == 0) return; // ��� ��Ʈ ������ ����
        _tapCount++;
        UpdateTapUI();
    }

    void OnNoteSpawned(NoteMover mover)
    {
        mover.OnArrived += OnNoteArrived;
        _queue.Enqueue(mover);
        //SetStatus($"NOTE INCOMING... (queue: {_queue.Count})");
        // �� ���� ������ ���� �������� ����
    }

    void OnNoteArrived(NoteMover mover)
    {
        // ť �� �� ��Ʈ�� �������� ���� ����
        if (_queue.Count > 0 && _queue.Peek() == mover)
        {
            //Debug.Log(mover.GetComponent<Image>().sprite.name.Substring(4));
            var cur_ges = mover.GetComponent<Image>().sprite.name.Substring(4);
            if (ges_text.text == cur_ges)
            {
                Success("�յ��� ���߱� ����!");
            }
            else Fail("�յ��� ���߱� ����!");

            _queue.Dequeue();
            _tapCount = 0;                 // �� ���� ���� �Ŀ��� ����
            //UpdateTapUI();
        }

        mover.OnArrived -= OnNoteArrived;
        Debug.Log("mover ����ϴ�");
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

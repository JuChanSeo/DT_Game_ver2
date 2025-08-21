using UnityEngine;

public class NoteMover : MonoBehaviour
{
    public RectTransform target;  // UI 타겟
    public float speed = 300f;    // UI에서는 초당 픽셀 단위로 속도 설정

    public System.Action<NoteMover> OnArrived;

    private bool _arrived;
    private RectTransform _rect;

    void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (_arrived || target == null) return;

        // UI 이동 (anchoredPosition 기준)
        Vector2 dir = (target.anchoredPosition - _rect.anchoredPosition).normalized;
        _rect.anchoredPosition += dir * speed * Time.deltaTime;

        // 타겟 도착 판정 (거리 기준)
        if (Vector2.Distance(_rect.anchoredPosition, target.anchoredPosition) <= 1f)
        {
            _arrived = true;
            OnArrived?.Invoke(this);
        }
    }
}

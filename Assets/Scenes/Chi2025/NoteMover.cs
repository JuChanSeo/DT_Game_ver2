using UnityEngine;

public class NoteMover : MonoBehaviour
{
    public RectTransform target;  // UI Ÿ��
    public float speed = 300f;    // UI������ �ʴ� �ȼ� ������ �ӵ� ����

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

        // UI �̵� (anchoredPosition ����)
        Vector2 dir = (target.anchoredPosition - _rect.anchoredPosition).normalized;
        _rect.anchoredPosition += dir * speed * Time.deltaTime;

        // Ÿ�� ���� ���� (�Ÿ� ����)
        if (Vector2.Distance(_rect.anchoredPosition, target.anchoredPosition) <= 1f)
        {
            _arrived = true;
            OnArrived?.Invoke(this);
        }
    }
}

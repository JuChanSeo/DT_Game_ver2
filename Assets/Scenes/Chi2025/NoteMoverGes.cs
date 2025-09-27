using UnityEngine;

public class NoteMoverGes : MonoBehaviour
{
    public RectTransform target;
    public float speed = 300f;

    public System.Action<NoteMoverGes> OnArrived;

    private bool _arrived;
    private RectTransform _rect;

    void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (_arrived || target == null) return;

        Vector2 dir = (target.anchoredPosition - _rect.anchoredPosition).normalized;
        _rect.anchoredPosition += dir * speed * Time.deltaTime;

        if (Vector2.Distance(_rect.anchoredPosition, target.anchoredPosition) <= 1f)
        {
            _arrived = true;
            OnArrived?.Invoke(this);
        }
    }
}

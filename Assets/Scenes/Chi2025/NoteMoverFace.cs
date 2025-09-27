using UnityEngine;

public class NoteMoverFace : MonoBehaviour
{
    public RectTransform target;  // UI ????
    public float speed = 300f;    // UI?????? ???? ???? ?????? ???? ????

    public System.Action<NoteMoverFace> OnArrived;

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

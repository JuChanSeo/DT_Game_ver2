using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class vectorposition : MonoBehaviour
{
    public Image targetImage; // Canvas ���� Image

    void Start()
    {
        if (targetImage != null)
        {
            // Image�� RectTransform ��������
            RectTransform rectTransform = targetImage.GetComponent<RectTransform>();

            // ���� World Position ��
            Vector3 worldPosition = rectTransform.position;
            Debug.Log("World Position: " + worldPosition);

            // Render Mode�� ���� Screen Position Ȯ��
            Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, rectTransform.position);
            Debug.Log("Screen Position: " + screenPosition);
        }
        else
        {
            Debug.LogError("Target Image�� �Ҵ���� �ʾҽ��ϴ�!");
        }
    }
}

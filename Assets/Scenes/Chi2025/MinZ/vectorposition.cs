using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class vectorposition : MonoBehaviour
{
    public Image targetImage; // Canvas 위의 Image

    void Start()
    {
        if (targetImage != null)
        {
            // Image의 RectTransform 가져오기
            RectTransform rectTransform = targetImage.GetComponent<RectTransform>();

            // 현재 World Position 값
            Vector3 worldPosition = rectTransform.position;
            Debug.Log("World Position: " + worldPosition);

            // Render Mode에 따른 Screen Position 확인
            Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, rectTransform.position);
            Debug.Log("Screen Position: " + screenPosition);
        }
        else
        {
            Debug.LogError("Target Image가 할당되지 않았습니다!");
        }
    }
}

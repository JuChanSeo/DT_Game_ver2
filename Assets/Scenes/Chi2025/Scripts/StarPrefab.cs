using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPrefab : MonoBehaviour
{
    UnityEngine.UI.Image img;
    GameObject canvas;
    Vector3 direction;
    public float moveSpeed;
    public float minSize;
    public float maxSize;
    public float sizeSpeed;
    public float color_speed;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        //transform.parent = canvas.transform;
        transform.SetParent(canvas.transform);

        img = GetComponent<UnityEngine.UI.Image>();
        direction = new Vector3(Random.Range(-1.0f, 1.0f),
                                Random.Range(-1.0f, 1.0f),
                                Random.Range(-1.0f, 1.0f));
        float size = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(size, size, size);

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * moveSpeed);
        transform.localScale = Vector3.Lerp(transform.localScale,
                                            Vector3.zero,
                                            Time.deltaTime * sizeSpeed);
        Color color = img.color;
        color.a = Mathf.Lerp(img.color.a, 0, Time.deltaTime * color_speed);
        img.color = color;

        if(img.color.a < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}

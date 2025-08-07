using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawing_pattern : MonoBehaviour
{
    float time;
    Vector2 pos1 = new Vector2(Screen.width / 2 - 347f, Screen.height / 2 + 347f);
    Vector2 pos2 = new Vector2(Screen.width / 2, Screen.height / 2 + 347f);
    Vector2 pos3 = new Vector2(Screen.width / 2 + 347f, Screen.height / 2 + 347f);

    Vector2 pos4 = new Vector2(Screen.width / 2 - 347f, Screen.height / 2);
    Vector2 pos5 = new Vector2(Screen.width / 2, Screen.height / 2);
    Vector2 pos6 = new Vector2(Screen.width / 2 + 347f, Screen.height / 2);

    Vector2 pos7 = new Vector2(Screen.width / 2 - 347f, Screen.height / 2 - 347f);
    Vector2 pos8 = new Vector2(Screen.width / 2, Screen.height / 2 - 347f);
    Vector2 pos9 = new Vector2(Screen.width / 2 + 347f, Screen.height / 2 - 347f);
    Vector3 next_pos;
    string debug_text;
    List<List<Vector2>> pos_seq;
    List<Vector2> cur_seq;

    public GameObject drawing_cursor;
    // Start is called before the first frame update
    void Start()
    {
        drawing_cursor.SetActive(false);
        pos_seq = new List<List<Vector2>> {
            new List<Vector2> {pos1, pos2, pos5, pos7},//0
            new List<Vector2> {pos2, pos5, pos8, pos7},//1
            new List<Vector2> {pos2, pos5, pos4, pos7},//2
            new List<Vector2> {pos3, pos5, pos4, pos7},//3
            new List<Vector2> {pos1, pos4, pos5, pos9},//4
            new List<Vector2> {pos3, pos2, pos5, pos8},//5
            new List<Vector2> {pos1, pos4, pos8, pos9},//6
            new List<Vector2> {pos7, pos4, pos2, pos3},//7
            new List<Vector2> {pos2, pos4, pos8, pos6},//8
            new List<Vector2> {pos2, pos4, pos8, pos9},//9
        };
        cur_seq = pos_seq[0];
        drawing_cursor.transform.GetChild(0).position = cur_seq[0];
        next_pos = cur_seq[1];

    }

    // Update is called once per frame
    void Update()
    {
        drawing_cursor.transform.GetChild(0).position = Vector2.MoveTowards(drawing_cursor.transform.GetChild(0).position, next_pos, 10f);
        if (Vector2.Distance(drawing_cursor.transform.GetChild(0).position, cur_seq[1]) < 1f)
        {
            next_pos = cur_seq[2];
        }
        if (Vector2.Distance(drawing_cursor.transform.GetChild(0).position, cur_seq[2]) < 1f)
        {
            next_pos = cur_seq[3];
        }
        if (Vector2.Distance(drawing_cursor.transform.GetChild(0).position, cur_seq[3]) < 1f)
        {
            drawing_cursor.transform.GetChild(0).position = cur_seq[0];
            next_pos = cur_seq[1];
        }

    }

    public void alloc_pattern(int rand_idx)
    {
        drawing_cursor.SetActive(true);
        cur_seq = pos_seq[rand_idx];
        drawing_cursor.transform.GetChild(0).position = cur_seq[0];
        next_pos = cur_seq[1];
    }
}

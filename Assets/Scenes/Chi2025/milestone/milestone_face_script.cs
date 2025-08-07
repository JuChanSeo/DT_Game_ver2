using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.IO;

[Serializable]
public class SaveData_f
{
    public string name;
    public string Type = "Face";
    public List<int> cnt_ans = new List<int>();
    public List<string> ground_truth = new List<string>();
    public List<string> answer = new List<string>();
}

public class milestone_face_script : MonoBehaviour
{
    face_emo_edit_milestone face_emo_edit_milestone_script;

    GameObject Pet;
    Animator anim;
    SkinnedMeshRenderer face_renderer;

    private float time_remain;
    private bool start_flag;

    public Slider slider_time;
    public TextMeshProUGUI text_emo;

    float time_limit;
    List<string> emo_list = new List<string>();
    public List<Texture2D> emo_picture_list = new List<Texture2D>();
    public RawImage emo_picture_panel;


    int cur_ans_idx;
    int cnt_answer;

    public List<int> cnt_ans__ = new List<int>();
    public List<string> ground_truth__ = new List<string>();
    public List<string> answer__ = new List<string>();

    public TextMeshProUGUI face_instruct_text;
    public TextMeshProUGUI time_remain_text;
    public TextMeshProUGUI cnt_ans_text;
    public TextMeshProUGUI Pnum_text;

    bool true_or_false;

    string cur_face;

    // Start is called before the first frame update
    void Start()
    {

        emo_list.Add("neutral");
        emo_list.Add("happy");
        emo_list.Add("surprised");
        emo_list.Add("angry");
        emo_list.Add("sad");

        Pnum_text.text = "P" + PlayerPrefs.GetInt("milestone_pnum").ToString();
        time_limit = PlayerPrefs.GetFloat("milestone_time_lim");
        face_emo_edit_milestone_script = GameObject.Find("facialexpression").GetComponent<face_emo_edit_milestone>();
        time_remain_text.gameObject.SetActive(false);
        face_emo_edit_milestone_script.excute_emo_model = true;
    }

    // Update is called once per frame
    void Update()
    {
        slider_time.value = time_remain / time_limit;
        time_remain_text.text = $"{(int)time_remain + (int)1}초 안에 밑의 표정을 따라해주세요";

        if (start_flag && time_remain > 0)
            time_remain -= Time.deltaTime;
        else if (start_flag)
        {
            cnt_answer++;
            cnt_ans__.Add(cnt_answer);
            cnt_ans_text.text = "갯수: " + cnt_answer.ToString();

            start_flag = false;
            time_remain = 0;
            if (true_or_false)
            {
                Debug.Log("성공!");
                answer__.Add(emo_list[cur_ans_idx]);
            }
            else
            {
                Debug.Log("실패!");
                answer__.Add(cur_face);
            }

            //if (cnt_answer == 5)
            //{
            //    JsonSave();
            //}

            time_remain_text.gameObject.SetActive(false);
            Invoke("game_start_button_click", 3f);
        }

        cur_face = text_emo.text;
        if (text_emo.text == emo_list[cur_ans_idx])
        {
            true_or_false = true;
        }
        
    }


    public void game_start_button_click()
    {
        start_flag = true;
        cur_ans_idx = UnityEngine.Random.Range(0, 5);
        face_instruct_text.text = emo_list[cur_ans_idx];
        emo_picture_panel.texture = emo_picture_list[cur_ans_idx];
        ground_truth__.Add(face_instruct_text.text);
        time_remain = time_limit;
        time_remain_text.gameObject.SetActive(true);
        true_or_false = false;
        if (GameObject.Find("Button_face") != null) GameObject.Find("Button_face").SetActive(false);
    }

    public void JsonSave()
    {
        SaveData_f saveData = new SaveData_f();

        saveData.name = Pnum_text.text;

        for (int i = 0; i < cnt_ans__.Count; i++)
        {
            saveData.cnt_ans.Add(cnt_ans__[i]);
            saveData.ground_truth.Add(ground_truth__[i]);
            saveData.answer.Add(answer__[i]);
        }

        string json = JsonUtility.ToJson(saveData, true);
        string path = saveData.name;
        string fileN = saveData.name + "_" + saveData.Type + ".json";

        if (!new DirectoryInfo(path).Exists) new DirectoryInfo(path).Create();

        if (!File.Exists(Path.Combine(path, fileN))) File.WriteAllText(Path.Combine(path, fileN), json);
        else
        {
            string newN = FileUploadName(path, fileN);
            File.WriteAllText(Path.Combine(path, newN), json);
        }
    }

    public string FileUploadName(string dirPath, string fileN)
    {
        string fileName = fileN;

        if (fileN.Length > 0)
        {
            int indexOfDot = fileName.LastIndexOf(".");
            string strName = fileName.Substring(0, indexOfDot);
            string strExt = fileName.Substring(indexOfDot);

            bool bExist = true;
            int fileCount = 0;

            string dirMapPath = string.Empty;

            while (bExist)
            {
                dirMapPath = dirPath;
                string pathCombine = System.IO.Path.Combine(dirMapPath, fileName);

                if (System.IO.File.Exists(pathCombine))
                {
                    fileCount++;
                    fileName = strName + "(" + fileCount + ")" + strExt;
                }
                else
                {
                    bExist = false;
                }
            }
        }

        return fileName;

    }

    private void OnApplicationQuit()
    {
        JsonSave();
    }

    public void back_bt_clicked()
    {
        JsonSave();
        SceneManager.LoadScene("milestone_main");
    }

    public void setemotion_nodefault(string emo)
    {
        Debug.Log(emo);
        if (emo == "neutral")
        {
            for (int i = 0; i < 7; i++)
            {
                face_renderer.SetBlendShapeWeight(i, 0);
            }
            return;
        }

        //0:blink, 1:bark, 2:smile, 3:angry, 4:sad, 5:happy, 6:surprise
        int emo_label;
        emo_label = 0;
        if (emo == "blink") emo_label = 0;
        else if (emo == "bark") emo_label = 1;
        else if (emo == "smile") emo_label = 2;
        else if (emo == "angry") emo_label = 3;
        else if (emo == "sad") emo_label = 4;
        else if (emo == "happy") emo_label = 5;
        else if (emo == "surprised") emo_label = 6;

        for (int i = 0; i < 7; i++)
        {
            if (i == emo_label)
            {
                face_renderer.SetBlendShapeWeight(i, 100);
            }
            else
            {
                face_renderer.SetBlendShapeWeight(i, 0);
            }
        }

    }

    public void setemotion(string emo)
    {
        //0:blink, 1:bark, 2:smile, 3:angry, 4:sad, 5:happy, 6:surprise
        int emo_label;
        if (emo == "blink") emo_label = 0;
        else if (emo == "bark") emo_label = 1;
        else if (emo == "smile") emo_label = 2;
        else if (emo == "angry") emo_label = 3;
        else if (emo == "sad") emo_label = 4;
        else if (emo == "happy") emo_label = 5;
        else emo_label = 6;

        face_renderer.SetBlendShapeWeight(emo_label, 100);
        Invoke("setemotion_default", 2f);
    }

    void setemotion_default()
    {
        for (int i = 0; i < 7; i++)
        {
            face_renderer.SetBlendShapeWeight(i, 0);
        }

    }

    public static int[] MakeRandomNumbers(int maxValue, int randomSeed = 0)
    {
        return MakeRandomNumbers(0, maxValue, randomSeed);
    }
    public static int[] MakeRandomNumbers(int minValue, int maxValue, int randomSeed = 0)
    {
        if (randomSeed == 0)
            randomSeed = (int)System.DateTime.Now.Ticks;

        List<int> values = new List<int>();
        for (int v = minValue; v < maxValue; v++)
        {
            values.Add(v);
        }

        int[] result = new int[maxValue - minValue];
        System.Random random = new System.Random(Seed: randomSeed);
        int i = 0;
        while (values.Count > 0)
        {
            int randomValue = values[random.Next(0, values.Count)];
            result[i++] = randomValue;

            if (!values.Remove(randomValue))
            {
                // Exception
                break;
            }
        }

        return result;
    }
}

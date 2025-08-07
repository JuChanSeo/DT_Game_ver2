using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;	// UnityWebRequest사용을 위해서 적어준다.
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

[Serializable]
public class SaveData_v
{
    public string name;
    public string Type = "Voice";
    public List<int> cnt_ans = new List<int>();
    public List<string> ground_truth = new List<string>();
    public List<string> answer = new List<string>();
}

public class speechRecog : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //Petctrl petctrl_script;

    public List<int> cnt_ans__ = new List<int>();
    public List<string> ground_truth__ = new List<string>();
    public List<string> answer__ = new List<string>();

    public TMP_Text res_text;
    public TextMeshProUGUI speak_txt;
    bool _pressed = false;

    /// <summary>
    /// 의도인식 실험을 위한 추가부분
    /// </summary>
    public UnityEngine.UI.Slider slider_time;
    public TextMeshProUGUI time_remain_text;
    public TextMeshProUGUI cnt_answer_text;
    public TextMeshProUGUI Pnum_text;
    float time_remain;
    float time_limit;
    int cnt_answer;

    List<string> list_instruct = new List<string>()
    { "강아지를 불러주세요",
      "강아지가 한바퀴 돌게 해주세요",
      "강아지가 제자리에서 점프하게 해주세요",
      "강아지가 눕게 해주세요",
      "강아지가 애교부리게 해주세요",
      "강아지가 제자리에 앉게 해주세요",
      "강아지가 짖어보게 해주세요",
      "강아지가 몸을 긁게 해주세요",
      "강아지가 손을 내밀게 해주세요",
      "강아지가 엎드리게 해주세요"
    };

    public void OnPtDown()
    {
        speak_txt.text = list_instruct[UnityEngine.Random.Range(0,10)];
        ground_truth__.Add(speak_txt.text);
        Debug.Log("버튼이 눌려지고 있음");
        startRecording();
        time_remain = time_limit;
        _pressed = true;
        time_remain_text.gameObject.SetActive(true);
        gameObject.transform.position = new Vector3(10000, 10000, 10000);
    }

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    speak_txt.SetActive(false);
    //    Debug.Log("버튼 눌림이 해제됨");
    //    stopRecording();
    //    _pressed = false;
    //}

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }


    const int BlockSize_16Bit = 2;
    string url;
    private string _microphoneID = null;
    private AudioClip _recording = null;
    private int _recordingLengthSec = 15;
    private int _recordingHZ = 22050;

    // Start is called before the first frame update
    void Start()
    {
        time_remain = 0;
        speak_txt.text = "";
        Pnum_text.text = "P" + PlayerPrefs.GetInt("milestone_pnum").ToString();
        time_remain_text.gameObject.SetActive(false);
        time_limit = PlayerPrefs.GetFloat("milestone_time_lim");

        //petctrl_script = GameObject.Find("Scripts").GetComponent<Petctrl>();
        // 사용할 언어(Kor)를 맨 뒤에 붙임
        url = "https://naveropenapi.apigw.ntruss.com/recog/v1/stt?lang=Kor";
        _microphoneID = Microphone.devices[0];
        //bgm_player_ = GameObject.Find("Audio player").GetComponent<bgm_player>();
    }

    // Update is called once per frame
    void Update()
    {
        slider_time.value = time_remain / time_limit;
        time_remain_text.text = $"{(int)time_remain + (int)1}초 안에 말해주세요";
        if (_pressed && time_remain > 0) time_remain -= Time.deltaTime;
        else if(_pressed)
        {
            speak_txt.text = "";
            Debug.Log("버튼 눌림이 해제됨");
            stopRecording();
            _pressed = false;
            time_remain_text.gameObject.SetActive(false);

        }
    }

    // 버튼을 OnPointerDown 할 때 호출
    public void startRecording()
    {
        Debug.Log("start recording");
        _recording = Microphone.Start(_microphoneID, false, _recordingLengthSec, _recordingHZ);
    }

    // 버튼을 OnPointerUp 할 때 호출
    public void stopRecording()
    {
        if (Microphone.IsRecording(_microphoneID))
        {
            Microphone.End(_microphoneID);

            Debug.Log("stop recording");
            if (_recording == null)
            {
                Debug.LogError("nothing recorded");
                return;
            }
            // audio clip to byte array
            byte[] byteData = getByteFromAudioClip(_recording);
            //byte[] byteData = WavUtility.FromAudioClip(_recording);

            // 녹음된 audioclip api 서버로 보냄
            StartCoroutine(PostVoice(url, byteData));
        }
        return;
    }
    private byte[] getByteFromAudioClip(AudioClip audioClip)
    {
        MemoryStream stream = new MemoryStream();
        const int headerSize = 44;
        ushort bitDepth = 16;

        int fileSize = audioClip.samples * BlockSize_16Bit + headerSize;

        // audio clip의 정보들을 file stream에 추가(링크 참고 함수 선언)
        WriteFileHeader(ref stream, fileSize);
        WriteFileFormat(ref stream, audioClip.channels, audioClip.frequency, bitDepth);
        WriteFileData(ref stream, audioClip, bitDepth);

        // stream을 array형태로 바꿈
        byte[] bytes = stream.ToArray();

        return bytes;
    }
    [Serializable]
    public class VoiceRecognize
    {
        public string text;
    }

    private IEnumerator PostVoice(string url, byte[] data)
    {
        // request 생성
        WWWForm form = new WWWForm();
        UnityWebRequest request = UnityWebRequest.Post(url, form);

        // 요청 헤더 설정
        request.method = "POST";
        request.SetRequestHeader("X-NCP-APIGW-API-KEY-ID", "p81oclc4zf");
        request.SetRequestHeader("X-NCP-APIGW-API-KEY", "BGUFP6VLAFITFOoJc0YO3jHfesVLkpny8XqgfIBZ");
        request.SetRequestHeader("Content-Type", "application/octet-stream");

        // 바디에 처리과정을 거친 Audio Clip data를 실어줌
        request.uploadHandler = new UploadHandlerRaw(data);

        // 요청을 보낸 후 response를 받을 때까지 대기
        yield return request.SendWebRequest();

        // 만약 response가 비어있다면 error
        if (request == null)
        {
            Debug.LogError(request.error);
        }
        else
        {
            // json 형태로 받음 {"text":"인식결과"}
            string message = request.downloadHandler.text;
            //Debug.Log("Voice Server responded: " + message);

            VoiceRecognize voiceRecognize = JsonUtility.FromJson<VoiceRecognize>(message);

            Debug.Log("Voice Server responded: " + voiceRecognize.text);
            res_text.text = "결과: " + voiceRecognize.text;
            cnt_answer++;

            cnt_ans__.Add(cnt_answer);
            //지시문 text는 OnPointerDown 함수에서 Add
            answer__.Add(voiceRecognize.text);
            cnt_answer_text.text = "갯수: " + cnt_answer.ToString();

            Invoke("OnPtDown", 2f);
            //if(cnt_answer == 2)
            //{
            //    JsonSave();
            //}
            //excute_motion(voiceRecognize.text);
            //// Voice Server responded: 인식결과
        }
    }

    public void JsonSave()
    {
        SaveData_v saveData = new SaveData_v();

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

        if (!File.Exists(Path.Combine(path,fileN))) File.WriteAllText(Path.Combine(path, fileN), json);
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

    public void back_bt_clicked()
    {
        JsonSave();
        SceneManager.LoadScene("milestone_main");
    }

    private void OnApplicationQuit()
    {
        JsonSave();
    }

    //void excute_motion(string motion_text)
    //{
    //    if (motion_text == "앉아" || motion_text.Contains("앉") || motion_text.Contains("않") || motion_text.Contains("안"))
    //    {
    //        petctrl_script.pet_idle_sit_idle();
    //    }
    //    else if (motion_text == "엎드려" || motion_text.Contains("엎") || motion_text.Contains("업") || motion_text.Contains("업드")
    //         || motion_text.Contains("어뜨"))
    //    {
    //        petctrl_script.pet_idle_lying_idle();
    //    }
    //    else if (motion_text == "기다려" || motion_text.Contains("잠깐") || motion_text.Contains("멈춰") || motion_text.Contains("거기서"))
    //    {
    //        petctrl_script.pet_wait();
    //    }
    //    else if (motion_text == "점프" || motion_text.Contains("점") || motion_text.Contains("뛰") || motion_text.Contains("띄")
    //        || motion_text.Contains("쩜"))
    //    {
    //        petctrl_script.pet_jump();
    //    }
    //    else if (motion_text == "짖어" || motion_text.Contains("짖") || motion_text.Contains("울어")
    //            || motion_text.Contains("멍") || motion_text.Contains("몽"))
    //    {
    //        petctrl_script.pet_bark();
    //    }
    //    else
    //    {
    //        petctrl_script.set_text_speechBubble("다시 말해볼까요?");
    //        petctrl_script.pet_reaction_false();
    //    }
    //    //else if (motion_text == "손")
    //    //{
    //    //    petctrl_script.pet_hand();
    //    //}
    //    //else if (motion_text == "빵")
    //    //{
    //    //    petctrl_script.pet_idle_lay_idle();
    //    //}
    //    //else if (motion_text == "이리와")
    //    //{
    //    //    petctrl_script.pet_come();
    //    //}
    //    //else if (motion_text == "오른쪽")
    //    //{
    //    //    petctrl_script.pet_RTurn();
    //    //}
    //    //else if (motion_text == "왼쪽")
    //    //{
    //    //    petctrl_script.pet_LTurn();
    //    //}
    //}

    private static int WriteFileHeader(ref MemoryStream stream, int fileSize)
    {
        int count = 0;
        int total = 12;

        // riff chunk id
        byte[] riff = Encoding.ASCII.GetBytes("RIFF");
        count += WriteBytesToMemoryStream(ref stream, riff, "ID");

        // riff chunk size
        int chunkSize = fileSize - 8; // total size - 8 for the other two fields in the header
        count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(chunkSize), "CHUNK_SIZE");

        byte[] wave = Encoding.ASCII.GetBytes("WAVE");
        count += WriteBytesToMemoryStream(ref stream, wave, "FORMAT");

        // Validate header
        Debug.AssertFormat(count == total, "Unexpected wav descriptor byte count: {0} == {1}", count, total);

        return count;
    }

    private static int WriteFileFormat(ref MemoryStream stream, int channels, int sampleRate, UInt16 bitDepth)
    {
        int count = 0;
        int total = 24;

        byte[] id = Encoding.ASCII.GetBytes("fmt ");
        count += WriteBytesToMemoryStream(ref stream, id, "FMT_ID");

        int subchunk1Size = 16; // 24 - 8
        count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(subchunk1Size), "SUBCHUNK_SIZE");

        UInt16 audioFormat = 1;
        count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(audioFormat), "AUDIO_FORMAT");

        UInt16 numChannels = Convert.ToUInt16(channels);
        count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(numChannels), "CHANNELS");

        count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(sampleRate), "SAMPLE_RATE");

        int byteRate = sampleRate * channels * BytesPerSample(bitDepth);
        count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(byteRate), "BYTE_RATE");

        UInt16 blockAlign = Convert.ToUInt16(channels * BytesPerSample(bitDepth));
        count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(blockAlign), "BLOCK_ALIGN");

        count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(bitDepth), "BITS_PER_SAMPLE");

        // Validate format
        Debug.AssertFormat(count == total, "Unexpected wav fmt byte count: {0} == {1}", count, total);

        return count;
    }

    private static int WriteFileData(ref MemoryStream stream, AudioClip audioClip, UInt16 bitDepth)
    {
        int count = 0;
        int total = 8;

        // Copy float[] data from AudioClip
        float[] data = new float[audioClip.samples * audioClip.channels];
        audioClip.GetData(data, 0);

        byte[] bytes = ConvertAudioClipDataToInt16ByteArray(data);

        byte[] id = Encoding.ASCII.GetBytes("data");
        count += WriteBytesToMemoryStream(ref stream, id, "DATA_ID");

        int subchunk2Size = Convert.ToInt32(audioClip.samples * BlockSize_16Bit); // BlockSize (bitDepth)
        count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(subchunk2Size), "SAMPLES");

        // Validate header
        Debug.AssertFormat(count == total, "Unexpected wav data id byte count: {0} == {1}", count, total);

        // Write bytes to stream
        count += WriteBytesToMemoryStream(ref stream, bytes, "DATA");

        // Validate audio data
        Debug.AssertFormat(bytes.Length == subchunk2Size, "Unexpected AudioClip to wav subchunk2 size: {0} == {1}", bytes.Length, subchunk2Size);

        return count;
    }

    private static int WriteBytesToMemoryStream(ref MemoryStream stream, byte[] bytes, string tag = "")
    {
        int count = bytes.Length;
        stream.Write(bytes, 0, count);
        //Debug.LogFormat ("WAV:{0} wrote {1} bytes.", tag, count);
        return count;
    }

    private static int BytesPerSample(UInt16 bitDepth)
    {
        return bitDepth / 8;
    }

    private static byte[] ConvertAudioClipDataToInt16ByteArray(float[] data)
    {
        MemoryStream dataStream = new MemoryStream();

        int x = sizeof(Int16);

        Int16 maxValue = Int16.MaxValue;

        int i = 0;
        while (i < data.Length)
        {
            dataStream.Write(BitConverter.GetBytes(Convert.ToInt16(data[i] * maxValue)), 0, x);
            ++i;
        }
        byte[] bytes = dataStream.ToArray();

        // Validate converted bytes
        Debug.AssertFormat(data.Length * x == bytes.Length, "Unexpected float[] to Int16 to byte[] size: {0} == {1}", data.Length * x, bytes.Length);

        dataStream.Dispose();

        return bytes;
    }
}

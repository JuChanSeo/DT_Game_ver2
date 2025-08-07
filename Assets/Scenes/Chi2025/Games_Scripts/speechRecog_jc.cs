using System.Collections;
using UnityEngine;
using System;
using System.Text;
using System.IO;
using UnityEngine.Networking;	// UnityWebRequest사용을 위해서 적어준다.
using UnityEngine.EventSystems;
using TMPro;

public class speechRecog_jc : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Petctrl petctrl_script;
    public TMP_Text res_text;
    public GameObject speak_txt;
    bool _pressed = false;
    bgm_player bgm_player_;

    public void OnPointerDown(PointerEventData eventData)
    {
        speak_txt.SetActive(true);
        Debug.Log("버튼이 눌려지고 있음");
        startRecording();
        _pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        speak_txt.SetActive(false);
        Debug.Log("버튼 눌림이 해제됨");
        stopRecording();
            _pressed = false;
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
        speak_txt.SetActive(false);
        petctrl_script = GameObject.Find("Scripts").GetComponent<Petctrl>();
        // 사용할 언어(Kor)를 맨 뒤에 붙임
        url = "https://naveropenapi.apigw.ntruss.com/recog/v1/stt?lang=Kor";
        _microphoneID = Microphone.devices[0];
        bgm_player_ = GameObject.Find("Audio player").GetComponent<bgm_player>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
            excute_motion(voiceRecognize.text);
            //// Voice Server responded: 인식결과
        }
    }

    void excute_motion(string motion_text)
    {
        if(motion_text == "앉아" || motion_text.Contains("앉") || motion_text.Contains("않") || motion_text.Contains("안"))
        {
            petctrl_script.pet_idle_sit_idle();
        }
        else if(motion_text == "엎드려" || motion_text.Contains("엎") || motion_text.Contains("업") || motion_text.Contains("업드")
             || motion_text.Contains("어뜨"))
        {
            petctrl_script.pet_idle_lying_idle();
        }
        else if (motion_text == "기다려" || motion_text.Contains("잠깐") || motion_text.Contains("멈춰") || motion_text.Contains("거기서"))
        {
            petctrl_script.pet_wait();
        }
        else if (motion_text == "점프" || motion_text.Contains("점") || motion_text.Contains("뛰") || motion_text.Contains("띄")
            || motion_text.Contains("쩜"))
        {
            petctrl_script.pet_jump();
        }
        else if (motion_text == "짖어" || motion_text.Contains("짖") || motion_text.Contains("울어")
                || motion_text.Contains("멍") || motion_text.Contains("몽"))
        {
            petctrl_script.pet_bark();
        }
        else
        {
            petctrl_script.set_text_speechBubble("다시 말해볼까요?");
            petctrl_script.pet_reaction_false();
        }
        //else if (motion_text == "손")
        //{
        //    petctrl_script.pet_hand();
        //}
        //else if (motion_text == "빵")
        //{
        //    petctrl_script.pet_idle_lay_idle();
        //}
        //else if (motion_text == "이리와")
        //{
        //    petctrl_script.pet_come();
        //}
        //else if (motion_text == "오른쪽")
        //{
        //    petctrl_script.pet_RTurn();
        //}
        //else if (motion_text == "왼쪽")
        //{
        //    petctrl_script.pet_LTurn();
        //}
    }
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

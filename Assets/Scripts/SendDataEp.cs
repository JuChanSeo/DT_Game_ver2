using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Net;
using System;
using System.Text;
using TMPro;
using UnityEngine.Profiling;
using System.Data.SqlTypes;
using UnityEditor;


public class SendDataEp : MonoBehaviour
{
    public string id;
    public string emotion;
    public string question;
    public string answer;
    
    public string wav_name;
    public string file; 
    
    public byte[] fileData;
    
    void Start()
    {
        this.id = "null_id";
        this.emotion = "null_emotion";
        this.question = "null_question";
        this.answer = "null_answer";
    }
    

    // StartCoroutine(Upload());
    public void UpdateEmotion(string emotion)
    {
        this.emotion = emotion;
        this.question = "null_question";
        this.answer = "null_answer";
    }
    public void UpdateQA(string question, string answer)
    {
        this.emotion = "null_emotion";
        this.question = question;
        this.answer = answer;
    }

    public void UpdateWAV(string wav_name, string file, byte[] fileData){
        this.wav_name= wav_name;
        this.file=file;
        this.fileData=fileData;
       
    }

    public void Send()
    {
        if(this.wav_name==null){
            this.wav_name="example_wav_name";
        }
        if(this.file==null){
            this.file="example.wav";
        }
        // Debug.Log(this.id+this.emotion+this.question+this.answer+this.wav_name+this.file);
        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        if(this.answer==null){
            this.answer="empthy answer";
        }
        //string URL = "https://223.130.138.24.nip.io:32163/be.runtime/dev/v1/service/gameTypeB-2/test/episode";
        string URL = "https://serengeti.aifrica.co.kr:31458/be.runtime/dev/v1/service/gameTypeB-2/test/episode";
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        // this.id="id_string";
        // this.wav_name="dummy_example";
        // this.file="Assets/Resources/Dummy_example.wav";
        // byte[] fileData=File.ReadAllBytes(this.file);

        if (this.emotion==null){
            this.emotion="null_emotion";
        }
        Debug.Log(this.id+this.emotion+this.question+this.answer);
        formData.Add(new MultipartFormDataSection("user_id", this.id));
        formData.Add(new MultipartFormDataSection("emotion", this.emotion));
        formData.Add(new MultipartFormDataSection("question", this.question));
        formData.Add(new MultipartFormDataSection("answer", this.answer));
        // formData.Add(new MultipartFormDataSection("wav_name", this.wav_name));
        // formData.Add(new MultipartFormFileSection("file", fileData, Path.GetFileName(this.file), "audio/wav"));
        // formData.Add(new MultipartFormFileSection("file", fileData, this.wav_name, "audio/wav"));
        
        
        using (UnityWebRequest request = UnityWebRequest.Post(URL, formData))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
            }
            else if (request.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.Log(request.error);
            }
            else if (request.result == UnityWebRequest.Result.InProgress)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("성공!" + request.downloadHandler.text);
            }
        }
    }


}

public class FormFile
{
    public string Name { get; set; }

    public string ContentType { get; set; }

    public string FilePath { get; set; }

    public Stream Stream { get; set; }
}

public class RequestHelper
{

    public static string PostMultipart(string url, Dictionary<string, object> parameters)
    {

        string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
        byte[] boundaryBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.ContentType = "multipart/form-data; boundary=" + boundary;
        request.Method = "POST";
        request.KeepAlive = true;
        request.Credentials = System.Net.CredentialCache.DefaultCredentials;

        if (parameters != null && parameters.Count > 0)
        {

            using (Stream requestStream = request.GetRequestStream())
            {

                foreach (KeyValuePair<string, object> pair in parameters)
                {

                    requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                    if (pair.Value is FormFile)
                    {
                        FormFile file = pair.Value as FormFile;
                        string header = "Content-Disposition: form-data; name=\"" + pair.Key + "\"; filename=\"" + file.Name + "\"\r\nContent-Type: " + file.ContentType + "\r\n\r\n";
                        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(header);
                        requestStream.Write(bytes, 0, bytes.Length);
                        byte[] buffer = new byte[32768];
                        int bytesRead;
                        if (file.Stream == null)
                        {
                            // upload from file
                            using (FileStream fileStream = File.OpenRead(file.FilePath))
                            {
                                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                                    requestStream.Write(buffer, 0, bytesRead);
                                fileStream.Close();
                            }
                        }
                        else
                        {
                            // upload from given stream
                            while ((bytesRead = file.Stream.Read(buffer, 0, buffer.Length)) != 0)
                                requestStream.Write(buffer, 0, bytesRead);
                        }
                    }
                    else
                    {
                        string data = "Content-Disposition: form-data; name=\"" + pair.Key + "\"\r\n\r\n" + pair.Value;
                        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
                        requestStream.Write(bytes, 0, bytes.Length);
                    }
                }

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                requestStream.Write(trailer, 0, trailer.Length);
                requestStream.Close();
            }
        }

        using (WebResponse response = request.GetResponse())
        {
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
                return reader.ReadToEnd();
        }


    }
}

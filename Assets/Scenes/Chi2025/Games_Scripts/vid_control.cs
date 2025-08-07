using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;

public class vid_control : MonoBehaviour
{
    public VideoPlayer start_video;
    game_mode game_mode_script;
    Tutorial_pictureGame tutorial_pictureGame_script;
    
    // Start is called before the first frame update
    void Start()
    {
        game_mode_script = GameObject.Find("Scripts").GetComponent<game_mode>();
        tutorial_pictureGame_script = GameObject.Find("Scripts").GetComponent<Tutorial_pictureGame>();
        start_video.loopPointReached += checkover_start_vid;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void checkover_start_vid(VideoPlayer np)
    {
        print("start video is over");
        if(SceneManager.GetActiveScene().name == "Game3")
        {
            game_mode_script.start_scan();
        }

        if (SceneManager.GetActiveScene().name == "Tutorial_Game3")
        {
            Debug.Log("check");
            tutorial_pictureGame_script.picture_next_bt_clicked();
        }
    }

    public void excute_vid()
    {
        start_video.Play();
    }
}

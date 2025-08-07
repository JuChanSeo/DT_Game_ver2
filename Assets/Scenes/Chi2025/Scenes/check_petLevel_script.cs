using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class check_petLevel_script : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject levelUp_popUp;
    public TextMeshProUGUI levelUp_popUp_text;

    void Start()
    {
        if (PlayerPrefs.GetFloat("exp") > 1f)
        {
            //레벨 업
            PlayerPrefs.SetFloat("exp", 0);
            if (PlayerPrefs.GetInt("Level_pet") < 5)
            {
                int previous_level = PlayerPrefs.GetInt("Level_pet");
                int cur_level = PlayerPrefs.GetInt("Level_pet") + 1;
                string levelup_str = $"강아지가 레벨{cur_level}로 한 단계 성장했어요!\n게임을 시작하여 확인해보세요";
                levelUp_popUp.SetActive(true);
                PlayerPrefs.SetInt("Level_pet", PlayerPrefs.GetInt("Level_pet") + 1);

                levelUp_popUp_text.text = levelup_str;
                levelUp_popUp.SetActive(true);
                Invoke("popUp_disappear", 10f);

            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void popUp_disappear()
    {
        levelUp_popUp.SetActive(false);
    }
}

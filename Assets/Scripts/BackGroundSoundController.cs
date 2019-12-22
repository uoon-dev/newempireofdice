using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static MusicController;

public class BackGroundSoundController : MonoBehaviour
{

    public static BackGroundSoundController instance;
    [SerializeField] AudioSource mainBackGroundAudioSource;
    [SerializeField] AudioSource gameBackGroundAudioSource;
    public static class BGM_NAME {
        public const string MAIN_BGM = "mainBGM";
        public const string GAME_BGM = "gameBGM";
    }    
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      /*
      1. 처음에는 무조건 강제로 끄는 거.
      2. 그 다음에 배경음악을 꺼놨다면 다음부턴 끄게 하지 않도록 하는 거.
      */

    }

    public void StartPlay(string soundName)
    {
        if (PlayerPrefs.GetInt( SOUND_KEY.BGM, SOUND_STATUS.ON) != SOUND_STATUS.ON) {
            //PlayerSettings.muteOtherAudioSources = false;
            return;
        }

        //PlayerSettings.muteOtherAudioSources = true;

        switch (soundName) {
        case BGM_NAME.MAIN_BGM:
            if (gameBackGroundAudioSource.isPlaying) {
                gameBackGroundAudioSource.Stop();
            }
            if (!mainBackGroundAudioSource.isPlaying) {
                mainBackGroundAudioSource.Play();
            }
            break;
        case BGM_NAME.GAME_BGM:
            if (mainBackGroundAudioSource.isPlaying)
            {
                mainBackGroundAudioSource.Stop();
            }
            if (!gameBackGroundAudioSource.isPlaying)
            {
                gameBackGroundAudioSource.Play();
            }
            break;
    }
    }

    public void StopPlay(string soundName) {
        switch (soundName)
        {
            case BGM_NAME.MAIN_BGM:
                if (mainBackGroundAudioSource.isPlaying)
                {
                    mainBackGroundAudioSource.Stop();
                }
                break;
            case BGM_NAME.GAME_BGM:
                if (gameBackGroundAudioSource.isPlaying)
                {
                    gameBackGroundAudioSource.Stop();
                }
                break;
        }
    }
}

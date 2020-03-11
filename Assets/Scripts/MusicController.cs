using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MusicController: MonoBehaviour
{
    [SerializeField] Sprite playingMusicIcon = null;
    [SerializeField] Sprite stoppingMusicIcon = null;
    [SerializeField] Sprite playingSoundIcon = null;
    [SerializeField] Sprite stoppingSoundIcon = null;
    [SerializeField] Text musicText= null;
    [SerializeField] Text effectText = null;
    [SerializeField] GameObject controlEffectSoundButton = null;
    [SerializeField] GameObject controlBGMMusicButton = null;
    LevelLoader levelLoader;


    public static MusicController instance = null;
    public static class SOUND_KEY {
        public const string BGM = "backgroundSound";
        public const string EFFECT = "effectSound";
    }
    public static class SOUND_STATUS {
        public const int ON = 1;
        public const int OFF = -999;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        SetMusicUI();
        SetEffectUI();
        controlEffectSoundButton.GetComponent<Button>().onClick.AddListener(OnClickChangeEffect);
        controlBGMMusicButton.GetComponent<Button>().onClick.AddListener(OnClickChangeMusic);
    }

    private void Initialize()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
    }  

    public void SetMusicUI() {
        Debug.Log(PlayerPrefs.GetInt(SOUND_KEY.BGM, SOUND_STATUS.ON) + ":SetMusicUI");
        if (PlayerPrefs.GetInt(SOUND_KEY.BGM, SOUND_STATUS.ON) == SOUND_STATUS.ON)
        {
            controlBGMMusicButton.GetComponent<Image>().sprite = playingMusicIcon;
            if (musicText != null)
            {
                musicText.text = "배경음악 On";
            }
        }
        else {
            controlBGMMusicButton.GetComponent<Image>().sprite = stoppingMusicIcon;
            if (musicText != null)
            {
                musicText.text = "배경음악 Off";
            }
        }
        
    }
    public void SetEffectUI()
    {
        if (PlayerPrefs.GetInt(SOUND_KEY.EFFECT, SOUND_STATUS.ON) == SOUND_STATUS.ON)
        {
            controlEffectSoundButton.GetComponent<Image>().sprite = playingSoundIcon;
            if (effectText != null) {
                effectText.text = "효과음 On";
            }
        }
        else
        {
            controlEffectSoundButton.GetComponent<Image>().sprite = stoppingSoundIcon;
            if (effectText != null)
            {
                effectText.text = "효과음 Off";
            }
        }
    }
    public string GetBGMName()
    {
        string currentSceneName= levelLoader.GetCurrentSceneName();
        return currentSceneName == "Level" ? BackGroundSoundController.BGM_NAME.GAME_BGM : BackGroundSoundController.BGM_NAME.MAIN_BGM;
    }
    public void OnClickChangeMusic() {
        if (PlayerPrefs.GetInt(SOUND_KEY.BGM, SOUND_STATUS.ON) == SOUND_STATUS.ON)
        {
            PlayerPrefs.SetInt(SOUND_KEY.BGM, SOUND_STATUS.OFF);
            if (BackGroundSoundController.instance != null)
            {
                BackGroundSoundController.instance.StopPlay(GetBGMName());
            }
        }
        else {
            PlayerPrefs.SetInt(SOUND_KEY.BGM, SOUND_STATUS.ON);
            if (BackGroundSoundController.instance != null)
            {
                BackGroundSoundController.instance.StartPlay(GetBGMName());
            }
        }
        SetMusicUI();
    }
    public void OnClickChangeEffect() {
        if (PlayerPrefs.GetInt(SOUND_KEY.EFFECT, SOUND_STATUS.ON) == SOUND_STATUS.ON)
        {
            PlayerPrefs.SetInt(SOUND_KEY.EFFECT, SOUND_STATUS.OFF);
        }
        else {
            PlayerPrefs.SetInt(SOUND_KEY.EFFECT, SOUND_STATUS.ON);
        }
        SetEffectUI();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MusicController;

public class EffectSoundController : MonoBehaviour
{
    public static EffectSoundController instance = null;
    [SerializeField] AudioSource getNewDiceSource;
    [SerializeField] AudioSource rewardMineSource;
    [SerializeField] AudioSource rewardDungeonSource;
    [SerializeField] AudioSource getLandSource;
    [SerializeField] AudioSource getLandPerfectSource;
    [SerializeField] AudioSource useMasicItemSource;
    [SerializeField] AudioSource rewardHorseSource;
    [SerializeField] AudioSource useArtifactItemSource;
    [SerializeField] AudioSource useSoldierSource;
    [SerializeField] AudioSource rewardBombSource;
    [SerializeField] AudioSource clickDiceSource;
    [SerializeField] AudioSource finishOneRoundSource;
    [SerializeField] AudioSource getLandPerfectSourceHumanVoiceSource;
    [SerializeField] AudioSource finishOneRoundSourceHumanVoiceSource;
    [SerializeField] AudioSource attackBlockSource;
    [SerializeField] AudioSource getLandPerfectSource2;

    
    public static class SOUND_NAME {
        public const string GET_NEW_DICE = "getNewDice";
        public const string REWARD_MINE = "rewardMine";
        public const string REWARD_DUNGEON = "rewardDungeon";
        public const string GET_LAND= "getLand";
        public const string GET_LAND_PERFECT="getLandPerfect";
        public const string USE_WIZARD_ITEM="useMasicItem";
        public const string REWARD_HORSE="rewardHorse";
        public const string USE_RELICS_ITEM="useArtifactItem";
        public const string USE_ARMY="useSoldier";
        public const string REWARD_BOMB="rewardBomb";
        public const string CLICK_DICE="clickDice";
        public const string FINISH_ONE_ROUND = "finishOneRound";
        public const string GET_LAND_PERFECT_HUMAN_VOICE = "getLandPerfectHumanVoice";
        public const string FINISH_ONE_ROUND_HUMAN_VOICE = "finishOneRoundHumanVoice";
        public const string ATTACK_BLOCK= "attackBlock";
        public const string GET_LAND_PERFECT_2 = "getLandPerfect2";
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySoundByName(string soundName) {
        if (PlayerPrefs.GetInt(SOUND_KEY.EFFECT, SOUND_STATUS.ON) != SOUND_STATUS.ON) return;
        switch (soundName) {
            case SOUND_NAME.GET_NEW_DICE:
                getNewDiceSource.Play();
                break;
            case SOUND_NAME.REWARD_MINE:
                rewardMineSource.Play();
                break;
            case SOUND_NAME.REWARD_DUNGEON:
                rewardDungeonSource.Play();
                break;
            case SOUND_NAME.GET_LAND:
                getLandSource.Play();
                break;
            case SOUND_NAME.GET_LAND_PERFECT:
                getLandPerfectSource.Play();
                break;
            case SOUND_NAME.USE_WIZARD_ITEM:
                useMasicItemSource.Play();
                break;
            case SOUND_NAME.REWARD_HORSE:
                rewardHorseSource.Play();
                break;
            case SOUND_NAME.USE_RELICS_ITEM:
                useArtifactItemSource.Play();
                break;
            case SOUND_NAME.USE_ARMY:
                useSoldierSource.Play();
                break;
            case SOUND_NAME.REWARD_BOMB:
                rewardBombSource.Play();
                break;
            case SOUND_NAME.CLICK_DICE:
                clickDiceSource.Play();
                break;
            case SOUND_NAME.FINISH_ONE_ROUND:
                finishOneRoundSource.Play();
                break;
            case SOUND_NAME.GET_LAND_PERFECT_HUMAN_VOICE:
                getLandPerfectSourceHumanVoiceSource.Play();
                break;
            case SOUND_NAME.FINISH_ONE_ROUND_HUMAN_VOICE:
                finishOneRoundSourceHumanVoiceSource.Play();
                break;
            case SOUND_NAME.ATTACK_BLOCK:
                attackBlockSource.Play();
                break;
            case SOUND_NAME.GET_LAND_PERFECT_2:
                getLandPerfectSource2.Play();
                break;
        }
    }
}

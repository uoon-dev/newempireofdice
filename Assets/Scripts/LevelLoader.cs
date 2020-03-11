using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Controllers.TutorialController;
using RedBlueGames.Tools.TextTyper;
using UnityEngine.Analytics;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] int timeToWait = 2;
    public string currentSceneName = "";
    int currentSceneIndex;
    public int currentLevelNumber;
    public bool goingToNextLevel = false;
    private static GameObject mainCanvas;
    public NewHeartController newHeartController;
    public static UIAlignController UIAlignController;

    private void Initialize()
    {
        newHeartController = FindObjectOfType<NewHeartController>();
        UIAlignController = FindObjectOfType<UIAlignController>();

        mainCanvas = GameObject.Find("Main Canvas");        

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        currentSceneName = SceneManager.GetActiveScene().name;
        currentLevelNumber = PlayerPrefs.GetInt("currentLevelNumber");

    }

    // Start is called before the first frame update
    void Awake()
    {
        Initialize();

        if (BackGroundSoundController.instance != null)
            BackGroundSoundController.instance.StartPlay(BackGroundSoundController.BGM_NAME.MAIN_BGM);
        if (currentSceneIndex == 0)
        {
            StartCoroutine(WaitForTime());
        }
        if (currentSceneName == Constants.SCENE_NAME.LEVEL || currentSceneName == Constants.SCENE_NAME.TUTORIAL) {
            if (BackGroundSoundController.instance != null)
                BackGroundSoundController.instance.StartPlay(BackGroundSoundController.BGM_NAME.GAME_BGM);
        }
    }
    IEnumerator WaitForTime()
    {
        yield return new WaitForSeconds(timeToWait);
        LoadNextScene();
    }
    public void LoadClickedMap(int levelNumber)
    {
        if (levelNumber == 1) {
            UIAlignController.DeactiveStartButton();
            mainCanvas.GetComponent<CanvasGroup>().DOFade(1, 0.4f).OnComplete(() => {
                PlayerPrefs.SetInt("currentLevelNumber", levelNumber);
                SceneManager.LoadScene(Constants.SCENE_NAME.TUTORIAL);
            });
        } else {
            if(newHeartController.CanUseHeart() == true) {
                UIAlignController.DeactiveStartButton();
                UIAlignController.ActiveHeartUseAnimation();
                mainCanvas.GetComponent<CanvasGroup>().DOFade(1, 0.4f).OnComplete(() => {
                    PlayerPrefs.SetInt("currentLevelNumber", levelNumber);
                    SceneManager.LoadScene(Constants.SCENE_NAME.LEVEL);
                });
            }
        }
    }
    public void LoadNextScene()
    {
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
    public void OnClickLoadNextLevel()
    {
        SetIsGoingToNextLevel(true);
        LoadNextLevel();
    }
    public void LoadNextLevel() 
    {
        if(newHeartController.CanUseHeart() == false) return;
        UIAlignController.DeactiveStartButton();
        UIAlignController.ActiveHeartUseAnimation();
        Invoke("InvokedLoadNextLevel", 0.4f);
    }

    public void SetIsGoingToNextLevel(bool isGoingToNextLevel)
    {
        goingToNextLevel = isGoingToNextLevel;
    }

    public bool GetIsGoingToNextLevel()
    {
        return goingToNextLevel;   
    }

    public void OnClickLoadCurrentScene()
    {
        LoadCurrentScene();
    }
    
    // 플레이 중 다시 시작할 때 사용
    public void LoadCurrentScene()
    {
        if (currentSceneName == Constants.SCENE_NAME.TUTORIAL) {
            PlayerPrefs.SetInt("currentLevelNumber", currentLevelNumber);
            TutorialController.SetTutorialCount(0);
            SceneManager.LoadScene(currentSceneName);
            return;
        }
        if(newHeartController.CanUseHeart() == false) {
            FindObjectOfType<PauseController>().HideScreen();
            return;
        }
        UIAlignController.DeactiveStartButton();
        UIAlignController.ActiveHeartUseAnimation();
        Invoke("InvokedLoadCurrentScene", 0.4f);
    }

    public void InvokedLoadCurrentScene()
    {
        PlayerPrefs.SetInt("currentLevelNumber", currentLevelNumber);
        SceneManager.LoadScene(currentSceneName);
    }
    public void InvokedLoadNextLevel()
    {
        PlayerPrefs.SetInt("currentLevelNumber", currentLevelNumber + 1);
        SceneManager.LoadScene("Level");
    }

    public void LoadMapScene()
    {
        AnalyticsEvent.LevelQuit(currentLevelNumber);
        PlayerPrefs.DeleteKey("currentLevelNumber");
        SceneManager.LoadScene("Map System");
    }
    public void LoadHomeScene()
    {
        SceneManager.LoadScene("Start Screen");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;;
    }
    public int GetCurrentLevelNumber()
    {
        return currentLevelNumber;
    }
}
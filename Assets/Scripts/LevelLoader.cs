using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Controllers.TutorialController;
using RedBlueGames.Tools.TextTyper;
public class LevelLoader : MonoBehaviour
{
    [SerializeField] int timeToWait = 3;
    public static string currentSceneName = "";
    int currentSceneIndex;
    public static int currentLevelNumber;
    public static bool goingToNextLevel = false;
    private static GameObject mainCanvas;
    // Start is called before the first frame update
    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        currentSceneName = SceneManager.GetActiveScene().name;
        currentLevelNumber = PlayerPrefs.GetInt("currentLevelNumber");
        if (BackGroundSoundController.instance != null)
            BackGroundSoundController.instance.StartPlay(BackGroundSoundController.BGM_NAME.MAIN_BGM);
        if (currentSceneIndex == 0)
        {
            StartCoroutine(WaitForTime());
        }
        if (currentSceneName == "Level" || currentSceneName == "Level 1") {
            if (BackGroundSoundController.instance != null)
                BackGroundSoundController.instance.StartPlay(BackGroundSoundController.BGM_NAME.GAME_BGM);
        }
        mainCanvas = GameObject.Find("Main Canvas");
    }
    IEnumerator WaitForTime()
    {
        yield return new WaitForSeconds(timeToWait);
        LoadNextScene();
    }
    public static void LoadClickedMap(int levelNumber)
    {
        if (levelNumber == 1) {
            mainCanvas.GetComponent<CanvasGroup>().DOFade(1, 0.4f).OnComplete(() => {
                PlayerPrefs.SetInt("currentLevelNumber", levelNumber);
                SceneManager.LoadScene("Level 1");
            });
        } else {
            if(CanUseHeart() == true) {
                FindObjectOfType<UIAlignController>().ActiveHeartUseAnimation();
                mainCanvas.GetComponent<CanvasGroup>().DOFade(1, 0.4f).OnComplete(() => {
                    PlayerPrefs.SetInt("currentLevelNumber", levelNumber);
                    SceneManager.LoadScene("Level");
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
        goingToNextLevel = true;
        LoadNextLevel();
    }
    public void LoadNextLevel() 
    {
        // if (currentSceneName == "Level 1") {
        //     PlayerPrefs.SetInt("currentLevelNumber", 2);    
        //     SceneManager.LoadScene("Level");
        //     return;
        // }
        if(CanUseHeart() == false) return;
        FindObjectOfType<UIAlignController>().ActiveHeartUseAnimation();
        Invoke("InvokedLoadNextLevel", 0.4f);
    }
    public void OnClickLoadCurrentScene()
    {
        LoadCurrentScene();
    }    
    // 플레이 중 다시 시작할 때 사용
    public void LoadCurrentScene()
    {
        if (currentSceneName == "Level 1") {
            PlayerPrefs.SetInt("currentLevelNumber", currentLevelNumber);
            TutorialController.SetTutorialCount(0);
            SceneManager.LoadScene(currentSceneName);
            return;
        }
        if(CanUseHeart() == false) {
            FindObjectOfType<PauseController>().HideScreen();
            return;
        }
        FindObjectOfType<UIAlignController>().ActiveHeartUseAnimation();
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
    public static bool CanUseHeart()
    {
        var heartController = FindObjectOfType<HeartController>();
        if (heartController.GetHeartAmount() <= 0) {
            heartController.ToggleNoHeartCanvas(true);
            return false;
        }
        heartController.UseHeart();
        return true;
    }
    public void LoadMapScene()
    {
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
    public static string GetCurrentSceneName()
    {
        return currentSceneName;
    }
    public static int GetCurrentLevelNumber()
    {
        return currentLevelNumber;
    }
}
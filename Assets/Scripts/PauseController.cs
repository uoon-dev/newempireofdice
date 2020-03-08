using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PauseController : MonoBehaviour
{
    [SerializeField] GameObject PauseCanvas = null;
    LevelLoader levelLoader;
    Text stageText;

    void Start()
    {
        Initialize();
        HideScreen();
    }

    private void Initialize()
    {
        levelLoader = FindObjectOfType<LevelLoader>(); 

        stageText = Utils.FindInActiveObjectByName(Constants.GAME_OBJECT_NAME.STAGE_TEXT).GetComponent<Text>();
        stageText.text = $"Stage {levelLoader.GetCurrentLevelNumber()}";
    }

    public void ShowScreen()
    {
        var blocks = FindObjectsOfType<Block>();
        foreach (var block in blocks) {
            block.HideTooltip();
        }
        PauseCanvas.GetComponent<Canvas>().sortingOrder = 13;
        PauseCanvas.SetActive(true);
    }

    public void HideScreen()
    {
        PauseCanvas.SetActive(false);
    }
}

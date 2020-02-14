using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIAlignController : MonoBehaviour
{
    [SerializeField] GameObject creditButton = null;
    [SerializeField] GameObject helpButton = null;
    [SerializeField] GameObject compassImage = null;
    [SerializeField] GameObject pauseButton = null;
    [SerializeField] GameObject resetDiceScreen = null;
    [SerializeField] GameObject levelLeftArea = null;
    [SerializeField] GameObject levelRightArea = null;
    [SerializeField] GameObject levelCompleteLeftButton = null;
    [SerializeField] GameObject levelCompleteRightButton = null;
    [SerializeField] GameObject levelLostLeftButton = null;
    [SerializeField] GameObject mainCanvas = null;
    [SerializeField] GameObject background = null;
    [SerializeField] Sprite[] backgroundImages = null;
    [SerializeField] Sprite startButtonLoadingImage = null;

    GameObject borderSign = null;
    GameObject heartUseAnimationObject;
    Image startButtonImage = null;
    Button startButton = null;
    Button nextLevelButton = null;
    string currentSceneName;
    private int backgroundImageIndex = 0;
    LevelLoader levelLoader;

    void Start()
    {
        Initialize();
        currentSceneName = levelLoader.GetCurrentSceneName();
        switch(currentSceneName) 
        {
            case "Start Screen": 
            {
                HomeAlignItems();
                break;
            }
            case "Map System": 
            {
                MapAlignItems();
                break;
            }
            default: break;
        }

        if (currentSceneName.Equals("Level")) 
        {
            LevelAlignItems();
        }

        if (mainCanvas != null)
            SetBackgroundImage();

        DeactiveHeartUseAnimation();
    }

    private void Initialize()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        
        borderSign = GameObject.Find("Border Sign");
    }

    public void DeactiveHeartUseAnimation()
    {
        heartUseAnimationObject = GameObject.Find("Heart Use Animation");
        if (heartUseAnimationObject != null) {
            heartUseAnimationObject.GetComponent<CanvasGroup>().DOFade(0, 0);
            heartUseAnimationObject.GetComponent<Animator>().enabled = false;
        }
    }

    private void HomeAlignItems() 
    {
        Vector3 creditButtonSize = creditButton.GetComponent<RectTransform>().sizeDelta;
        creditButton.transform.position = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width - creditButtonSize.x/2 - Screen.width/15 - (Screen.width >= 1792 ? 180 : 0), creditButtonSize.y/2 + Screen.height/5 , 5));

        Vector3 helpButtonSize = helpButton.GetComponent<RectTransform>().sizeDelta;
        helpButton.transform.position = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width - creditButtonSize.x/2 - Screen.width/15 - (Screen.width >= 1792 ? 180 : 0), creditButtonSize.y/2 + Screen.height/15 , 5));
    }

    private void MapAlignItems() 
    {
        float cameraHeight = 2 * Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;
        float leftPaddingAbsolutePositionX = -borderSign.transform.position.x - cameraWidth / 2 + 0.325f;

        var header = mainCanvas.transform.Find("Header");
        var headerLeftSide = header.transform.Find("Left");
        var headerRightSide = header.transform.Find("Right");

        headerLeftSide.position = new Vector3(leftPaddingAbsolutePositionX - 3.7f, 1.7f, 0);
        headerRightSide.position = new Vector3(3.7f - leftPaddingAbsolutePositionX, 1.7f, 0);
        
        var footer = mainCanvas.transform.Find("Footer");
        var footerRightSide = footer.transform.Find("Right");

        footerRightSide.position = new Vector3(2.4f - leftPaddingAbsolutePositionX, -cameraHeight/3 - 0.15f, 0);
    }

    private void LevelAlignItems() {
        switch(BlockController.GetBoardSize()) 
        {
            case 4: {
                levelLeftArea.transform.GetChild(0).transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                break;
            }
            case 5: {
                levelLeftArea.transform.GetChild(0).transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                break;
            }
            case 6: {
                levelLeftArea.transform.GetChild(0).transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);                
                break;
            }
            case 7: {
                levelLeftArea.transform.GetChild(0).transform.localScale = new Vector3(1f, 1f, 1f);
                break;
            }
            default: {
                levelLeftArea.transform.GetChild(0).transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                break;
            }
        }
    }

    public void SetBackgroundImage()
    {
        int currentLevel = levelLoader.GetCurrentLevelNumber();
        if (StorageController.IsBackgroundImageIndexSaved(currentLevel))
        {
            backgroundImageIndex = StorageController.LoadBackgroundImageIndex(currentLevel);
        }
        else {
            backgroundImageIndex = UnityEngine.Random.Range(0, backgroundImages.Length / 2);
            StorageController.SaveBackgroundImageIndex(currentLevel, backgroundImageIndex);
        }
        

        foreach (var backgroundImage in backgroundImages) 
        {
            if (backgroundImage.name == $"bg_play_{backgroundImageIndex}") 
            {
                mainCanvas.GetComponent<Image>().sprite = backgroundImage;
            }
        }
    }
    
    public void UpdateBackgroundImage()
    {
        foreach (var backgroundImage in backgroundImages) 
        {
            if (backgroundImage.name == $"bg_play_{backgroundImageIndex}_done") 
            {
                background.GetComponent<Image>().sprite = backgroundImage;
                background.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
            }
        }
    }

    public void UpdateBlocksNumberColor()
    {
        var blocks = FindObjectsOfType<Block>();
        foreach (var block in blocks)
        {
            // undestroyed normal block
            if (!block.isDestroyed && block.GetBlockType() == String.Empty && !block.isClickable)
            {
                block.blockText.color = new Color32(72,170,92, 255);
            }
        }
    }

    public void UnClickDices()
    {
        var dices = FindObjectsOfType<Dice>();
        foreach (Dice dice in dices)
        {
            dice.UnClickDice();
        }
        CloseTooltips();
    }

    public void CloseTooltips()
    {
        var blocks = FindObjectsOfType<Block>();
        foreach (var block in blocks)
        {
            block.CloseTooltip();
        }
    }

    public void ActiveHeartUseAnimation()
    {
        heartUseAnimationObject.GetComponent<CanvasGroup>().DOFade(1, 0);
        heartUseAnimationObject.GetComponent<Animator>().enabled = true;
    }

    public void DeactiveStartButton()
    {
        string currentSceneName = levelLoader.GetCurrentSceneName();

        if (currentSceneName == Constants.SCENE_NAME.MAP_SYSTEM)
        {
            startButtonImage = GameObject.Find(Constants.GAME_OBJECT_NAME.START_BUTTON).GetComponent<Image>();
            startButtonImage.sprite = startButtonLoadingImage;
            
            startButton = GameObject.Find(Constants.GAME_OBJECT_NAME.START_BUTTON).GetComponent<Button>();        
            startButton.interactable = false;
        } 
        else if (currentSceneName == Constants.SCENE_NAME.LEVEL)
        {
            if (GameObject.Find(Constants.GAME_OBJECT_NAME.RETRY_BUTTON) != null)
            {
                startButton = GameObject.Find(Constants.GAME_OBJECT_NAME.RETRY_BUTTON).GetComponent<Button>();
                startButton.interactable = false;
            }
            if (GameObject.Find(Constants.GAME_OBJECT_NAME.NEXT_LEVEL_BUTTON) != null)
            {
                nextLevelButton = GameObject.Find(Constants.GAME_OBJECT_NAME.NEXT_LEVEL_BUTTON).GetComponent<Button>();
                nextLevelButton.interactable = false;
            }
        }
    }    
}

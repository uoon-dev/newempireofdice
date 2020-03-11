using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using RedBlueGames.Tools.TextTyper;
using Controllers.TutorialController;

public class Dice : MonoBehaviour
{
    [SerializeField] Sprite unclickedDiceImage = null;
    [SerializeField] Sprite clickedDiceImage = null;
    [SerializeField] GameObject BlockBox = null;

    Text diceText = null;
    AudioSource clickSound = null;

    int minNumber = 1;
    int maxNumber = 7;
    int tutorialCount = 0;
    public static bool isSound = true;
    private Animator diceAnimator;

    bool isClicked = false;
    bool isDestroyed = false;
    LevelLoader levelLoader;
    NewTutorialController newTutorialController;
    DiceController diceController;
    BlockController blockController;

    void Start()
    {
        Initialize();

        if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.TUTORIAL) 
        {
            SetTutorialDiceNumber();
        } 
        else 
        {
            SetDiceNumber(minNumber, maxNumber);
        }

        SetClickSound();
        diceAnimator = GetComponent<Animator>();
        diceAnimator.ResetTrigger("isClicked");
    }

    private void Initialize()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        newTutorialController = FindObjectOfType<NewTutorialController>();
        blockController = FindObjectOfType<BlockController>();
        diceController = FindObjectOfType<DiceController>();
        diceText = this.transform.Find(Constants.GAME_OBJECT_NAME.NUMBER_TEXT).GetComponent<Text>();
    }

    public void OnClickButton() {
        
        var attackGageDisplay = FindObjectOfType<AttackGageDisplay>();
        var blocks = FindObjectsOfType<Block>();
        foreach (var block in blocks)
        {
            block.CloseTooltip();
        }

        ToggleDice();
        if (TutorialDialogueController.dialogueTurn == 7)
        {
            var arrow = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.ARROW);
            Block pickedBlock = blockController.GetOneBlock(Constants.TYPE.FIRST_BLOCK);

            DOTween.Kill(arrow.transform);
            diceController.ToggleOneDiceClick(this.name, false);
            newTutorialController.MoveArrowToBlock(pickedBlock);
        }
        else if (TutorialDialogueController.dialogueTurn == 11)
        {

            string[] diceNames = {"Dice (2)", "Dice (3)"};
            diceController.ToggleOneDiceClick(this.name, false);
            var arrow = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.ARROW);
            var clonedArrow = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.ARROW+"(Clone)");
            if (this.name == "Dice (2)")
            {
                arrow.GetComponent<CanvasGroup>().DOFade(0, 0.2f);
            }
            else if (this.name == "Dice (3)")
            {
                clonedArrow.GetComponent<CanvasGroup>().DOFade(0, 0.2f);
            }

            if (diceController.isDicesPickRight(diceNames, 2))
            {
                DestroyImmediate(clonedArrow);
                Block pickedBlock = blockController.GetOneBlock(Constants.TYPE.FIRST_BLOCK);
                newTutorialController.MoveArrowToBlock(pickedBlock);
            }
        }
        else if (TutorialDialogueController.dialogueTurn == 15)
        {
            string[] diceNames = {"Dice (4)", "Dice (5)", "Dice (6)"};
            diceController.ToggleOneDiceClick(this.name, false);
            var arrow = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.ARROW);
            var clonedArrow1 = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.ARROW+"(Clone)");
            var clonedArrow2 = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.ARROW+"(Clone)2");
            if (this.name == "Dice (4)")
            {
                arrow.GetComponent<CanvasGroup>().DOFade(0, 0.2f);
            }
            else if (this.name == "Dice (5)")
            {
                clonedArrow1.GetComponent<CanvasGroup>().DOFade(0, 0.2f);
            }
            else if (this.name == "Dice (6)")
            {
                clonedArrow2.GetComponent<CanvasGroup>().DOFade(0, 0.2f);
            }

            if (diceController.isDicesPickRight(diceNames, 3))
            {
                DestroyImmediate(clonedArrow1);
                DestroyImmediate(clonedArrow2);
                Block pickedBlock = blockController.GetOneBlock(Constants.TYPE.LEFT_MIDDLE_BLOCK);
                newTutorialController.MoveArrowToBlock(pickedBlock);
            }            
        }

        // if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.TUTORIAL) {
        //     if (TutorialController.GetTutorialCount() == 4) {
        //         TextTyperTester.Jump();
        //         TutorialController.AllowClickEventNextButton();
        //         TutorialController.ControllArrowUI();
        //     }
        //     if (TutorialController.GetTutorialCount() == 5) {
        //         // TutorialController.ToggleDiceArrow();
        //         TutorialController.ToggleDiceArrow();
        //     }
        //     if (TutorialController.GetTutorialCount() == 8) {
        //         TutorialController.ToggleClonedArrow(int.Parse(this.name.Split(' ')[1]) - 1);
        //     }
        //     if (TutorialController.GetTutorialCount() == 10) {
        //         TutorialController.Jump(false);
        //         TutorialController.ControllArrowUI();
        //     }
        //     if (TutorialController.GetTutorialCount() == 11) {
        //         TutorialController.ToggleCanvasBody(1);
        //     }
        // }
    }

    public void ToggleDice()
    {
        if (isClicked == true)
        {
            UnClickDice();
        }
        else
        {
            ClickDice();
        }
    }

    public void ClickDice()
    {
        var attackGageDisplay = FindObjectOfType<AttackGageDisplay>();
        if (!isDestroyed) {
            attackGageDisplay.AddGageNum(int.Parse(diceText.text));
            diceAnimator.SetTrigger("isClicked");
            isClicked = true;
            if (EffectSoundController.instance != null)
                EffectSoundController.instance.PlaySoundByName(EffectSoundController.SOUND_NAME.CLICK_DICE);
        }
    }

    public void UnClickDice()
    {
        if (isClicked)
        {
            var attackGageDisplay = FindObjectOfType<AttackGageDisplay>();
            attackGageDisplay.SpendGageNum(int.Parse(diceText.text));
            this.transform.GetChild(0).GetComponent<Image>().sprite = unclickedDiceImage;
            ResetAnimation();

            isClicked = false;
            if (EffectSoundController.instance != null)
                EffectSoundController.instance.PlaySoundByName(EffectSoundController.SOUND_NAME.CLICK_DICE);
        }
    }

    public void DestoryDice()
    {
        ReduceAttackGage();
        HideDice();
    }

    private void ReduceAttackGage()
    {
        var attackGageDisplay = FindObjectOfType<AttackGageDisplay>();
        attackGageDisplay.SpendGageNum(int.Parse(diceText.text));
    }

    private void HideDice()
    {
        ResetAnimation();
        GetComponentInChildren<Image>().raycastTarget = false;
        GetComponentInChildren<Image>().color = Color.clear;
        GetComponentInChildren<Text>().text = "0";
        GetComponentInChildren<Text>().color = Color.clear;
        isDestroyed = true;
    }

    public void ResetDice()
    {
        if (TutorialController.GetTutorialCount() != 13) {
            GetComponentInChildren<Image>().raycastTarget = true;
        }        
        GetComponentInChildren<Image>().color = Color.white;
        GetComponentInChildren<Text>().color = Color.black;
        SetDiceNumber(minNumber, maxNumber);
        SetDiceRollAnimation();

        isDestroyed = false;
    }

    public void ResetDice(int timesNumber)
    {
        GetComponentInChildren<Image>().raycastTarget = true;
        GetComponentInChildren<Image>().color = Color.white;
        GetComponentInChildren<Text>().color = Color.black;
        SetDiceNumber(minNumber, maxNumber);
        diceText.text = (GetCurrentNumber() * timesNumber).ToString();
        isDestroyed = false;
        SetDiceRollAnimation();
        
        FindObjectOfType<NoDiceNoCoinController>().ToggleScreen();
    }

    private void SetDiceRollAnimation()
    {
        diceText.GetComponent<CanvasGroup>().DOFade(0, 0);
        diceAnimator.SetTrigger("isAnimated");
        Invoke("ResetDiceRollAnimation", 0.15f);
    }

    private void ResetDiceRollAnimation()
    {
        diceAnimator.ResetTrigger("isAnimated");
        diceText.GetComponent<CanvasGroup>().DOFade(1, 0);
    }

    public void SetDiceNumber(int startNumber, int endNumber)
    {
        int randomNumber =  Random.Range(startNumber, endNumber);
        diceText = GetComponentInChildren<Text>();
        diceText.text = randomNumber.ToString();
    }

    public void SetTutorialDiceNumber()
    {
        int tutorialCount = TutorialController.GetTutorialCount();
        int diceNumber = 1;
        // if (tutorialCount == 3)
        // {
        //     diceNumber = 1;
        // }

        // if (tutorialCount == 5)
        // {
        //     diceNumber = 6;
        // }
        switch(this.name)
        {
            case "Dice (1)": 
            {
                diceNumber = 4;
                break;
            }
            case "Dice (2)": 
            {
                diceNumber = 5;
                break;
            }
            case "Dice (3)": 
            {
                diceNumber = 3;
                break;
            }
            case "Dice (4)": 
            {
                diceNumber = 3;
                break;
            }
            case "Dice (5)": 
            {
                diceNumber = 6;
                break;
            }
            case "Dice (6)": 
            {
                diceNumber = 2;
                break;
            }
        }

        diceText.text = diceNumber.ToString();
    }

    public void ResetAnimation()
    {
        this.transform.GetChild(0).GetComponent<Image>().sprite = unclickedDiceImage;
        diceAnimator.ResetTrigger("isClicked");
        isClicked = false;
    }

    public bool CheckIsClicked()
    {
        return isClicked; 
    }

    public bool IsDestroyed()
    {
        return isDestroyed;
    }

    public void PowerUpDice(int powerUpGage)
    {
        diceText.text = (int.Parse(diceText.text) + powerUpGage).ToString();
    }

    public void AddMaxNumber()
    {
        maxNumber++;
    }

    public int GetCurrentNumber() 
    {
        return int.Parse(diceText.text);
    }

    public int GetMaxNumber()
    {
        return maxNumber;
    }

    public void SetClickSound() {
        isSound = PlayerPrefs.GetInt("sound") == 1 ? true : false;
        clickSound = GetComponentInChildren<AudioSource>();
        clickSound.Stop();
    }

    public static void ToggleSound(bool value) {
        isSound = value;
    }

    // 특수 주사위
    public void EffectMaldivesDice()
    {
        diceText.text = "8";
    }

    public void ToggleAllowClick(bool isAllow)
    {
        var button = this.transform.Find(Constants.GAME_OBJECT_NAME.IMAGE).GetComponent<Button>();
        button.interactable = isAllow;
    }
}

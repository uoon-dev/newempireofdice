using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using DG.Tweening;

public class NewTutorialController : MonoBehaviour
{
    TutorialDialogueController tutorialDialogueController; 
    BlockController blockController;
    DiceController diceController;
    ResetDiceController resetDiceController;
    [SerializeField]
    GameObject tutorialGuideCanvas;
    [SerializeField]
    Sprite[] guiderImages;
    Transform pannelSetting;
    GameObject introCanvas;
    GameObject attackGage;
    GameObject leftArea;
    GameObject rightArea;

    GameObject MainDialogueContainer;
    GameObject SubDialogueContainer;
    GameObject turn;
    GameObject guideItem;
    GameObject toast;
    GameObject oval;
    GameObject arrow;
    GameObject outline;
    GameObject outlineDice;
    GameObject outlineCircle;
    GameObject outlineRect;
    GameObject outlineFullRect;
    GameObject blocks;
    Transform subTextBox;
    Transform subSuperText;
    Transform subGuiderImage;
    public bool dialogueUpdated = false;
    public bool isOver = true; // turn이 초과된 경우

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    void Update()
    {
        if (tutorialDialogueController != null && dialogueUpdated)
        {
            int dialogueTurn = TutorialDialogueController.dialogueTurn;
            switch (dialogueTurn)
            {
                case 2: 
                {
                    var lastBlock = blockController.GetOneBlock(Constants.TYPE.LAST_BLOCK);
                    var lastBlockPosition = lastBlock.transform.position;
                    var dialogues = FindObjectsOfType<TutorialDialogueController>();
                    MainDialogueContainer.GetComponent<CanvasGroup>().DOFade(0, 0.25f)
                        .OnComplete(() => {
                            dialogues[0].lines[0] = "<d=5>주군, 이 정도면 수업은 충분한 것 같습니다! 이제 주군의 지혜와 강력한 주사위로 마왕성을 무찔러주십시오!";
                            MainDialogueContainer.SetActive(false);
                            SubDialogueContainer.transform.DOMove(new Vector2(1.7f * lastBlockPosition.x, lastBlockPosition.y - lastBlockPosition.y / 5), 0.15f);
                            SubDialogueContainer.SetActive(true);

                            if (isOver)
                            {
                                TutorialDialogueController.dialogueTurn = dialogueTurn - 1;
                                isOver = false;
                            }

                            lastBlock.GetComponent<Canvas>().overrideSorting = true;
                            lastBlock.GetComponent<Canvas>().sortingOrder = 102;
                            
                            oval.transform.position =
                                new Vector2(lastBlock.transform.position.x, lastBlock.transform.position.y + 10);
                            oval.GetComponent<Rigidbody2D>().DORotate(360, 10).SetLoops(-1, LoopType.Restart);
                            oval.GetComponent<CanvasGroup>().DOFade(1, 0.2f);
                        });

                    break;
                }
                case 3:
                {
                    var lastBlock = blockController.GetOneBlock(Constants.TYPE.LAST_BLOCK);
                    var rightAreaPosition = rightArea.transform.position;

                    lastBlock.GetComponent<Canvas>().overrideSorting = false;
                    lastBlock.GetComponent<Canvas>().sortingOrder = 5;
                    oval.GetComponent<CanvasGroup>().DOFade(0, 0.2f);
                    TutorialDialogueController.isClickable = false;

                    // subTextBox.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 0);

                    Sequence sequence = DOTween.Sequence();
                    sequence.Append(subGuiderImage.GetComponent<CanvasGroup>().DOFade(0, 0.1f));
                    sequence.AppendCallback(() => {
                        subGuiderImage.transform.localScale = new Vector2(1, 1);
                        subSuperText.GetComponent<SuperTextMesh>().baseOffset = new Vector2(3.6f, 0);
                    });
                    sequence.Append(subGuiderImage.transform.DOLocalMove(new Vector2(-59.7f, 27.7f), 0));
                    sequence.Join(subSuperText.GetComponent<LayoutElement>().DOMinSize(new Vector2(180f, 0), 0.2f));
                    sequence.Join(SubDialogueContainer.transform.DOMove(new Vector2(rightAreaPosition.x * 0.9f, rightAreaPosition.y * 0.9f), 0.3f));
                    sequence.AppendInterval(0.1f);
                    sequence.Append(subGuiderImage.GetComponent<CanvasGroup>().DOFade(1, 0.1f));
                    sequence.AppendCallback(() => {
                        diceController.BounceDices();
                        attackGage.GetComponent<Canvas>().overrideSorting = true;
                        attackGage.GetComponent<Canvas>().sortingOrder = 102;
                    });
                    sequence.AppendInterval(1f);
                    sequence.AppendCallback(() => {
                        subTextBox.GetComponent<Button>().interactable = true;
                    });
                    sequence.Play(); 
                    break;
                }
                case 4: 
                {
                    var middleBlock = blockController.GetOneBlock(Constants.TYPE.MIDDLE_BLOCK);
                    var lastBlock = blockController.GetOneBlock(Constants.TYPE.LAST_BLOCK);
                    var lastBlockPosition = lastBlock.transform.position;
                    var dicePosition = middleBlock.transform.position;

                    subSuperText.GetComponent<LayoutElement>().minWidth = 280;
                    subSuperText.gameObject.SetActive(false);
                    subSuperText.gameObject.SetActive(true);
                    
                    // for layout update..
                    subTextBox.GetComponent<Button>().interactable = false;
                    attackGage.GetComponent<Canvas>().overrideSorting = false;
                    attackGage.GetComponent<Canvas>().sortingOrder = 6;
                    diceController.UnbounceDices();

                    leftArea.GetComponent<Canvas>().overrideSorting = true;
                    leftArea.GetComponent<Canvas>().sortingOrder = 102;
                    outline.transform.position = 
                        new Vector2(middleBlock.transform.position.x - 5, middleBlock.transform.position.y - 5);

                    TutorialDialogueController.isClickable = true;
                    subTextBox.GetComponent<VerticalLayoutGroup>().padding.right = 75;

                    Sequence sequence = DOTween.Sequence();
                    sequence.Append(subGuiderImage.GetComponent<CanvasGroup>().DOFade(0, 0.2f));
                    sequence.AppendCallback(() => {
                        outline.GetComponent<CanvasGroup>().DOFade(1, 0.4f);
                        subGuiderImage.transform.localScale = new Vector2(-1, 1);
                        subSuperText.GetComponent<SuperTextMesh>().baseOffset = new Vector2(2f, 0);
                    });
                    sequence.Append(subGuiderImage.transform.DOLocalMove(new Vector2(213.76f, 30.6f), 0));
                    sequence.Join(subSuperText.GetComponent<LayoutElement>().DOMinSize(new Vector2(280f, 0), 0.2f));
                    sequence.Join(SubDialogueContainer.transform.DOMove(new Vector2((Screen.width < 1000 ? 1.75f : 1.85f) * lastBlockPosition.x, dicePosition.y), 0.15f));
                    // sequence.Join(SubDialogueContainer.transform.DOLocalMove(new Vector2(128f, 78.5f), 0.15f));
                    sequence.AppendInterval(0.1f);
                    sequence.Append(subGuiderImage.GetComponent<CanvasGroup>().DOFade(1, 0.1f));
                    sequence.Play(); 

                    break;
                }
                case 5: 
                {
                    subTextBox.DOScaleY(0.7f, 0);
                    subSuperText.DOScaleY(1.4f, 0);
                    subGuiderImage.DOLocalMoveY(19.5f, 0.3f);
                    outline.GetComponent<CanvasGroup>().DOFade(0, 0.2f);

                    break;
                }
                case 7:
                {
                    GameObject toast = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.TOAST);
                    Sequence sequence = DOTween.Sequence();
                    var fisrtDice = diceController.GetOneDice("Dice (1)");
                    var dicePosition = fisrtDice.transform.position;
                    TutorialDialogueController.isClickable = false;
                    tutorialGuideCanvas.GetComponent<CanvasGroup>().interactable = false;
                    tutorialGuideCanvas.GetComponent<CanvasGroup>().blocksRaycasts = false;
                    
                    diceController.ToggleDicesClick(false);
                    diceController.ToggleOneDiceClick("Dice (1)", true);

                    arrow.transform.DOMove(new Vector3(
                        dicePosition.x - dicePosition.x / 15, 
                        dicePosition.y + dicePosition.y / 5, 
                        dicePosition.z), 0);
                    // arrow.transform.DOMove(new Vector3(
                    //     dicePosition.x - (fisrtDice.GetComponent<RectTransform>().rect.width - 10) * Camera.main.aspect, 
                    //     dicePosition.y + (fisrtDice.GetComponent<RectTransform>().rect.width + 10) * Camera.main.aspect, 
                    //     dicePosition.z), 0);
                    pannelSetting = tutorialGuideCanvas.transform.Find(Constants.GAME_OBJECT_NAME.PANEL_SETTING);
                    
                    sequence.Append(SubDialogueContainer.GetComponent<CanvasGroup>().DOFade(0, 0.2f));
                    sequence.AppendCallback(() => {
                        pannelSetting.gameObject.SetActive(false);
                        attackGage.GetComponent<Canvas>().overrideSorting = true;
                        attackGage.GetComponent<Canvas>().sortingOrder = 102;
                        arrow.SetActive(true);
                        arrow.GetComponent<CanvasGroup>().DOFade(1, 0.2f);
                    });
                    sequence.AppendInterval(0.2f);
                    sequence.Append(toast.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
                    sequence.AppendCallback(() => {
                        arrow.transform.DOMove(new Vector3(
                            dicePosition.x - dicePosition.x / 15 - 10, 
                            dicePosition.y + dicePosition.y / 5 + 10,
                            dicePosition.z), 0.3f).SetLoops(-1, LoopType.Yoyo);
                    });
                    sequence.Play();
                    break;
                }
                case 8:
                {
                    subGuiderImage.GetComponent<Image>().sprite = guiderImages[2];

                    SubDialogueContainer.GetComponent<CanvasGroup>().DOFade(1, 0.2f);
                    pannelSetting.gameObject.SetActive(true);
                    arrow.GetComponent<CanvasGroup>().DOFade(0, 0.2f);
                    toast.GetComponent<CanvasGroup>().DOFade(0, 0.2f);
                    
                    attackGage.GetComponent<Canvas>().overrideSorting = false;
                    attackGage.GetComponent<Canvas>().sortingOrder = 6;
                    turn.GetComponent<Canvas>().sortingOrder = 102;
                    
                    TutorialDialogueController.isClickable = true;

                    break;
                }
                case 9:
                {
                    subGuiderImage.GetComponent<Image>().sprite = guiderImages[4];
                    var lastBlock = blockController.GetOneBlock(Constants.TYPE.LAST_BLOCK);

                    outlineCircle.transform.position =
                        new Vector2(lastBlock.transform.position.x, lastBlock.transform.position.y + 10);
                    outlineCircle.GetComponent<Rigidbody2D>().DORotate(360, 10).SetLoops(-1, LoopType.Restart);
                    outlineCircle.GetComponent<CanvasGroup>().DOFade(1, 0.2f);

                    outlineRect.transform.position =
                        new Vector2(turn.transform.position.x + turn.transform.position.x / 70, turn.transform.position.y + 5);
                    outlineRect.GetComponent<CanvasGroup>().DOFade(1, 0.2f);

                    subGuiderImage.transform.DOLocalMove(new Vector2(225.5f, 28.2f), 0.2f);
                    subTextBox.GetComponent<VerticalLayoutGroup>().padding.bottom = 70;
                    subSuperText.GetComponent<LayoutElement>().DOMinSize(new Vector2(300f, 0), 0.2f);

                    break;
                }
                case 10:
                {
                    subGuiderImage.GetComponent<Image>().sprite = guiderImages[0];
                    subTextBox.GetComponent<VerticalLayoutGroup>().padding.bottom = 70;
                    outlineCircle.GetComponent<CanvasGroup>().DOFade(0, 0.2f);
                    outlineRect.GetComponent<CanvasGroup>().DOFade(0, 0.2f);
                    break;
                }
                case 11:
                {
                    var description = toast.transform.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.DESC);
                    var secondDice = diceController.GetOneDice("Dice (2)");
                    var secondDicePosition = secondDice.transform.position;

                    var thirdDice = diceController.GetOneDice("Dice (3)");
                    var thirdDicePosition = thirdDice.transform.position;
                    GameObject clonedArrow = Instantiate(arrow, transform.position, transform.rotation);

                    clonedArrow.transform.SetParent(guideItem.transform, false); 
                    pannelSetting.gameObject.SetActive(false);
                    description.GetComponent<Text>().text = "주사위를 잘 선택한 뒤 공격해보세요!";
                    Block fisrtBlock = blockController.GetOneBlock(Constants.TYPE.FIRST_BLOCK);
                    fisrtBlock.ToggleAllowClick(false);

                    DOTween.Kill(arrow.transform);
                    arrow.transform.DOMove(new Vector3(
                        secondDicePosition.x - secondDicePosition.x / 17, 
                        secondDicePosition.y + secondDicePosition.y / 5, 
                        secondDicePosition.z), 0);
                    arrow.GetComponent<CanvasGroup>().DOFade(1, 0.2f);

                    clonedArrow.transform.DOMove(new Vector3(
                        thirdDicePosition.x - thirdDicePosition.x / 17, 
                        thirdDicePosition.y + thirdDicePosition.y / 5, 
                        thirdDicePosition.z), 0);
                    clonedArrow.GetComponent<CanvasGroup>().DOFade(1, 0.2f);

                    SubDialogueContainer.GetComponent<CanvasGroup>().DOFade(0, 0.2f).OnComplete(() => {
                        toast.GetComponent<CanvasGroup>().DOFade(1, 0.2f);

                        arrow.transform.DOMove(new Vector3(
                            secondDicePosition.x - secondDicePosition.x / 17 - 10, 
                            secondDicePosition.y + secondDicePosition.y / 5 + 10,
                            secondDicePosition.z), 0.3f).SetLoops(-1, LoopType.Yoyo);

                        clonedArrow.transform.DOMove(new Vector3(
                            thirdDicePosition.x - thirdDicePosition.x / 17 - 10, 
                            thirdDicePosition.y + thirdDicePosition.y / 5 + 10,
                            thirdDicePosition.z), 0.3f).SetLoops(-1, LoopType.Yoyo);
                    });

                    diceController.ToggleOneDiceClick("Dice (2)", true);
                    diceController.ToggleOneDiceClick("Dice (3)", true);
                    break;
                }
                case 12:
                {
                    var costImage = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.COST_IMAGE);
                    var costText = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.MONEY_TEXT);
                    costImage.GetComponent<Canvas>().sortingOrder = 102;
                    costText.GetComponent<Canvas>().sortingOrder = 102;
                    
                    subGuiderImage.GetComponent<Image>().sprite = guiderImages[2];
                    SubDialogueContainer.GetComponent<CanvasGroup>().DOFade(1, 0.2f);
                    pannelSetting.gameObject.SetActive(true);
                    turn.GetComponent<Canvas>().sortingOrder = 5;
                    arrow.GetComponent<CanvasGroup>().DOFade(0, 0.2f);
                    toast.GetComponent<CanvasGroup>().DOFade(0, 0.2f);

                    subSuperText.GetComponent<LayoutElement>().minWidth = 250;
                    subSuperText.gameObject.SetActive(false);
                    subSuperText.gameObject.SetActive(true);
                    subSuperText.GetComponent<LayoutElement>().DOMinSize(new Vector2(250f, 0), 0.2f);
                    subGuiderImage.transform.DOLocalMove(new Vector2(197.4f, 27.7f), 0);

                    outlineRect.transform.position =
                        new Vector2(costImage.transform.position.x + costImage.transform.position.x / 50, costImage.transform.position.y);
                    outlineRect.GetComponent<CanvasGroup>().DOFade(1, 0.2f);

                    break;
                }
                case 13:
                {
                    var costImage = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.COST_IMAGE);
                    var costText = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.MONEY_TEXT);
                    subGuiderImage.GetComponent<Image>().sprite = guiderImages[0];
                    costImage.GetComponent<Canvas>().sortingOrder = 5;
                    costText.GetComponent<Canvas>().sortingOrder = 6;
                    leftArea.GetComponent<Canvas>().overrideSorting = false;
                    leftArea.GetComponent<Canvas>().sortingOrder = 5;                    
                    outlineRect.GetComponent<CanvasGroup>().DOFade(0, 0.2f);
                    break;
                }
                case 15:
                {
                    var description = toast.transform.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.DESC);
                    var fourDice = diceController.GetOneDice("Dice (4)");
                    var fourDicePosition = fourDice.transform.position;

                    var fiveDice = diceController.GetOneDice("Dice (5)");
                    var fiveDicePosition = fiveDice.transform.position;

                    var sixDice = diceController.GetOneDice("Dice (6)");
                    var sixDicePosition = sixDice.transform.position;
                    GameObject clonedArrow1 = Instantiate(arrow, transform.position, transform.rotation);
                    GameObject clonedArrow2 = Instantiate(arrow, transform.position, transform.rotation);

                    clonedArrow1.transform.SetParent(guideItem.transform, false); 
                    clonedArrow2.transform.SetParent(guideItem.transform, false);
                    clonedArrow2.name = clonedArrow2.name + "2";
                    pannelSetting.gameObject.SetActive(false);
                    description.GetComponent<Text>().text = "주사위를 잘 선택한 뒤 공격해보세요!";
                    Block fisrtBlock = blockController.GetOneBlock(Constants.TYPE.FIRST_BLOCK);
                    fisrtBlock.ToggleAllowClick(false);

                    DOTween.Kill(arrow.transform);
                    arrow.transform.DOMove(new Vector3(
                        fourDicePosition.x - fourDicePosition.x / 17, 
                        fourDicePosition.y + fourDicePosition.y / 4, 
                        fourDicePosition.z), 0);
                    arrow.GetComponent<CanvasGroup>().DOFade(1, 0.2f);

                    clonedArrow1.transform.DOMove(new Vector3(
                        fiveDicePosition.x - fiveDicePosition.x / 17, 
                        fiveDicePosition.y + fiveDicePosition.y / 4, 
                        fiveDicePosition.z), 0);
                    clonedArrow1.GetComponent<CanvasGroup>().DOFade(1, 0.2f);

                    clonedArrow2.transform.DOMove(new Vector3(
                        sixDicePosition.x - sixDicePosition.x / 17, 
                        sixDicePosition.y + sixDicePosition.y / 4, 
                        sixDicePosition.z), 0);
                    clonedArrow2.GetComponent<CanvasGroup>().DOFade(1, 0.2f);

                    SubDialogueContainer.GetComponent<CanvasGroup>().DOFade(0, 0.2f).OnComplete(() => {
                        TutorialDialogueController.isClickable = false;
                        toast.GetComponent<CanvasGroup>().DOFade(1, 0.2f);

                        arrow.transform.DOMove(new Vector3(
                            fourDicePosition.x - fourDicePosition.x / 17 - 10, 
                            fourDicePosition.y + fourDicePosition.y / 4 + 10,
                            fourDicePosition.z), 0.3f).SetLoops(-1, LoopType.Yoyo);

                        clonedArrow1.transform.DOMove(new Vector3(
                            fiveDicePosition.x - fiveDicePosition.x / 17 - 10, 
                            fiveDicePosition.y + fiveDicePosition.y / 4 + 10,
                            fiveDicePosition.z), 0.3f).SetLoops(-1, LoopType.Yoyo);

                        clonedArrow2.transform.DOMove(new Vector3(
                            sixDicePosition.x - sixDicePosition.x / 17 - 10, 
                            sixDicePosition.y + sixDicePosition.y / 4 + 10,
                            sixDicePosition.z), 0.3f).SetLoops(-1, LoopType.Yoyo);
                    });

                    diceController.ToggleOneDiceClick("Dice (4)", true);
                    diceController.ToggleOneDiceClick("Dice (5)", true);
                    diceController.ToggleOneDiceClick("Dice (6)", true);

                    blockController.GetOneBlock(Constants.TYPE.BOTTOM_MIDDLE_BLOCK).ToggleAllowClick(false);
                    blockController.GetOneBlock(Constants.TYPE.LEFT_MIDDLE_BLOCK).ToggleAllowClick(false);
                    break;
                }
                case 16:
                {
                    SubDialogueContainer.GetComponent<CanvasGroup>().DOFade(1, 0.2f);
                    toast.GetComponent<CanvasGroup>().DOFade(0, 0.2f);
                    leftArea.GetComponent<Canvas>().overrideSorting = true;
                    leftArea.GetComponent<Canvas>().sortingOrder = 102;
                    pannelSetting.gameObject.SetActive(true);
                    subGuiderImage.GetComponent<Image>().sprite = guiderImages[2];
                    TutorialDialogueController.isClickable = true;                    
                    break;
                }
                case 17:
                {
                    Sequence sequence = DOTween.Sequence();
                    var sixDice = diceController.GetOneDice("Dice (3)");
                    var dicePosition = sixDice.transform.position;
                    var rightAreaPosition = rightArea.transform.position;
                    blockController.GetOneBlock(Constants.TYPE.BOTTOM_MIDDLE_BLOCK).ToggleAllowClick(true);
                    leftArea.GetComponent<Canvas>().sortingOrder = 5;
                                    
                    sequence.Append(subGuiderImage.GetComponent<CanvasGroup>().DOFade(0, 0.1f));
                    sequence.AppendCallback(() => {
                        subGuiderImage.GetComponent<Image>().sprite = guiderImages[0];
                        subGuiderImage.transform.localScale = new Vector2(1, 1);
                        subSuperText.GetComponent<SuperTextMesh>().baseOffset = new Vector2(3.6f, 0);
                    });
                    sequence.Append(subGuiderImage.transform.DOLocalMove(new Vector2(-75.6f, 27.7f), 0));
                    sequence.Join(subSuperText.GetComponent<LayoutElement>().DOMinSize(new Vector2(220f, 0), 0.2f));
                    sequence.Join(SubDialogueContainer.transform.DOMove(new Vector2(rightAreaPosition.x * 1.05f, rightAreaPosition.y * 0.9f), 0.3f));
                    sequence.AppendInterval(0.1f);
                    sequence.Append(subGuiderImage.GetComponent<CanvasGroup>().DOFade(1, 0.1f));
                    sequence.AppendCallback(() => {
                        diceController.BounceDices();
                        attackGage.GetComponent<Canvas>().overrideSorting = true;
                        attackGage.GetComponent<Canvas>().sortingOrder = 102;
                        resetDiceController.ResetOneDice();
                        outlineFullRect.transform.position = 
                            new Vector2(dicePosition.x, dicePosition.y);
                        outlineFullRect.GetComponent<CanvasGroup>().DOFade(1, 0.1f);                        
                    });
                    sequence.AppendInterval(1f);
                    sequence.AppendCallback(() => {
                        subTextBox.GetComponent<Button>().interactable = true;
                    });
                    sequence.Play();

                    break;
                }
                case 18:
                {
                    DOTween.Kill(subGuiderImage.transform);
                    subGuiderImage.GetComponent<Image>().sprite = guiderImages[3];
                    subGuiderImage.transform.DOLocalMove(new Vector3(-102.97f, 33.2f, 1), 0.2f);
                    subSuperText.GetComponent<LayoutElement>().DOMinSize(new Vector2(270f, 0), 0.2f);
                    outlineFullRect.GetComponent<CanvasGroup>().DOFade(0, 0.1f);
                    break;
                }
                case 19:
                {
                    Sequence sequence = DOTween.Sequence();
                    var moneyArea = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.MONEY_AREA);
                    var costIcon = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.COST_ICON);
                    var costText = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.COST_TEXT);
                    var costImage = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.COST_IMAGE);
                    var moneyText = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.MONEY_TEXT);
                    var rightAreaPosition = rightArea.transform.position;
                    
                    moneyArea.GetComponent<Canvas>().sortingOrder = 102;
                    moneyText.GetComponent<Canvas>().sortingOrder = 102;
                    costImage.GetComponent<Canvas>().sortingOrder = 102;
                    costIcon.GetComponent<Canvas>().sortingOrder = 103;
                    costText.GetComponent<Canvas>().sortingOrder = 103;

                    ToggleClickEventResetDiceScreen(true);
                    TutorialDialogueController.isClickable = false;

                    DOTween.Kill(arrow.transform);
                    arrow.transform.DOMove(new Vector3(
                        moneyArea.transform.position.x - moneyArea.transform.position.x / 15,
                        moneyArea.transform.position.y + moneyArea.transform.position.y / 1.5f, 
                        moneyArea.transform.position.z), 0);

                    subGuiderImage.GetComponent<Image>().sprite = guiderImages[0];
                    sequence.Append(subGuiderImage.transform.DOLocalMove(new Vector2(-117.48f, 38.4f), 0.2f));
                    sequence.Join(subSuperText.GetComponent<LayoutElement>().DOMinSize(new Vector2(300f, 0), 0.2f));
                    sequence.Join(SubDialogueContainer.transform.DOMove(new Vector2(rightAreaPosition.x, rightAreaPosition.y * 0.9f), 0.25f));
                    sequence.Append(arrow.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
                    sequence.AppendCallback(() => {
                        sequence.Append(arrow.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
                        arrow.transform.DOMove(new Vector3(
                            moneyArea.transform.position.x - moneyArea.transform.position.x / 15 - 10, 
                            moneyArea.transform.position.y + moneyArea.transform.position.y / 1.5f + 10, 
                            moneyArea.transform.position.z), 0.3f).SetLoops(-1, LoopType.Yoyo);            
                    });

                    break;
                }
                case 20:
                {
                    Sequence sequence = DOTween.Sequence();
                    var moneyArea = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.MONEY_AREA);
                    var costIcon = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.COST_ICON);
                    var costText = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.COST_TEXT);
                    var costImage = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.COST_IMAGE);
                    var moneyText = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.MONEY_TEXT);
                    var attackPowerText = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.ATTACK_POWER_TEXT);
                    var attackPowerImage = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.ATTACK_POWER_IMAGE);
                    var dicePosition = diceController.GetOneDice("Dice (2)").transform.position;
                    var rightAreaPosition = rightArea.transform.position;
                    
                    moneyArea.GetComponent<Canvas>().sortingOrder = 5;
                    moneyText.GetComponent<Canvas>().sortingOrder = 6;
                    costImage.GetComponent<Canvas>().sortingOrder = 6;
                    costIcon.GetComponent<Canvas>().sortingOrder = 6;
                    costText.GetComponent<Canvas>().sortingOrder = 6;
                    TutorialDialogueController.isClickable = true;

                    outlineDice.transform.DOMove(new Vector3(dicePosition.x, dicePosition.y - dicePosition.y / 7, 1), 0);
                    outlineRect.transform.DOMove(new Vector3(attackPowerText.transform.position.x - attackPowerText.transform.position.x / 60, attackPowerText.transform.position.y, 1), 0);

                    sequence.Append(subGuiderImage.GetComponent<CanvasGroup>().DOFade(0, 0.1f));
                    sequence.AppendCallback(() => {
                        subGuiderImage.transform.localScale = new Vector2(1, 1);
                        subSuperText.GetComponent<SuperTextMesh>().baseOffset = new Vector2(3.6f, 0);
                        subSuperText.GetComponent<LayoutElement>().minWidth = 270;
                        subSuperText.gameObject.SetActive(false);
                        subSuperText.gameObject.SetActive(true);
                        arrow.GetComponent<CanvasGroup>().DOFade(0, 0.15f);
                   });
                    sequence.AppendInterval(0.1f);
                    sequence.AppendCallback(() => {
                        resetDiceController.ResetDices();
                    });
                    sequence.Append(subGuiderImage.transform.DOLocalMove(new Vector2(-103.8f, 45f), 0));
                    sequence.Join(subSuperText.GetComponent<LayoutElement>().DOMinSize(new Vector2(270f, 0), 0.2f));
                    sequence.Join(SubDialogueContainer.transform.DOMove(new Vector2(rightAreaPosition.x * 0.7f, rightAreaPosition.y * 0.9f), 0.3f));
                    sequence.AppendInterval(0.1f);
                    sequence.Append(subGuiderImage.GetComponent<CanvasGroup>().DOFade(1, 0.1f));
                    sequence.AppendCallback(() => {
                        diceController.BounceDices();
                        attackGage.GetComponent<Canvas>().overrideSorting = true;
                        attackGage.GetComponent<Canvas>().sortingOrder = 102;
                        attackPowerText.GetComponent<Canvas>().sortingOrder = 102;
                        attackPowerImage.GetComponent<Canvas>().sortingOrder = 102;                        
                        outlineDice.GetComponent<CanvasGroup>().DOFade(1, 0.15f);
                        outlineRect.GetComponent<CanvasGroup>().DOFade(1, 0.15f);
                    });
                    sequence.AppendInterval(1f);
                    sequence.AppendCallback(() => {
                        subTextBox.GetComponent<Button>().interactable = true;
                    });
                    sequence.Play();                     
                    break;
                }
                case 21:
                {
                    var attackPowerText = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.ATTACK_POWER_TEXT);
                    var attackPowerImage = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.ATTACK_POWER_IMAGE);
                    var rightAreaPosition = rightArea.transform.position;
                    Sequence sequence = DOTween.Sequence();

                    attackGage.GetComponent<Canvas>().overrideSorting = false;
                    attackGage.GetComponent<Canvas>().sortingOrder = 5;
                    attackPowerText.GetComponent<Canvas>().sortingOrder = 5;
                    attackPowerImage.GetComponent<Canvas>().sortingOrder = 5;

                    diceController.UnbounceDices();
                    leftArea.GetComponent<Canvas>().sortingOrder = 102;
                    turn.GetComponent<Canvas>().sortingOrder = 102;

                    sequence.Append(outlineDice.GetComponent<CanvasGroup>().DOFade(0, 0.15f));
                    sequence.Join(outlineRect.GetComponent<CanvasGroup>().DOFade(0, 0.15f));
                    sequence.Append(subGuiderImage.GetComponent<CanvasGroup>().DOFade(0, 0.1f));
                    sequence.AppendCallback(() => {
                        subGuiderImage.GetComponent<Image>().sprite = guiderImages[4];                        
                        subGuiderImage.transform.localScale = new Vector2(-1, 1);
                        subSuperText.GetComponent<SuperTextMesh>().baseOffset = new Vector2(1.8f, 0);
                    });
                    sequence.Append(subGuiderImage.transform.DOLocalMove(new Vector2(188.27f, 23.2f), 0));
                    sequence.Join(subSuperText.GetComponent<LayoutElement>().DOMinSize(new Vector2(230f, 0), 0.2f));
                    sequence.Join(SubDialogueContainer.transform.DOMove(new Vector2(rightAreaPosition.x, rightAreaPosition.y * 0.9f), 0.3f));
                    sequence.AppendInterval(0.1f);
                    sequence.Append(subGuiderImage.GetComponent<CanvasGroup>().DOFade(1, 0.1f));
                    sequence.AppendCallback(() => {
                        var lastBlock = blockController.GetOneBlock(Constants.TYPE.LAST_BLOCK);

                        outlineCircle.transform.position =
                            new Vector2(lastBlock.transform.position.x, lastBlock.transform.position.y + 10);
                        outlineCircle.GetComponent<CanvasGroup>().DOFade(1, 0.2f);
                        outlineRect.transform.position =
                            new Vector2(turn.transform.position.x + turn.transform.position.x / 70, turn.transform.position.y + 5);
                        outlineRect.GetComponent<CanvasGroup>().DOFade(1, 0.2f);
                    });

                    sequence.Play();

                    break;
                }
                case 22:
                {
                    leftArea.GetComponent<Canvas>().sortingOrder = 5;
                    turn.GetComponent<Canvas>().sortingOrder = 5;
                    outlineCircle.GetComponent<CanvasGroup>().DOFade(0, 0.2f);
                    outlineRect.GetComponent<CanvasGroup>().DOFade(0, 0.2f);                            
                    SubDialogueContainer.GetComponent<CanvasGroup>().DOFade(0, 0.25f)
                        .OnComplete(() => {
                            MainDialogueContainer.SetActive(true);
                            tutorialDialogueController.Apply();
                            MainDialogueContainer.GetComponent<CanvasGroup>().DOFade(1, 0.2f);
                            SubDialogueContainer.SetActive(false);
                        });
                    break;
                }
                case 24:
                {
                    MainDialogueContainer.GetComponent<CanvasGroup>().DOFade(0, 0.15f).OnComplete(() => {
                        diceController.ToggleDicesClick(true);
                        tutorialGuideCanvas.SetActive(false);
                    });
                    break;
                }                
            }

            dialogueUpdated = false;
        }   
    }

    private void Initialize()
    {
        TutorialDialogueController.dialogueTurn = 0;
        TutorialDialogueController.isClickable = true;

        blockController = FindObjectOfType<BlockController>();
        diceController = FindObjectOfType<DiceController>();
        resetDiceController = FindObjectOfType<ResetDiceController>();

        // tutorialGuideCanvas = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.TUTORIAL_GUIDE_CANVAS);
        introCanvas = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.INTRO_CANVAS);
        attackGage = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.ATTACK_GAGE);
        leftArea = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.LEFT_AREA);
        rightArea = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.RIGHT_AREA);
        blocks = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.BLOCKS);
        guideItem = Utils.FindInActiveObjectByName(Constants.TUTORIAL.GAME_OBJECT_NAME.GUIDE_ITEM);
        turn = Utils.FindInActiveObjectByName(Constants.TUTORIAL.GAME_OBJECT_NAME.TURN);
        toast = Utils.FindInActiveObjectByName(Constants.TUTORIAL.GAME_OBJECT_NAME.TOAST);
        MainDialogueContainer = Utils.FindInActiveObjectByName(Constants.TUTORIAL.GAME_OBJECT_NAME.MAIN_DIALOGUE_CONTAINER);
        SubDialogueContainer = Utils.FindInActiveObjectByName(Constants.TUTORIAL.GAME_OBJECT_NAME.SUB_DIALOGUE_CONTAINER);
        subTextBox = SubDialogueContainer.transform.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.TEXT_BOX);
        subGuiderImage = SubDialogueContainer.transform.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.GUIDER_IMAGE);
        subSuperText = subTextBox.transform.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.SUPER_TEXT);

        oval = Utils.FindInActiveObjectByName(Constants.TUTORIAL.GAME_OBJECT_NAME.OVAL);
        outline = Utils.FindInActiveObjectByName(Constants.TUTORIAL.GAME_OBJECT_NAME.OUTLINE);
        outlineDice = Utils.FindInActiveObjectByName(Constants.TUTORIAL.GAME_OBJECT_NAME.OUTLINE_DICE);
        outlineCircle = Utils.FindInActiveObjectByName(Constants.TUTORIAL.GAME_OBJECT_NAME.OUTLINE_CIRCLE);
        outlineRect = Utils.FindInActiveObjectByName(Constants.TUTORIAL.GAME_OBJECT_NAME.OUTLINE_RECT);
        outlineFullRect = Utils.FindInActiveObjectByName(Constants.TUTORIAL.GAME_OBJECT_NAME.OUTLINE_FULL_RECT);
        arrow = Utils.FindInActiveObjectByName(Constants.TUTORIAL.GAME_OBJECT_NAME.ARROW);

        ToggleClickEventResetDiceScreen(false);
    }

    public void ActiveDialogue()
    {
        introCanvas.SetActive(false);
        tutorialGuideCanvas.SetActive(true);
        tutorialDialogueController = FindObjectOfType<TutorialDialogueController>();
    }

    public void MoveArrowToBlock(Block block)
    {
        Sequence sequence = DOTween.Sequence();
        block.ToggleAllowClick(true);
        var blockPosition = block.transform.position;
        DOTween.Kill(arrow.transform);
        sequence.Append(arrow.GetComponent<CanvasGroup>().DOFade(0, 0.2f));
        sequence.AppendCallback(() => {
            arrow.transform.DOMove(new Vector3(
                blockPosition.x - blockPosition.x / 3,
                blockPosition.y + blockPosition.y / 5, 
                blockPosition.z), 0);
                
            sequence.Append(arrow.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
            arrow.transform.DOMove(new Vector3(
                blockPosition.x - blockPosition.x / 3 - 10, 
                blockPosition.y + blockPosition.y / 5 + 10, 
                blockPosition.z), 0.3f).SetLoops(-1, LoopType.Yoyo);            
        });
        sequence.Play();
    }

    public static void ToggleClickEventResetDiceScreen(bool isAllow) {
        var moneyArea = GameObject.Find("Money Area");
        moneyArea.GetComponent<Image>().raycastTarget = isAllow;
    }
}

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
    [SerializeField]
    GameObject tutorialGuideCanvas;
    GameObject introCanvas;
    GameObject attackGage;
    GameObject leftArea;

    GameObject MainDialogueContainer;
    GameObject SubDialogueContainer;
    GameObject oval;
    GameObject outline;
    GameObject blocks;
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
                    var lastBlock = blockController.GetLastBlock();
                    MainDialogueContainer.GetComponent<CanvasGroup>().DOFade(0, 0.25f)
                        .OnComplete(() => {
                            MainDialogueContainer.SetActive(false);
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
                            oval.SetActive(true);
                        });

                    break;
                }
                case 3:
                {
                    var lastBlock = blockController.GetLastBlock();
                    lastBlock.GetComponent<Canvas>().overrideSorting = false;
                    lastBlock.GetComponent<Canvas>().sortingOrder = 5;
                    oval.SetActive(false);
                    TutorialDialogueController.isClickable = false;

                    Transform textBox = SubDialogueContainer.transform.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.TEXT_BOX);
                    Transform guiderImage = SubDialogueContainer.transform.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.GUIDER_IMAGE);
                    Transform superText = textBox.transform.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.SUPER_TEXT);
                    // textBox.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 0);

                    Sequence sequence = DOTween.Sequence();
                    sequence.Append(guiderImage.GetComponent<CanvasGroup>().DOFade(0, 0.1f));
                    sequence.AppendCallback(() => {
                        guiderImage.transform.localScale = new Vector2(1, 1);
                        superText.GetComponent<SuperTextMesh>().baseOffset = new Vector2(3.6f, 0);
                    });
                    sequence.Append(guiderImage.transform.DOLocalMove(new Vector2(-59.7f, 27.7f), 0));
                    sequence.Join(superText.GetComponent<LayoutElement>().DOMinSize(new Vector2(180f, 0), 0.2f));
                    sequence.Join(SubDialogueContainer.transform.DOLocalMove(new Vector2(52, 68.6f), 0.3f));
                    sequence.AppendInterval(0.1f);
                    sequence.Append(guiderImage.GetComponent<CanvasGroup>().DOFade(1, 0.1f));
                    sequence.AppendCallback(() => {
                        diceController.BounceDices();
                        attackGage.GetComponent<Canvas>().overrideSorting = true;
                        attackGage.GetComponent<Canvas>().sortingOrder = 102;
                    });
                    sequence.AppendInterval(1f);
                    sequence.AppendCallback(() => {
                        textBox.GetComponent<Button>().interactable = true;
                    });
                    sequence.Play(); 
                    break;
                }
                case 4: 
                {
                    var middleBlock = blockController.GetMiddleBlock();
                    Transform textBox = SubDialogueContainer.transform.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.TEXT_BOX);
                    Transform guiderImage = SubDialogueContainer.transform.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.GUIDER_IMAGE);
                    Transform superText = textBox.transform.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.SUPER_TEXT);

                    superText.GetComponent<LayoutElement>().minWidth = 280;
                    superText.gameObject.SetActive(false);
                    superText.gameObject.SetActive(true);
                    
                    // for layout update..
                    textBox.GetComponent<Button>().interactable = false;
                    attackGage.GetComponent<Canvas>().overrideSorting = false;
                    attackGage.GetComponent<Canvas>().sortingOrder = 6;
                    diceController.UnbounceDices();

                    leftArea.GetComponent<Canvas>().overrideSorting = true;
                    leftArea.GetComponent<Canvas>().sortingOrder = 102;
                    outline.transform.position = 
                        new Vector2(middleBlock.transform.position.x - 5, middleBlock.transform.position.y - 5);
                    outline.SetActive(true);

                    TutorialDialogueController.isClickable = true;
                    textBox.GetComponent<VerticalLayoutGroup>().padding.right = 75;

                    Sequence sequence = DOTween.Sequence();
                    sequence.Append(guiderImage.GetComponent<CanvasGroup>().DOFade(0, 0.2f));
                    sequence.AppendCallback(() => {
                        guiderImage.transform.localScale = new Vector2(-1, 1);
                        superText.GetComponent<SuperTextMesh>().baseOffset = new Vector2(2f, 0);
                    });
                    sequence.Append(guiderImage.transform.DOLocalMove(new Vector2(218.6f, 30.6f), 0));
                    sequence.Join(superText.GetComponent<LayoutElement>().DOMinSize(new Vector2(280f, 0), 0.2f));
                    sequence.Join(SubDialogueContainer.transform.DOLocalMove(new Vector2(118f, 78.5f), 0.3f));
                    sequence.AppendInterval(0.1f);
                    sequence.Append(guiderImage.GetComponent<CanvasGroup>().DOFade(1, 0.1f));
                    sequence.Play(); 

                    break;
                }
            }

            dialogueUpdated = false;
        }   
    }

    private void Initialize()
    {
        blockController = FindObjectOfType<BlockController>();
        diceController = FindObjectOfType<DiceController>();

        // tutorialGuideCanvas = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.TUTORIAL_GUIDE_CANVAS);
        introCanvas = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.INTRO_CANVAS);
        attackGage = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.ATTACK_GAGE);
        leftArea = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.LEFT_AREA);
        blocks = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.BLOCKS);
        MainDialogueContainer = Utils.FindInActiveObjectByName(Constants.TUTORIAL.GAME_OBJECT_NAME.MAIN_DIALOGUE_CONTAINER);
        SubDialogueContainer = Utils.FindInActiveObjectByName(Constants.TUTORIAL.GAME_OBJECT_NAME.SUB_DIALOGUE_CONTAINER);
        oval = Utils.FindInActiveObjectByName(Constants.TUTORIAL.GAME_OBJECT_NAME.OVAL);
        outline = Utils.FindInActiveObjectByName(Constants.TUTORIAL.GAME_OBJECT_NAME.OUTLINE);
    }

    public void ActiveDialogue()
    {
        introCanvas.SetActive(false);
        tutorialGuideCanvas.SetActive(true);
        tutorialDialogueController = FindObjectOfType<TutorialDialogueController>();
    }
    
}

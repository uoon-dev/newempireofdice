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
    [SerializeField]
    GameObject tutorialGuideCanvas;
    GameObject introCanvas;

    GameObject MainDialogueContainer;
    GameObject SubDialogueContainer;
    GameObject oval;
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
                            Debug.Log(dialogueTurn);

                            lastBlock.GetComponent<Canvas>().overrideSorting = true;
                            lastBlock.GetComponent<Canvas>().sortingOrder = 102;
                            
                            oval.transform.position = 
                                new Vector2(lastBlock.transform.position.x, lastBlock.transform.position.y + 15);
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

                    // SubDialogueContainer.transform.localPosition = ;
                    SubDialogueContainer.transform.DOLocalMove(new Vector2(52, 68.6f), 0.5f);
                    Transform textBox = SubDialogueContainer.transform.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.TEXT_BOX);
                    textBox.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 0);

                    Transform guiderImage = SubDialogueContainer.transform.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.GUIDER_IMAGE);
                    guiderImage.GetComponent<CanvasGroup>().DOFade(0, 0.1f).OnComplete(() => {
                        guiderImage.transform.DOLocalMove(new Vector2(-59.7f, 27.7f), 0);
                        guiderImage.GetComponent<CanvasGroup>().DOFade(1, 0.2f).SetDelay(0.4f);
                        guiderImage.transform.localScale = new Vector2(1, 1);
                    });;
                    // guiderImage.transform.DOLocalMove(new Vector2(-59.7f, 27.7f), 0).SetDelay(0.1f).OnComplete(() => {
                    //     guiderImage.GetComponent<CanvasGroup>().DOFade(1, 0.2f);
                    // });

                    Transform superText = textBox.transform.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.SUPER_TEXT);
                    superText.GetComponent<SuperTextMesh>().baseOffset = new Vector2(3.6f, 0);
                    superText.GetComponent<LayoutElement>().DOMinSize(new Vector2(180f, 0), 0.25f);
                    break;
                }                
            }

            dialogueUpdated = false;
        }   
    }

    private void Initialize()
    {
        blockController = FindObjectOfType<BlockController>();

        // tutorialGuideCanvas = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.TUTORIAL_GUIDE_CANVAS);
        introCanvas = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.INTRO_CANVAS);
        MainDialogueContainer = Utils.FindInActiveObjectByName(Constants.TUTORIAL.GAME_OBJECT_NAME.MAIN_DIALOGUE_CONTAINER);
        SubDialogueContainer = Utils.FindInActiveObjectByName(Constants.TUTORIAL.GAME_OBJECT_NAME.SUB_DIALOGUE_CONTAINER);
        oval = Utils.FindInActiveObjectByName(Constants.TUTORIAL.GAME_OBJECT_NAME.OVAL);
    }

    public void ActiveDialogue()
    {
        introCanvas.SetActive(false);
        tutorialGuideCanvas.SetActive(true);
        tutorialDialogueController = FindObjectOfType<TutorialDialogueController>();
    }
    
}

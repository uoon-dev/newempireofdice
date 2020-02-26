using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class NewTutorialController : MonoBehaviour
{
    TutorialDialogueController tutorialDialogueController; 
    BlockController blockController;
    [SerializeField]
    GameObject tutorialGuideCanvas;
    GameObject introCanvas;

    GameObject oval;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    void Update()
    {
        if (tutorialDialogueController != null)
        {
            // int dialogueTurn = tutorialDialogueController.dialogueTurn;
            // Debug.Log(dialogueTurn +":dialogueTurn");
            // switch (dialogueTurn)
            // {
            //     case 2: 
            //     {
            //         var lastBlock = blockController.GetLastBlock();
            //         oval.transform.localPosition = lastBlock.transform.localPosition;
            //         oval.SetActive(true);
            //         break;
            //     }
            // }
        }   
    }

    private void Initialize()
    {
        blockController = FindObjectOfType<BlockController>();

        // tutorialGuideCanvas = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.TUTORIAL_GUIDE_CANVAS);
        introCanvas = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.INTRO_CANVAS);
        oval = Utils.FindInActiveObjectByName(Constants.TUTORIAL.GAME_OBJECT_NAME.OVAL);
    }

    public void ActiveDialogue()
    {
        introCanvas.SetActive(false);
        tutorialGuideCanvas.SetActive(true);
        tutorialDialogueController = FindObjectOfType<TutorialDialogueController>();
        tutorialDialogueController.isClickable = true;
    }
    
}

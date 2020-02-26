using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class NewTutorialController : MonoBehaviour
{
    TutorialDialogueController tutorialDialogueController; 
    [SerializeField]
    GameObject tutorialGuideCanvas;
    GameObject introCanvas;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        // tutorialGuideCanvas = GameObject.Find(Constants.GAME_OBJECT_NAME.TUTORIAL_GUIDE_CANVAS);
        introCanvas = GameObject.Find(Constants.GAME_OBJECT_NAME.INTRO_CANVAS);

    }

    public void ActiveDialogue()
    {
        introCanvas.SetActive(false);
        tutorialGuideCanvas.SetActive(true);
        // tutorialGuideCanvas.GetComponent<CanvasGroup>().alpha = 1;
        // tutorialGuideCanvas.GetComponent<CanvasGroup>().interactable = true;
        // tutorialGuideCanvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
        tutorialDialogueController = FindObjectOfType<TutorialDialogueController>();
        // tutorialDialogueController.Apply();
        tutorialDialogueController.isClickable = true;
    }
    
}

using UnityEngine;
using System.Collections;

public class TutorialDialogueController : MonoBehaviour
{
    public SuperTextMesh textMesh;
    public KeyCode advanceKey = KeyCode.Return;
    public SpriteRenderer advanceKeySprite;
    private Vector3 advanceKeyStartScale = Vector3.one;
    public Vector3 advanceKeyScale = Vector3.one;
    public float advanceKeyTime = 1f;
    public string[] lines;
    private int currentLine = 0;
    public bool isClickable = false;
    public static int dialogueTurn = 0;
    GameObject MainDialogueContainer;
    GameObject SubDialogueContainer;


    void Start()
    {   
        Initialize();
        // advanceKeyStartScale = advanceKeySprite.transform.localScale;
        Apply();
    }

    private void Initialize()
    {
        MainDialogueContainer = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.MAIN_DIALOGUE_CONTAINER);
        SubDialogueContainer = Utils.FindInActiveObjectByName(Constants.TUTORIAL.GAME_OBJECT_NAME.SUB_DIALOGUE_CONTAINER);
    }

    public void CompletedDrawing()
    {
        Debug.Log("I completed reading! Done!");
    }
    public void CompletedUnreading()
    {
        Debug.Log("I completed unreading!! Bye!");
        // if (dialogueTurn == 2)
        // {
        //     MainDialogueContainer.SetActive(false);
        //     SubDialogueContainer.SetActive(true);
        // }
        Apply(); //go to next line
    }
    public void Apply()
    {
        //isDoneFading = false;
        textMesh.Text = lines[currentLine]; //invoke accessor so rebuild() is called
        currentLine++; //move to next line of dialogue...
        currentLine %= lines.Length; //or loop back to first one
        // Debug.Log(dialogueTurn + ":dialogueTurn");
        Debug.Log(dialogueTurn + "@@@@@@@@@@@@@@@@@@");
        dialogueTurn++;
    }
    void Update()
    {
        // if (Input.GetKey(advanceKey))
        // {
        //     advanceKeySprite.transform.localScale = advanceKeyScale; //scale key if held
        // }
        // else
        // {
        //     advanceKeySprite.transform.localScale = Vector3.Lerp(advanceKeySprite.transform.localScale, advanceKeyStartScale, Time.deltaTime * advanceKeyTime);
        // }
        if (isClickable)
        {
            if (Input.GetKeyDown(advanceKey))
            {
                if (textMesh.reading)
                { //is text being read out, and player has lifted up the key in this block of text?
                    textMesh.SpeedRead(); //show all text, or speed up
                }
                if (!textMesh.reading && !textMesh.unreading)
                { //call Continue(), if no need to continue, advance to next box. only when button is pressed, too
                    if (!textMesh.Continue())
                    {
                        textMesh.UnRead();
                    }
                    else
                    {
                        Debug.Log("CONTINUING NOW");
                    }
                }
            }
            if (Input.GetKeyUp(advanceKey))
            {
                textMesh.RegularRead(); //return to normal reading speed, if possible.
            }
        }        
    }
}
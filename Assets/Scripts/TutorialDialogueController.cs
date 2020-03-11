using UnityEngine;
using UnityEngine.UI;
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
    public static bool isClickable = true;
    public static int dialogueTurn = 0;
    NewTutorialController newTutorialController;



    void Start()
    {   
        Initialize();
        // advanceKeyStartScale = advanceKeySprite.transform.localScale;
        Apply();
    }

    private void Initialize()
    {
        newTutorialController = FindObjectOfType<NewTutorialController>();
        // InitTextLines();
    }

    public void InitTextLines()
    {
        Debug.Log(dialogueTurn + ":dialogueTurn");
        if (dialogueTurn == 0)
        {
            lines[0] = "본격적으로 출발하기 전 <br>몇 가지를 알려드리겠습니다. <j>폐하.</j>";
            lines[1] = "<c=white>본격적으로 출발하기 전 <br>몇 가지를 알려드리겠습니다. <j>폐하.</j></c>";
        }
        else
        {
            lines[0] = "요 녀석은 마왕성입니다. 차근차근 땅을 넓혀 <br> 나가 <w=seasick><c=fire>마왕성을 점령하는 것</c></w>이 목표입니다!";
            lines[1] = "이것은 이것은 “주사위“ 입니다. <br> 마구 눌러보세요!";
        }
    }

    public void CompletedDrawing()
    {
        // Debug.Log("I completed reading! Done!");
    }
    public void CompletedUnreading()
    {
        Debug.Log("I completed unreading!! Bye!");
        Apply(); //go to next line
    }
    public void Apply()
    {
        //isDoneFading = false;
        if (currentLine < lines.Length)
        {
            if (lines[currentLine] != string.Empty) 
            {
                textMesh.Text = lines[currentLine]; //invoke accessor so rebuild() is called
                // if (dialogueTurn == 2)
                // {
                //     textMesh.Text = lines[19];
                //     currentLine--;
                //     dialogueTurn = 21;
                // }
            }
            currentLine++; //move to next line of dialogue...
            // currentLine %= lines.Length; //or loop back to first one
        }
        dialogueTurn++;
        // if (dialogueTurn == 22) dialogueTurn = 21;
        newTutorialController.dialogueUpdated = true;

        Debug.Log(dialogueTurn + ":dialogueTurn");
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
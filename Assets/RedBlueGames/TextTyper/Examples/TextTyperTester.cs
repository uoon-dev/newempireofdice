namespace RedBlueGames.Tools.TextTyper
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using RedBlueGames.Tools.TextTyper;
    using UnityEngine.UI;
    using TMPro;
    using Controllers.TutorialController;

    /// <summary>
    /// Class that tests TextTyper and shows how to interface with it.
    /// </summary>
    public class TextTyperTester : MonoBehaviour
    {
        [SerializeField]
        private AudioClip printSoundEffect;

        [Header("UI References")]

        [SerializeField]
        private Button printNextButton;

        [SerializeField]
        private Button printNoSkipButton;

        [SerializeField]
        private static GameObject tutorialCanvas;

        public static Queue<string> dialogueLines = new Queue<string>();

        [SerializeField]
        [Tooltip("The text typer element to test typing with")]
        private static TextTyper testTextTyper;

        public void Start()
        {
            dialogueLines.Clear();

            testTextTyper = FindObjectOfType<TextTyper>();
            testTextTyper.PrintCompleted.AddListener(this.HandlePrintCompleted);
            testTextTyper.CharacterPrinted.AddListener(this.HandleCharacterPrinted);

            this.printNextButton.onClick.AddListener(HandlePrintNextClicked);

            if (this.printNoSkipButton != null) {
                this.printNoSkipButton.onClick.AddListener(this.HandlePrintNoSkipClicked);
            }

            dialogueLines.Enqueue("안녕하세요 주군, 본격적으로 출발하기 전에 몇 가지를 알려드리겠습니다. 요녀석은 마왕성입니다. <delay=0.2></delay>차근차근 땅을 넓혀나가 여기까지 점령하는 것이 목표입니다!");
            dialogueLines.Enqueue("이것은 주사위 굴림 버튼입니다. 누르면 6개의 주사위를 굴립니다. 눌러보세요!");
            dialogueLines.Enqueue("잘 하셨습니다… <delay=0.2></delay><br>아니, 죄다 1만 나왔잖아요!!");
            dialogueLines.Enqueue("그래도 괜찮습니다… <delay=0.2></delay> 일단 주사위를 하나만 선택해보세요. <delay=0.2></delay> 선택한 주사위의 눈의 합은 우리의 공격력이 됩니다.");
            dialogueLines.Enqueue("이번엔 공격할 땅을 골라보세요.<delay=0.2></delay> 공격력이 땅에 적힌 방어력 이상이면 점령할 수 있습니다!");
            dialogueLines.Enqueue("축하드립니다! 첫 땅을 점령하셨군요. <delay=0.2></delay> 땅의 방어력에 딱 맞는 공격력으로 점령하는 것을 '딱뎀'이라고 합니다.");
            dialogueLines.Enqueue("'딱뎀'으로 땅을 점령하면 보너스 주사위 하나를 즉시 받습니다! <delay=0.5></delay>와! 6이 생겼네요!");
            dialogueLines.Enqueue("새로 받은 6을 가지고 영토를 더 확장해봅시다.<delay=0.2></delay> 이미 점령한 땅과 인접한 땅으로 점점 세력을 넓힐 수 있습니다.<delay=0.2></delay> <b>방금 우리가 점령한 땅 바로 위를 딱뎀으로 점령하세요!</b>");
            dialogueLines.Enqueue("정말 잘하셨습니다!<delay=0.2></delay> '딱뎀' 덕분에 주사위를 또 받았지만 한 개뿐이군요.");
            dialogueLines.Enqueue("꼭 딱뎀이 아니더라도 주사위는 다른 땅에 사용해둘 수도 있습니다. <delay=0.2></delay> 편리할 때도 있지만 그렇게 하면 턴이 소모되어 마왕성이 강해지게 됩니다. 그래도 한 번 해볼까요?");
            dialogueLines.Enqueue("비록 점령하진 못했지만 해당 땅의 방어력이 줄어든 것을 볼 수 있습니다.<delay=0.2></delay> 주사위를 굴리거나 사용할 때마다 턴이 소모되고, 마왕성의 방어력이 1씩 올라갑니다.");
            dialogueLines.Enqueue("자, 이제 주사위가 다 떨어졌으니 새로 굴려봅시다. 주사위에 표시된 5골드를 지불해 사용한 주사위들을 다시 굴릴 수 있습니다.");
            dialogueLines.Enqueue("새로 주사위 6개를 받았군요!<delay=0.2></delay> 골드는 딱뎀이든 아니든 땅을 점령하기만 하면 1씩 획득할 수 있어요.<delay=0.2></delay> 마왕성까지 가는 길은 험난하니, 실패하지 않으려면 이 골드 관리를 잘 해줘야 합니다.");
            dialogueLines.Enqueue("주군, 이 정도면 수업은 충분한 것 같습니다! 이제 주군의 지혜와 강력한 주사위로 마왕성을 무찔러주십시오!");
            // 하지만 보세요, 우리 최종 목표인 마왕성의 방어력이 증가했어요!<delay=0.2></delay>
            // dialogueLines.Enqueue("You can <b>use</b> <i>uGUI</i> <size=40>text</size> <size=20>tag</size> and <color=#ff0000ff>color</color> tag <color=#00ff00ff>like this</color>.");
            // dialogueLines.Enqueue("bold <b>text</b> test <b>bold</b> text <b>test</b>");
            // dialogueLines.Enqueue("You can <size=40>size 40</size> and <size=20>size 20</size>");
            // dialogueLines.Enqueue("You can <color=#ff0000ff>color</color> tag <color=#00ff00ff>like this</color>.");
            // dialogueLines.Enqueue("Sample Shake Animations: <anim=lightrot>Light Rotation</anim>, <anim=lightpos>Light Position</anim>, <anim=fullshake>Full Shake</anim>\nSample Curve Animations: <animation=slowsine>Slow Sine</animation>, <animation=bounce>Bounce Bounce</animation>, <animation=crazyflip>Crazy Flip</animation>");
            // ShowScript();
        }

        public void Update()
        {
            // if (Input.GetKeyDown(KeyCode.Space))
            // {

            //     var tag = RichTextTag.ParseNext("blah<color=red>boo</color");
            //     LogTag(tag);
            //     tag = RichTextTag.ParseNext("<color=blue>blue</color");
            //     LogTag(tag);
            //     tag = RichTextTag.ParseNext("No tag in here");
            //     LogTag(tag);
            //     tag = RichTextTag.ParseNext("No <color=blueblue</color tag here either");
            //     LogTag(tag);
            //     tag = RichTextTag.ParseNext("This tag is a closing tag </bold>");
            //     LogTag(tag);
            // }
        }

        public static void Jump() {
            HandlePrintNextClicked();
        }

        public static void HandlePrintNextClicked()
        {
            if (testTextTyper.IsSkippable() && testTextTyper.IsTyping)
            {
                testTextTyper.Skip();
            }
            else
            {
                if (dialogueLines.Count == 0)
                {                    
                    GameObject.Find("Tutorial Canvas").gameObject.SetActive(false);
                }
                ShowScript();
            }
        }

        private void HandlePrintNoSkipClicked()
        {
            ShowScript();
        }

        public static void ShowScript()
        {
            if (dialogueLines.Count <= 0)
            {
                return;
            }
            testTextTyper.TypeText(dialogueLines.Dequeue());
            if (dialogueLines.Count < 13) {
                ControllUIWhenScripting();
            }
        }

        public static void ControllUIWhenScripting() 
        {
            TutorialController.SetTutorialCount(TutorialController.GetTutorialCount() + 1);
            if (TutorialController.GetTutorialCount() == 7 || TutorialController.GetTutorialCount() == 8 || TutorialController.GetTutorialCount() == 9 || TutorialController.GetTutorialCount() == 10) 
            {
                TutorialController.Jump(true);
            }
            TutorialController.ControllArrowUI();
        }

        private void LogTag(RichTextTag tag)
        {
            if (tag != null)
            {
                Debug.Log("Tag: " + tag.ToString());
            }
        }

        private void HandleCharacterPrinted(string printedCharacter)
        {
            // Do not play a sound for whitespace
            if (printedCharacter == " " || printedCharacter == "\n")
            {
                return;
            }

            var audioSource = this.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = this.gameObject.AddComponent<AudioSource>();
            }

            audioSource.clip = this.printSoundEffect;
            audioSource.Play();
        }

        private void HandlePrintCompleted()
        {
            switch(TutorialController.GetTutorialCount()) {
                case 1: {
                    TutorialController.ShowNextButton();
                    break;
                }
                case 2: {
                    TutorialController.AllowClickEventResetDiceScreen();
                    TutorialController.PreventClickEventNextButton();
                    break;
                }
                case 3: {
                    TutorialController.ShowNextButton();
                    break;
                }
                case 4: {
                    TutorialController.AllowClickEventDices();
                    TutorialController.PreventClickEventBlocks();
                    TutorialController.PreventClickEventNextButton();
                    break;
                }
                case 5: {
                    TutorialController.AllowClickEventBlocks();                    
                    TutorialController.PreventClickEventNextButton();
                    break;
                }
                case 6: {
                    TutorialController.PreventClickEventDices();
                    TutorialController.PreventClickEventBlocks();
                    TutorialController.ShowNextButton();
                    break;
                }
                case 7: {
                    TutorialController.ShowNextButton();
                    break;
                }
                case 8: {
                    TutorialController.AllowClickEventDices();
                    TutorialController.AllowClickEventBlocks();
                    TutorialController.PreventClickEventNextButton();
                    break;
                }
                case 9: {
                    TutorialController.ShowNextButton();
                    break;
                }
                case 10: {
                    TutorialController.AllowClickEventDices();
                    TutorialController.PreventClickEventBlocks();                    
                    TutorialController.AllowClickEventBlocks();
                    TutorialController.PreventClickEventNextButton();
                    break;
                }
                case 11: {
                    TutorialController.ShowNextButton();                    
                    break;
                }
                case 12: {
                    TutorialController.AllowClickEventResetDiceScreen();                    
                    TutorialController.PreventClickEventNextButton();
                    TutorialController.PreventClickEventDices();
                    break;
                }
                case 13: {
                    TutorialController.ShowNextButton();
                    break;
                }
                case 14: {
                    TutorialController.ShowNextButton();
                    TutorialController.AllowClickEventResetDiceScreen();
                    TutorialController.AllowClickEventBlocks();
                    TutorialController.AllowClickEventDices();                    
                    break;
                }
                case 15: {
                    break;
                }
                default: break;
            }
            Debug.Log("TypeText Complete");
        }
    }
}
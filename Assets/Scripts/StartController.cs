using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartController : MonoBehaviour
{
    [SerializeField] GameObject[] purchasedDices = null;
    [SerializeField] GameObject leftButton = null;
    [SerializeField] GameObject rightButton = null;
    [SerializeField] GameObject stageTextObject = null;
    [SerializeField] GameObject stageStarObject = null;
    [SerializeField] GameObject heartRewardBar = null;
    [SerializeField] Sprite clearStar = null;
    [SerializeField] Sprite unclearStar = null;

    GameObject currentDice = null;
    GameObject heartShopCanvas = null;

    int leftButtonNumber = 0;
    int rightButtonNumber = 0;
     public bool isNextStage = false;

    void Start()
    {
        SetObjects();
        GetComponent<Animator>().SetBool("isClicked", false);
        HideScreen();
    }

    public void UpdateClickedMapStage(bool isGoingToNextStage)
    {
        isNextStage = isGoingToNextStage;
        // var StageStartButton = GameObject.Find("Start Button").GetComponent<Button>();
        // StageStartButton.onClick.RemoveAllListeners();
        // StageStartButton.onClick.AddListener(() => FindObjectOfType<MapController>().OnClickMap(isNextStage));
    }

    public void SetObjects()
    {
        heartShopCanvas = GameObject.Find("Heart Shop Canvas");
    }

    public void UpdateStageNumber(int levelNumber)
    {
        stageTextObject.GetComponent<Text>().text = $"Stage {levelNumber.ToString()}";
        int levelCleared = PlayerPrefs.GetInt($"Level {levelNumber}", 0);
        if (levelNumber % 10 == 0&& levelCleared==0)
        {
            heartRewardBar.SetActive(true);
        } else {
            heartRewardBar.SetActive(false);
        }
    }

    public void UpdateStageStar(int clearStarCount)
    {
        for (int i = 0; i < 3; i++) 
        {
            if (i < clearStarCount) {
                stageStarObject.transform.GetChild(i).GetComponent<Image>().sprite = clearStar;
            } else {
                stageStarObject.transform.GetChild(i).GetComponent<Image>().sprite = unclearStar;
            }
        }
    }

    public void SetCurrentDiceDisplay()
    {
        if (purchasedDices != null)
        {
            foreach (GameObject purchasedDice in purchasedDices)
            {
                if (purchasedDice.name.ToLower() == GetCurrentDiceType().ToLower())
                {
                    purchasedDice.SetActive(true);
                    currentDice = purchasedDice;
                }
                else
                {
                    purchasedDice.SetActive(false);
                }
            }

            SetDiceIndexToButton();
        }
    }

    public void SetDiceIndexToButton()
    {
        int currentDiceIndex = System.Array.IndexOf(purchasedDices, currentDice);

        if (currentDiceIndex == 0)
        {
            leftButton.SetActive(false);
            rightButton.SetActive(true);
            rightButtonNumber = currentDiceIndex + 1;
        } else if (currentDiceIndex == purchasedDices.Length - 1)
        {
            rightButton.SetActive(false);
            leftButton.SetActive(true);
            leftButtonNumber = currentDiceIndex - 1;
        }
        else
        {
            leftButton.SetActive(true);
            leftButtonNumber = currentDiceIndex - 1;

            rightButton.SetActive(true);
            rightButtonNumber = currentDiceIndex + 1;
        }
    }

    public void FixedDiceDisplay(string type)
    {
        switch(type)
        {
            case "left":
                {
                    for (int i = 0; i < purchasedDices.Length; i++)
                    {
                        if (i == leftButtonNumber)
                        {
                            purchasedDices[i].SetActive(true);
                            currentDice = purchasedDices[i];
                        }
                        else
                        {
                            purchasedDices[i].SetActive(false);
                        }
                    }
                    break;
                }
            case "right":
                {
                    for (int i = 0; i < purchasedDices.Length; i++)
                    {
                        if (i == rightButtonNumber)
                        {
                            purchasedDices[i].SetActive(true);
                            currentDice = purchasedDices[i];
                        }
                        else
                        {
                            purchasedDices[i].SetActive(false);
                        }
                    }
                    break;
                }
            default: break;
        }

        var buttonControllers = FindObjectsOfType<ButtonController>();
        Debug.Log(currentDice);
        buttonControllers[0].UseDice(currentDice.name.ToLower());        

        SetDiceIndexToButton();
    }

    // public void SlideDiceSelectDisplay(int index)
    // {
    //     currentDice = purchasedDices[index];

    //     foreach (GameObject purchasedDice in purchasedDices)
    //     {
    //         if (purchasedDice.name.Split(' ')[0].ToLower() != GetCurrentDiceType().ToLower())
    //         {
    //             purchasedDice.SetActive(false);
    //         }
    //         else
    //         {
    //             currentDice = purchasedDice;
    //             SetDiceIndexToButton();
    //         }
    //     }
    // }

    public static string GetCurrentDiceType()
    {
        if (PlayerPrefs.GetInt($"used-{Constants.MaldivesDice}") == 1)
        {
            return Constants.MaldivesDice;
        }
        else if (PlayerPrefs.GetInt($"used-{Constants.GoldrushDice}") == 1)
        {
            return Constants.GoldrushDice;
        }

        return "defaultDice";
    }

    public void DeleteDiceRecords()
    {
        PlayerPrefs.SetInt($"used-{Constants.MaldivesDice}", 0);
        PlayerPrefs.SetInt($"purchased-{Constants.MaldivesDice}", 0);

        PlayerPrefs.SetInt($"used-{Constants.GoldrushDice}", 0);
        PlayerPrefs.SetInt($"purchased-{Constants.GoldrushDice}", 0);
    }

    public void ShowScreen()
    {
        this.gameObject.GetComponent<CanvasGroup>().alpha = 1;
        this.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
        this.gameObject.transform.GetChild(1).GetComponent<CanvasGroup>().ignoreParentGroups = true;
        
        SetObjects();

        if (heartShopCanvas != null) 
        {
            FindObjectOfType<HeartShopController>().ToggleHeartShopCanvas(false);
        }
    }

    public void HideScreen()
    {
        this.gameObject.GetComponent<CanvasGroup>().alpha = 0;
        this.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
        this.gameObject.transform.GetChild(1).GetComponent<CanvasGroup>().ignoreParentGroups = false;        
    }   
}

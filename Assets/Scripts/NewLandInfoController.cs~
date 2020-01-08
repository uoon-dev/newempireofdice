using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Analytics;

public class NewLandInfoController : MonoBehaviour
{
    [SerializeField] Sprite newLandMineImage = null;
    [SerializeField] Sprite newLandDungeonImage = null;
    [SerializeField] Sprite newLandArmyImage = null;
    [SerializeField] Sprite newLandWizardImage = null;
    [SerializeField] Sprite newLandRelicsImage = null;
    [SerializeField] Sprite newLandVerticalImage = null;
    [SerializeField] Sprite newLandHorizontalImage = null;
    [SerializeField] Sprite newLandBombImage = null;
    [SerializeField] GameObject newLandImageObject = null;
    [SerializeField] GameObject newLandName = null;
    [SerializeField] GameObject newLandDescription = null;
    public static string newLandType = null;

    // Start is called before the first frame update
    void Start()
    {
    }


    public void ToggleNewLandInfoCanvas(bool isShow) {
        SetInfoType();
        this.gameObject.SetActive(isShow);
        var body = this.gameObject.transform.GetChild(0);

        if (isShow) {
            AnalyticsEvent.Custom("NEW_LAND", new Dictionary<string, object> { { "landType", newLandType } });
            body.transform.DOMoveY(Screen.height/2, 0.25f).SetDelay(1f);
            return;
        }
        body.transform.DOMoveY(-Screen.height/2, 0.25f).SetDelay(1f);
        return;
    }

    public void SetInfoType() {
        switch(newLandType) {
            case "mine": {
                newLandImageObject.GetComponent<Image>().sprite = newLandMineImage;
                newLandName.GetComponent<Text>().text = "광산";
                newLandDescription.GetComponent<Text>().text = "주사위 굴리기 비용 1 감소";
                break;
            }
            case "dungeon": {
                newLandImageObject.GetComponent<Image>().sprite = newLandDungeonImage;
                newLandName.GetComponent<Text>().text = "던전";
                newLandDescription.GetComponent<Text>().text = "주사위의 최대 눈 1 증가";
                break;
            }
            case "army": {
                newLandImageObject.GetComponent<Image>().sprite = newLandArmyImage;
                newLandName.GetComponent<Text>().text = "용병";
                newLandDescription.GetComponent<Text>().text = "원할 때 모든 주사위들의 눈을 1 더해주는 아이템 획득";
                break;
            }
            case "wizard": {
                newLandImageObject.GetComponent<Image>().sprite = newLandWizardImage;
                newLandName.GetComponent<Text>().text = "마법사";
                newLandDescription.GetComponent<Text>().text = "원할 때 모든 주사위들의 눈을 2배로 만들어주는 아이템 획득";
                break;
            }
            case "relics": {
                newLandImageObject.GetComponent<Image>().sprite = newLandRelicsImage;
                newLandName.GetComponent<Text>().text = "유물";
                newLandDescription.GetComponent<Text>().text = "원할 때 10배의 눈을 가진 주사위 한 개를 굴려주는 아이템 획득";
                break;
            }
            case "vertical": {
                newLandImageObject.GetComponent<Image>().sprite = newLandVerticalImage;
                newLandName.GetComponent<Text>().text = "공습";
                newLandDescription.GetComponent<Text>().text = "위아래 모든 땅의 방어력을 반으로 깎음";
                break;
            }
            case "horizontal": {
                newLandImageObject.GetComponent<Image>().sprite = newLandHorizontalImage;
                newLandName.GetComponent<Text>().text = "기병대";
                newLandDescription.GetComponent<Text>().text = "양옆 모든 땅의 방어력을 반으로 깎음";
                break;
            }
            case "bomb": {
                newLandImageObject.GetComponent<Image>().sprite = newLandBombImage;
                newLandName.GetComponent<Text>().text = "폭탄";
                newLandDescription.GetComponent<Text>().text = "주변 여덟 땅의 방어력을 반으로 깎음";
                break;
            }
        }
    }

    public void SetLandType(string type) {
        newLandType = type;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GuideCanvasController : MonoBehaviour
{
    // Start is called before the first frame update
    public static string guideType = null;

    void Start()
    {
        // ToggleGuideCanvas(true);
    }

    public void ToggleGuideCanvas(bool isShow)
    {
        this.gameObject.SetActive(isShow);
        var body = this.gameObject.transform.Find(Constants.GAME_OBJECT_NAME.BODY);
        var panelSetting = this.gameObject.transform.Find(Constants.GAME_OBJECT_NAME.PANEL_SETTING);

        if (isShow)
        {
            body.transform.DOMoveY(Screen.height/2, 0.25f).SetDelay(1f).OnComplete(() => {
                panelSetting.gameObject.SetActive(true);
            });
            return;
        }
        body.transform.DOMoveY(-Screen.height/2, 0.25f).SetDelay(1f);
        return;
    }

    public void SetGuideType(string type)
    {
        guideType = type;
    }    
}

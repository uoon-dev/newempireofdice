using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [SerializeField] GameObject PauseCanvas = null;

    void Start()
    {
        HideScreen();
    }

    public void ShowScreen()
    {
        var blocks = FindObjectsOfType<Block>();
        foreach (var block in blocks) {
            block.HideTooltip();
        }
        PauseCanvas.GetComponent<Canvas>().sortingOrder = 13;
        PauseCanvas.SetActive(true);

    }

    public void HideScreen()
    {
        PauseCanvas.SetActive(false);
    }
}

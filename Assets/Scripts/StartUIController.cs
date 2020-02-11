using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUIController : MonoBehaviour
{
    [SerializeField] 
    Sprite startButtonLoadingImage; 
    LevelLoader levelLoader;
    Image startButtonImage;

    void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        startButtonImage = GameObject.Find(Constants.GAME_OBJECT_NAME.START_BUTTON).GetComponent<Image>();
    }

    public void ClickStartButton()
    {
        startButtonImage.sprite = startButtonLoadingImage;
        levelLoader.LoadNextScene();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    [SerializeField] GameObject mainCanvas = null;
    [SerializeField] GameObject startController = null;
    [SerializeField] GameObject level = null;
    [SerializeField] GameObject borderSign = null;

    // Scroll 관련
    [SerializeField] ScrollRect scrollRect = null;

    [SerializeField] Sprite flagUncleared = null;
    [SerializeField] Sprite[] flagCleared = null;
    // 클리어한 맵 바로 다음 맵
    [SerializeField] Sprite flagNew = null;
    [SerializeField] Sprite flagClicked = null;
    [SerializeField] Sprite heartFlag = null;
    [SerializeField] Sprite[] mapViewImages = null;

    [SerializeField] Sprite clearStar = null;
    [SerializeField] Sprite unclearStar = null;

    public static GameObject previousClickedMap = null;
    public bool onClickedLastClearedNextMap = false;
    Image previousMapImage = null;

    int leftScrollCount = 0;
    int lastClearedMapNumber = 0;
    int restrictedMapCount = 20;
    int viewObjectMaxCountIndex = 3;
    int levelNumber = 1;
    float firstLeftWallPoint = 9999;
    float leftPaddingAbsolutePositionX;
    float canvasWidth;
    float canvasHeight;
    float cameraHeight;
    float cameraWidth;
    float speed = 1.5f;
    float[,] flagPositions;
    string currenetSceneName;
    LevelLoader levelLoader;
    NewHeartController newHeartController;
    UIController UIController;


    void Start()
    {
        currenetSceneName = SceneManager.GetActiveScene().name;
        // 테스트용
        // for (int i = 1; i < 161; i++) {
        //     PlayerPrefs.DeleteKey($"Level {i}");
        //     PlayerPrefs.DeleteKey($"LevelStar {i}");
        // }

        // for (int i = 1; i < 10; i++) {
        //     PlayerPrefs.SetInt($"Level {i}", 1);
        //     // PlayerPrefs.SetInt($"LevelStar {i}", i % 3 + 1);
        //     PlayerPrefs.SetInt($"LevelStar {i}", 1);
        // }

        // PlayerPrefs.SetInt("LevelCycled", 1);

        if (currenetSceneName == Constants.SCENE_NAME.MAP_SYSTEM) 
        {
            Initialize();
            SetCanvasWidthAndHeight();
            UpdateLastClearedMapNumber();
            SetFlagPositions();

            InitPositionByRatio();
            LoadMap();
            InitNextStageButton();

            startController.SetActive(true);
        }
    }

    private void Initialize()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        newHeartController = FindObjectOfType<NewHeartController>();
        UIController = FindObjectOfType<UIController>();
    }    

    private void InitPositionByRatio()
    {
        cameraHeight = 2 * Camera.main.orthographicSize;
        cameraWidth = cameraHeight * Camera.main.aspect;
        leftPaddingAbsolutePositionX = -borderSign.transform.position.x - cameraWidth / 2 + 0.325f;
        scrollRect.content.position = new Vector3(leftPaddingAbsolutePositionX, 0, 0);
    }

    private void Update()
    {
        if (currenetSceneName == Constants.SCENE_NAME.MAP_SYSTEM)
        {
            if (PlayerPrefs.GetInt("LevelCycled") == 1)
            {
                AnimateMap();
            }
        }

    }

    private void AnimateMap()
    {
        if (scrollRect.content.position == new Vector3(leftPaddingAbsolutePositionX, 0, 0)) {
            scrollRect.content.position = new Vector3((2 * leftPaddingAbsolutePositionX + cameraWidth) + leftPaddingAbsolutePositionX, 0, 0);
        }

        speed += 0.4f;

        scrollRect.content.position = Vector3.MoveTowards(scrollRect.content.position, new Vector3(leftPaddingAbsolutePositionX-0.00001f, 0, 0), speed * Time.deltaTime);
        mainCanvas.GetComponent<CanvasGroup>().blocksRaycasts = false;

        if (scrollRect.content.position == new Vector3(leftPaddingAbsolutePositionX-0.00001f, 0, 0))
        {
            mainCanvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
            PlayerPrefs.SetInt($"LevelCycled", 0);
        }
    }

    private void SetCanvasWidthAndHeight()
    {
        canvasWidth = mainCanvas.GetComponent<RectTransform>().rect.width;
        canvasHeight = mainCanvas.GetComponent<RectTransform>().rect.height;
    }

    private void UpdateLastClearedMapNumber()
    {
        int i = 1;
        bool stop = false;

        while (!stop)
        {
            int isClear = PlayerPrefs.GetInt($"Level {i}");
            int currentLevelNumber = levelLoader.GetCurrentLevelNumber();

            if (isClear == 1)
            {
                lastClearedMapNumber = i;
            }
            else
            {
                stop = true;
            }
            i++;
        }
    }

    public void LoadMap()
    {
        InitMap(3, 3);
        if (lastClearedMapNumber >= restrictedMapCount)
            InitMap(2, 2);

        if (lastClearedMapNumber >= restrictedMapCount * 2)
            InitMap(1, 1);

        if (lastClearedMapNumber >= restrictedMapCount * 3)
            InitMap(0, 0);            
    }

    public void InitMap(int cycle, int viewObjectOrder)
    {
        int firstLineLevelNumber = 0;

        for (int startIndex = 1; startIndex <= restrictedMapCount; startIndex++)
        {
            GameObject clonedLevel = Instantiate(level, transform.position, transform.rotation);
            Text clonedLevelText = clonedLevel.transform.GetChild(0).GetComponent<Text>();
            levelNumber = GetLevelNumber(startIndex, cycle);

            if (startIndex == 1)
            {
                firstLineLevelNumber = levelNumber;
            }

            clonedLevel.name = $"Level {levelNumber}";
            clonedLevelText.text = levelNumber.ToString();

            clonedLevel.transform.SetParent(scrollRect.content.GetChild(viewObjectOrder), false); 
            clonedLevel.transform.localPosition = InitMapPosition(startIndex, (levelNumber-1) % (restrictedMapCount * (viewObjectMaxCountIndex + 1)));

            // 레벨 1을 가진 view의 position.x 값을 왼쪽 벽으로 만들기 : (카메라 길이 + 각 양쪽 패딩) * 뷰 개수
            if (clonedLevel.name == Constants.SCENE_NAME.LEVEL1)
            {
                firstLeftWallPoint = (2 * leftPaddingAbsolutePositionX + cameraWidth) * Mathf.Floor(lastClearedMapNumber / restrictedMapCount);
            }
            UpdateFlagImage(clonedLevel, clonedLevelText);
        }

        Image viewImageObject = scrollRect.content.GetChild(viewObjectOrder).GetComponent<Image>();
        int viewImageCycle = (int)Mathf.Floor(firstLineLevelNumber / restrictedMapCount) % mapViewImages.Length;
        viewImageObject.sprite = mapViewImages[viewImageCycle];
    }

    private int GetLevelNumber(int startIndex, int cycle) {
        if (lastClearedMapNumber % restrictedMapCount == 0)
        {
            levelNumber = lastClearedMapNumber + startIndex - (restrictedMapCount * (viewObjectMaxCountIndex - cycle + leftScrollCount));
        }
        else
        {
            levelNumber = lastClearedMapNumber + startIndex - (lastClearedMapNumber % restrictedMapCount) - (restrictedMapCount * (viewObjectMaxCountIndex - cycle + leftScrollCount));
        }

        return levelNumber;
    }
    private Vector2 InitMapPosition(int startIndex, int cycle) {
        Vector2 position = new Vector2(0, 0);

        position = new Vector2(flagPositions[cycle, 0], 
            flagPositions[cycle, 1]);

        return position;
    }

    public void UpdateFlagImage(GameObject clonedLevel, Text clonedLevelText)
    {
        int isClear = PlayerPrefs.GetInt(clonedLevel.name);
        var star = clonedLevel.transform.GetChild(1);
        var newStageFlag = clonedLevel.transform.GetChild(2);
        var heartRewardFlag = clonedLevel.transform.GetChild(3);
        
        if (isClear == 1)
        {
            
            // 클리어한 맵에 클리어 이미지 씌우고 다음 맵에 새 맵 이미지 씌우기
            int clearStarCount = PlayerPrefs.GetInt($"LevelStar {levelNumber}");
            clonedLevel.GetComponent<Image>().sprite = flagCleared[clearStarCount - 1];

            // 별 이미지 세팅 
            for (int i = 0; i < clearStarCount; i++) 
            {
                star.GetChild(i).GetComponent<Image>().sprite = clearStar;
            }

            star.GetComponent<CanvasGroup>().DOFade(1, 0);
        }
        else
        {
            int clonedLevelNumber = int.Parse(clonedLevel.name.Split(' ')[1]);

            if (clonedLevelNumber == lastClearedMapNumber + 1 ||
                clonedLevel.name == Constants.SCENE_NAME.LEVEL1)
            {
                // clonedLevel.GetComponent<Image>().sprite = flagNew;
                // clonedLevel.GetComponent<RectTransform>().sizeDelta = new Vector2(51, 44);
                newStageFlag.gameObject.SetActive(true);
                clonedLevel.GetComponent<Button>().enabled = true;
                // clonedLevelText.transform.DOLocalMove(new Vector2(clonedLevelText.transform.localPosition.x - 20, clonedLevelText.transform.localPosition.y - 23), 0);
                clonedLevelText.fontSize = 30;
            } else {
                int levelCleared = PlayerPrefs.GetInt($"Level {clonedLevelNumber}",0);
                if (clonedLevelNumber % 10 == 0 && levelCleared==0)
                {
                    heartRewardFlag.gameObject.SetActive(true);
                }
                clonedLevel.GetComponent<Button>().enabled = false;
                clonedLevelText.color = new Color32(228,184,144, 255);
                clonedLevelText.fontSize = 22;
            }

            star.GetComponent<CanvasGroup>().DOFade(0, 0);
        }
    }

    public void DeleteMap(int order)
    {
        for (int i = 1; i <= restrictedMapCount; i++) {
            DestroyImmediate(scrollRect.content.GetChild(order).GetChild(0).gameObject);
        }
    }

    public void OnDrag()
    {
        // 가장 오른쪽 레벨 뷰일 때 오른쪽 스크롤 벽 세우기
        if (scrollRect.content.position.x <= -leftPaddingAbsolutePositionX)
        {
            scrollRect.content.position = new Vector2(-leftPaddingAbsolutePositionX, 0);
        }

        // 가장 왼쪽 레벨 1이 나오는 뷰일 때 왼쪽 스크롤 벽 세우기
        if (!firstLeftWallPoint.Equals(9999) && scrollRect.content.position.x >= firstLeftWallPoint + leftPaddingAbsolutePositionX)
        {
            scrollRect.content.position = new Vector2(firstLeftWallPoint + leftPaddingAbsolutePositionX, 0);
        }

        // view의 경계를 넘어설 때마다 첫번째 컴포넌트를 재사용하는 로직
        if (scrollRect.content.position.x > (2 * leftPaddingAbsolutePositionX + cameraWidth) * (leftScrollCount + 3) + 1)
        {
            leftScrollCount++;

            for (int i = 0; i < scrollRect.content.childCount; i++) {
                int viewNumber = int.Parse(scrollRect.content.GetChild(i).name.Split(' ')[1]);
                // 무한스크롤 로직상 4321 4321 4321 순열이 반복된다. 그래서 해당 순서가 돌아온 객체의 현재 index를 가져와서 그 객체값을 얻어 지우고 새롭게 생성해줘야 한다.
                if (viewNumber == (viewObjectMaxCountIndex + 1) - ((leftScrollCount - 1) % (viewObjectMaxCountIndex + 1))) {
                    DeleteMap(i);
                    InitMap(0, i);
                } 
            }
        }

        
        // Debug.Log($"{scrollRect.content.position.x} : {(2 * leftPaddingAbsolutePositionX + cameraWidth) * (leftScrollCount + 1)}");

        // view의 경계를 넘어설 때마다 마지막 컴포넌트를 재사용하는 로직
        if (scrollRect.content.position.x < (2 * leftPaddingAbsolutePositionX + cameraWidth) * leftScrollCount)
        {
            if (leftScrollCount != 0) {
                leftScrollCount--;

                for (int i = 0; i < scrollRect.content.childCount; i++) {
                    int viewNumber = int.Parse(scrollRect.content.GetChild(i).name.Split(' ')[1]);
                    if (viewNumber == (viewObjectMaxCountIndex + 1) - (leftScrollCount % (viewObjectMaxCountIndex + 1))) {
                        DeleteMap(i);
                        InitMap(3, i);
                    } 
                }
            }
        }
    }

    public void ClickMap(GameObject map)
    {
        if (previousMapImage != null)
        {
            previousClickedMap.GetComponent<Image>().sprite = previousMapImage.sprite;
        }

        previousClickedMap = map;
        InitStartController(false);
    }

    public void InitStartController(bool isGoingToNextStage)
    {
        onClickedLastClearedNextMap = isGoingToNextStage;
        var startController = FindObjectOfType<StartController>();
        int levelNumber = 0;

        if (isGoingToNextStage) {
            levelNumber = lastClearedMapNumber + 1;
        } else {
            levelNumber = int.Parse(previousClickedMap.name.Split(' ')[1]);
        }

        startController.UpdateStageNumber(levelNumber);

        int clearStarCount = PlayerPrefs.GetInt($"LevelStar {levelNumber}");
        startController.UpdateStageStar(clearStarCount);
        // startController.UpdateClickedMapStage(isGoingToNextStage);
        startController.ShowScreen();
    }

    public void OnClickMap()
    {
        int levelNumber = 0;
        
        if (onClickedLastClearedNextMap) 
        {
            levelNumber = lastClearedMapNumber + 1;
        } else 
        {
            levelNumber = int.Parse(previousClickedMap.name.Split(' ')[1]);
        }
        
        levelLoader.LoadClickedMap(levelNumber);
    }

    public int GetRestrictedMapCount()
    {
        return restrictedMapCount;
    }
    
    private void SetFlagPositions() 
    {
        flagPositions = new float[,]{
                {-551, 130},
                {-526, 56},
                {-519, -24.6f},
                {-509.6f, -96},
                {-423.6f, -75},
                {-381, 1.4f},
                {-328.6f, 60.9f},
                {-262, 96.1f},
                {-165.3f, 115.7f},
                {-111.2f, 168.9f},
                {-32.7f, 177.1f},
                {39.5f, 141.7f},
                {59, 65},
                {108, -2},
                {196, -14},
                {283.9f, 0},
                {361, 46},
                {419, 104},
                {492, 144},
                {568, 110},

                {-570, 45},
                {-510, 5},
                {-441, -11},
                {-370, -25},
                {-298, -21},
                {-281, 48},
                {-245, 110},
                {-176, 133},
                {-109, 122},
                {-60, 81},
                {1.6f, 52.6f},
                {26.7f, -13.9f},
                {50.6f, -79},
                {119, -102},
                {194, -94},
                {272, -102},
                {343.7f, -120},
                {420.2f, -108.8f},
                {482.6f, -68.1f},
                {564.6f, -1},

                {-570, 20},
                {-495, 51},
                {-415, 66},
                {-334, 101},
                {-288.6f, 159},
                {-207, 166},
                {-132, 147},
                {-74, 101},
                {-105, 19},
                {-69, -52},
                {24, -67},
                {117.8f, -45.5f},
                {148, 31.2f},
                {134.2f, 102.2f},
                {189, 158},
                {266, 173},
                {345, 154},
                {434.2f, 144.5f},
                {520.3f, 140.5f},
                {569.2f, 67.3f},

                {-575, 23.4f},
                {-497.3f, 6},
                {-440.2f, -40.3f},
                {-379.3f, -104.7f},
                {-306.7f, -111.5f},
                {-234.3f, -74.8f},
                {-171.9f, -30.4f},
                {-103.5f, -5.3f},
                {-30.4f, -15.8f},
                {34.7f, -45.6f},
                {109, -55},
                {175, -26},
                {193, 43},
                {210, 118},
                {281, 123},
                {344.3f, 95.6f},
                {409.4f, 110.4f},
                {447.4f, 162.5f},
                {500.6f, 202},
                {567.6f, 198}
            };
    }

    private void InitNextStageButton()
    {
        string nextStageNumber = (lastClearedMapNumber + 1).ToString();
        var nextStageButton = GameObject.Find("Next Stage Button").GetComponent<Button>();
        var nextStageNumberText = GameObject.Find("Next Stage Number").GetComponent<Text>();
        var nextStage = GameObject.Find($"Level {nextStageNumber}");
        nextStageNumberText.text = nextStageNumber;
        nextStageButton.onClick.AddListener(() => InitStartController(true));
    }
}

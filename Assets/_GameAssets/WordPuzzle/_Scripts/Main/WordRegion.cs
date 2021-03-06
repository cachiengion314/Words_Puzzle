﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using PlayFab;
using UnityEngine.UI;
using TMPro;
using Superpow;
using Spine.Unity;
using PlayFab.Internal;

public class WordRegion : MonoBehaviour
{
    [SerializeField] private GameObject _hintFree;
    [SerializeField] private GameObject _hintPrice;
    [SerializeField] private GameObject _MultiplehintFree;
    [SerializeField] private GameObject _MultiplehintPrice;
    [SerializeField] private GameObject _selectedhintFree;
    [SerializeField] private GameObject _SelectedhintPrice;
    [SerializeField] private List<GameObject> _beehives;
    [SerializeField] private List<RectTransform> _posTarget;
    [SerializeField] private Transform _posBottom;

    //[SerializeField] private Sprite _spriteExcellent;
    [SerializeField] private Sprite _spriteNormal;
    //public Image boardHighlight;
    public Image board;
    [Space]
    public TextMeshProUGUI _textLevel;
    public TextPreview textPreview;
    public Compliment compliment;
    public ButtonVideoHintFree btnAdsHintFreePfb;
    private ButtonVideoHintFree _btnHintADS;
    public Transform parentAdsHint;
    public Button btnDictionary;
    public Button btnHintTarget;
    public Button btnHint;
    public Button btnMultipleHint;
    public Button btnShuffle;
    public Button btnRewardAds;
    public Button btnBonusBox;
    public Button btnHelp;

    public Button btnSetting;
    public GameObject shadowBonuxbox;
    public GameObject shadowHelp;
    public GameObject starCollectPfb;
    [Space]
    //[SerializeField] private RectTransform _headerBlock;
    //[SerializeField] private RectTransform _centerBlock;
    [SerializeField] private RectTransform _rectCanvas;
    [SerializeField] private SafeAreaPanel _canvasSafeArea;
    public RectTransform RectCanvas
    {
        get
        {
            return _rectCanvas;
        }
    }

    private List<LineWord> lines = new List<LineWord>();
    public List<LineWord> WordRegionLines
    {
        get
        {
            return lines;
        }
    }

    private List<string> validWords = new List<string>();
    private int _extraWord;

    private int _countShowAdsHintFree;
    private int _countShowAdsHintFreeOldLevel;
    private GameLevel gameLevel;
    private int numWords, numCol, numRow;
    public int NumWords
    {
        get
        {
            return numWords;
        }
    }
    private float cellSize, startFirstColX = 0f;
    private bool hasLongLine;

    private RectTransform rt;
    private int _currLevel;

    [HideInInspector] public string keyLevel;
    public List<string> listWordInLevel;
    public List<string> listWordCorrect;
    [HideInInspector] public bool isOpenOverlay = false;
    [HideInInspector] public int numStarCollect;
    public static WordRegion instance;

    [Header("Object To Change Themes")]
    public Image background;
    public Image header;
    public Image iconStar;
    public Image iconAdd;
    public Image bgCurrency;
    public Image bgLevelTitle;
    public Image iconSetting;
    public Image iconDictionary;
    public Image iconHoney;
    public Image bgHoney;
    public Image imageGround;
    [Space]
    //public Image imgLeafTopLeft;
    //public Image imgLeafTopRight;
    //public GameObject imgLeafBoardWordRegion;
    public GameObject iconLevelTitle;
    public TextMeshProUGUI textNumberStar;
    [Space]
    public Image imgNumHint;
    public Image imgPriceHint;
    public Image imgNumMultipleHint;
    public Image imgPriceMultipleHint;
    public Image imgNumSelectedHint;
    public Image imgPriceSelectedHint;
    public TextMeshProUGUI textNumHint;
    public TextMeshProUGUI textPriceHint;
    public TextMeshProUGUI textNumMultipleHint;
    public TextMeshProUGUI textPriceMultipleHint;
    public TextMeshProUGUI textNumSelectedHint;
    public TextMeshProUGUI textPriceSelectedHint;
    [Space]
    public SpineControl animBtnHelp;
    public SpineControl animBtnBonusBox;
    public SpineControl animBtnHint;
    public SpineControl animBtnHintTarget;
    public SpineControl animBtnMultipleHint;
    public SpineControl animBtnShuffle;
    public SpineControl animBtnRewardAds;
    public SpineControl animBtnBonusBoxShadow;
    public SpineControl animBtnHelpShadow;
    [Space]
    public SpineControl animBeehive1;
    public SpineControl animBeehive2;
    public SpineControl animBeehive3;
    public SpineControl animBeehive4;

    public ButtonVideoHintFree BtnADS
    {
        get
        {
            return _btnHintADS;
        }
    }

    public List<LineWord> Lines
    {
        get
        {
            return lines;
        }
    }

    public int CurLevel
    {
        get
        {
            return _currLevel;
        }
    }

    public Sprite SpriteNormal
    {
        get
        {
            return _spriteNormal;
        }
        set
        {
            _spriteNormal = value;
        }
    }

    private void Awake()
    {
        instance = this;
        rt = GetComponent<RectTransform>();
        //boardHighlight.gameObject.SetActive(false);
        FacebookController.instance.bonusNewLevel = 0;
        FacebookController.instance.newWordOpenInLevel = new List<string>();
        FacebookController.instance.existWord = new List<string>();
        FacebookController.instance.GetUserData();
    }

    private void LevelStartCallEventFirebase()
    {
        // Log an event with multiple parameters, passed as an array:
        var parameters = new Firebase.Analytics.Parameter[]
        {
            new Firebase.Analytics.Parameter("device", SystemInfo.deviceName),
            new Firebase.Analytics.Parameter("level", _currLevel),
        };

        Firebase.Analytics.FirebaseAnalytics.LogEvent("curr_level_start", parameters);
    }

    public void UserItemCallEventFirebase(string nameItem)
    {
        var parameters = new Firebase.Analytics.Parameter[]
        {
            new Firebase.Analytics.Parameter("item_name", nameItem),
            new Firebase.Analytics.Parameter("level", _currLevel),
        };

        Firebase.Analytics.FirebaseAnalytics.LogEvent("items_used", parameters);
    }

    public void SpendStarItemCallEventFirebase(string nameItem, int value)
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent(
              Firebase.Analytics.FirebaseAnalytics.EventSpendVirtualCurrency,
              new Firebase.Analytics.Parameter[] {
                new Firebase.Analytics.Parameter(
                  Firebase.Analytics.FirebaseAnalytics.ParameterItemName, nameItem),
                new Firebase.Analytics.Parameter(
                  Firebase.Analytics.FirebaseAnalytics.ParameterValue, value),
                new Firebase.Analytics.Parameter(
                  Firebase.Analytics.FirebaseAnalytics.ParameterVirtualCurrencyName, nameItem),
              }
            );
    }

    private List<string> GetExtraWordRandom(List<string> words)
    {
        var wordLengthEqual = new List<string>();
        foreach (var word in words)
        {
            var wordEqual = words.FindAll(w => w.Length == word.Length);
            if (wordEqual.Count > 2)
            {
                foreach (var wEqual in wordEqual)
                {
                    if (!wordLengthEqual.Contains(wEqual))
                        wordLengthEqual.Add(wEqual);
                }
            }
        }
        for (int i = 0; i < _extraWord; i++)
        {
            if (wordLengthEqual.Count > 0)
            {
                var random = wordLengthEqual[Random.Range(0, wordLengthEqual.Count)];
                words.Remove(random);
                words.Add(random);
            }
        }
        return words;
    }

    //public void CalculateScaleSizeBoardRegionAndPan()
    //{
    //    var boardSizeX = board.rectTransform.sizeDelta.x;
    //    var panSizeX = imageGround.rectTransform.sizeDelta.x;
    //    var sizeOutSafeArea = Screen.safeArea.height < (int)(_rectCanvas.rect.height) ? ((int)(_rectCanvas.rect.height) - Screen.safeArea.height) / 2 :
    //        Screen.safeArea.height > (int)(_rectCanvas.rect.height) ? (Screen.safeArea.height - (int)(_rectCanvas.rect.height)) / 2 : 0;
    //    var boardSizeY = (int)(_rectCanvas.rect.height) / 2 - _centerBlock.rect.height / 2 - _headerBlock.rect.height - (Screen.safeArea.height < 1920f ? 0 : (Screen.safeArea.height < (int)(_rectCanvas.rect.height) ? sizeOutSafeArea : (Screen.safeArea.height > (int)(_rectCanvas.rect.height) ? -sizeOutSafeArea : 0)));
    //    var panSizeY = (int)(_rectCanvas.rect.height) / 2 - _centerBlock.rect.height / 2 - (Screen.safeArea.height < 1920f ? 0 : (Screen.safeArea.height < (int)(_rectCanvas.rect.height) ? sizeOutSafeArea : (Screen.safeArea.height > (int)(_rectCanvas.rect.height) ? -sizeOutSafeArea : 0)));

    //    board.rectTransform.sizeDelta = new Vector2(boardSizeX, boardSizeY);
    //    imageGround.rectTransform.sizeDelta = new Vector2(panSizeX, panSizeY);
    //}

    private void SetValidWords(List<string> words)
    {
        var tempLinesShown = lines.FindAll(li => li.isShown);
        foreach (var word in ExtraWord.instance.extraWords)
        {
            words.Remove(word);
        }

        foreach (var line in tempLinesShown)
        {
            words.Remove(line.answer);
        }

        validWords = words;
        if (SceneAnimate.Instance.isShowTest)
        {
            var validWordsLog = "";
            foreach (var word in validWords)
            {
                validWordsLog += word + " | ";
            }
            Debug.Log("Valid Words: "+validWordsLog);
        }
    }

    public void Load(GameLevel gameLevel, int level)
    {
        keyLevel = level.ToString();
        _currLevel = level + 1;
        this.gameLevel = gameLevel;
        _extraWord = gameLevel.numExtra;
        LevelStartCallEventFirebase();
        var wordList = CUtils.BuildListFromString<string>(this.gameLevel.answers);
        numWords = wordList.Count - _extraWord;
        HoneyPointsController.instance.NumberOfWordsInLevelWithoutExtra = numWords;

        var wordInLevel = wordList.GetRange(0, numWords);
        wordInLevel = wordInLevel.OrderBy(word => word.Length).ToList();

        foreach (var word in wordList)
        {
            listWordInLevel.Add(word.ToLower());
        }

        numCol = numWords <= 5 ? 1 :
                     numWords <= 12 ? 2 : 3;

        numRow = (int)Mathf.Ceil(numWords / (float)numCol);

        int maxCellInWidth = 0;

        for (int i = numRow; i <= numWords; i += numRow)
        {
            maxCellInWidth += wordInLevel[i - 1].Length;
        }

        if (numWords % numCol != 0) maxCellInWidth += wordInLevel[wordInLevel.Count - 1].Length;

        if (numCol > 1)
        {
            float coef = (maxCellInWidth + (maxCellInWidth - numCol) * Const.CELL_GAP_COEF_X + (numCol - 1) * Const.COL_GAP_COEF);
            cellSize = rt.rect.width / coef;
            float maxSize = rt.rect.height / (numRow + (numRow + numCol) * Const.CELL_GAP_COEF_Y);
            if (maxSize < cellSize)
            {
                cellSize = maxSize;
            }
        }
        else
        {
            float coef = (maxCellInWidth + (maxCellInWidth - numCol) * Const.CELL_GAP_COEF_X + (numCol - 1) * Const.COL_GAP_COEF);
            cellSize = rt.rect.height / (numRow + (numRow - 1) * Const.CELL_GAP_COEF_Y);
            float maxSize = rt.rect.width / (maxCellInWidth + (maxCellInWidth - 1) * Const.CELL_GAP_COEF_X);

            if (maxSize < cellSize)
            {
                hasLongLine = true;
                cellSize = maxSize;
            }
        }

        //if (cellSize > 250f) cellSize = 250f;
        var isTut = CPlayerPrefs.GetBool("TUTORIAL", false);
        CheckShowButton(isTut);

        string[] levelProgress = (Prefs.unlockedWorld == 0 && Prefs.unlockedSubWorld == 0 && Prefs.unlockedLevel == 0 && !isTut) ? new string[0] : GetLevelProgress();
        string[] answerProgress = (Prefs.unlockedWorld == 0 && Prefs.unlockedSubWorld == 0 && Prefs.unlockedLevel == 0 && !isTut) ? new string[0] : GetAnswerProgress();
        bool useProgress = false;
        bool useAnsProgress = false;

        if (levelProgress.Length != 0)
        {
            useProgress = CheckLevelProgress(levelProgress, wordList);
            if (!useProgress) ClearLevelProgress();
        }

        if (answerProgress.Length != 0)
        {
            useAnsProgress = CheckAnswerProgress(answerProgress, wordList, numWords);
            if (!useProgress) ClearAnswerProgress();
        }


        SetupLine(wordList, useProgress, levelProgress, answerProgress);

        SetLinesPosition();

        SetupNumhintFree();
        SetupNumMultiplehintFree();
        SetupNumSelectedhintFree();
        CurrencyController.onHintFreeChanged += UpdateHintFree;
        CurrencyController.onMultipleHintFreeChanged += UpdateHintFree;
        CurrencyController.onSelectedHintFreeChanged += UpdateHintFree;

        FacebookController.instance.newLevel = false;
        //FacebookController.instance.user.levelProgress = levelProgress;
        //FacebookController.instance.user.answerProgress = answerProgress;
        //FacebookController.instance.SaveDataGame();
        //CheckGameComplete();
        if (_currLevel >= TutorialController.instance.cellStarLevel && !CPlayerPrefs.HasKey("SHOW_TUT_CELL_STAR"))
        {
            foreach (var cellTut in lines[lines.Count - 1].cells)
            {
                if (!cellTut.isShown)
                {
                    cellTut.iconCoin.transform.localScale = Vector3.one;
                }
            }
        }
    }

    private void CheckShowButton(bool isTut)
    {
        if (!CPlayerPrefs.HasKey("HINT_TUTORIAL"))
            btnHint.gameObject.SetActive(false);
        if (!CPlayerPrefs.HasKey("SHUFFLE_TUTORIAL"))
            btnShuffle.gameObject.SetActive(false);
        if (!CPlayerPrefs.HasKey("HELP_TUTORIAL"))
        {
            btnHelp.gameObject.SetActive(false);
            shadowHelp.SetActive(false);
        }
        if (!CPlayerPrefs.HasKey("SELECTED_HINT_TUTORIAL"))
            btnHintTarget.gameObject.SetActive(false);
        if (!CPlayerPrefs.HasKey("TUT_EXTRA_WORD"))
        {
            btnBonusBox.gameObject.SetActive(false);
            shadowBonuxbox.SetActive(false);
        }
        if (!CPlayerPrefs.HasKey("MULTIPLE_HINT_TUTORIAL"))
            btnMultipleHint.gameObject.SetActive(false);
        var openBtnReward = CPlayerPrefs.HasKey("MULTIPLE_HINT_TUTORIAL") && CPlayerPrefs.HasKey("SELECTED_HINT_TUTORIAL") && CPlayerPrefs.HasKey("HINT_TUTORIAL");
        btnRewardAds.gameObject.SetActive(false);
        CUtils.CheckConnection(this, (result) =>
        {
            if (result == 0)
            {
                btnRewardAds.gameObject.SetActive(openBtnReward);
            }
            else
            {
                btnRewardAds.gameObject.SetActive(false);
            }
        });

    }

    void UpdateHintFree()
    {
        SetupNumhintFree();
        SetupNumMultiplehintFree();
        SetupNumSelectedhintFree();
    }

    private void SetupLine(List<string> wordList, bool useProgress, string[] levelProgress, string[] answerProgress)
    {
        int lineIndex = 0;
        int countID = 0;
        var wordInLevel = wordList.GetRange(0, numWords);
        wordInLevel = wordInLevel.OrderBy(word => word.Length).ToList();
        //var countAnswer = wordList.Count < 5 ? wordList.Count : wordList.Count - _extraWord;
        //foreach (var word in wordList)
        for (int i = 0; i < numWords; i++)
        {
            var word = wordInLevel[i];
            var words = wordList.FindAll(wd => wd.Length == word.Length);
            LineWord line = Instantiate(MonoUtils.instance.lineWord);
            //if (CPlayerPrefs.HasKey(line.name + "_Chapter_" + GameState.currentSubWorld + "_Level_" + GameState.currentLevel))
            //    line.answer = CPlayerPrefs.GetString(line.name + "_Chapter_" + GameState.currentSubWorld + "_Level_" + GameState.currentLevel);
            //else
            //line.answer = "";
            line.numLetters = word.Length;
            line.answers = words;
            line.cellSize = cellSize;
            line.name = line.name + lineIndex + "_" + _currLevel;
            line.Build(ConfigController.Config.isWordRightToLeft);

            if (useProgress)
            {
                line.SetProgress(levelProgress[lineIndex], answerProgress[lineIndex]);
            }

            line.SetLineWidth();

            line.transform.SetParent(transform);
            line.transform.localScale = Vector3.one;
            line.transform.localPosition = Vector3.zero;
            line.usedBee = CPlayerPrefs.GetBool(line.name, false);
            line.isAds = CPlayerPrefs.GetBool(line.name + "_ADS", false);
            if (!line.isShown)
            {
                GetCellShowHint(line);
            }
            else
            {
                line.selectID = countID;
                countID++;
            }
            lines.Add(line);
            lineIndex++;
        }
        var tempLinesShown = lines.FindAll(li => li.isShown);
        var tempLinesNotShown = lines.FindAll(li => !li.isShown);
        foreach (var lineShown in tempLinesShown)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i] != lineShown)
                {
                    lines[i].answers.Remove(lineShown.answer);
                }
            }
        }
        foreach (var lineNotShown in tempLinesNotShown)
        {
            foreach (var li in lines)
            {
                if (li != lineNotShown && li.cells.Count == lineNotShown.cells.Count)
                    lineNotShown.FilterSuitableAnswers(li);
            }
        }
        ShowBtnDictionaryInGamePlay();
        SetValidWords(wordList);
    }

    private void GetCellShowHint(LineWord line)
    {
        foreach (var cell in line.cells)
        {
            cell.bg.color = new Color(1, 1, 1, 0.5f);
            if (line.usedBee && !cell.isShown && cell != line.cells[0] && Prefs.IsSaveLevelProgress())
            {
                cell.iconCoin.transform.localScale = Vector3.one;
                //cell.CalculateTextRaitoScale(cell.iconCoin.rectTransform);
            }
        }
    }

    public void ShowBtnDictionaryInGamePlay()
    {
        var checkShowDicBtn = lines.Any(li => li.isShown);
        btnDictionary.gameObject.SetActive(checkShowDicBtn);
    }

    public bool IsUseBee()
    {
        var isUse = lines.Any(line => line.usedBee);
        return isUse;
    }

    private void SetupNumhintFree()
    {
        var hintFree = CurrencyController.GetHintFree();
        if (hintFree > 0)
        {
            ShowPriceHint(false);
        }
        else
        {
            ShowPriceHint(true);
        }
    }

    private void SetupNumMultiplehintFree()
    {
        var multiplehintFree = CurrencyController.GetMultipleHintFree();
        if (multiplehintFree > 0)
        {
            ShowPriceMultipleHint(false);
        }
        else
        {
            ShowPriceMultipleHint(true);
        }
    }

    private void SetupNumSelectedhintFree()
    {
        var selectedhintFree = CurrencyController.GetSelectedHintFree();
        if (selectedhintFree > 0)
        {
            ShowPriceSelectedHint(false);
        }
        else
        {
            ShowPriceSelectedHint(true);
        }
    }

    private void SetLinesPosition()
    {
        lines = lines.OrderBy(li => li.cells.Count).ToList();
        if (numCol >= 2)
        {
            float[] startX = new float[numCol];
            startX[0] = -(startFirstColX + (lines[numRow - 1].lineWidth + cellSize * Const.COL_GAP_COEF + lines[lines.Count - 1].lineWidth) / 2);

            for (int i = 1; i < numCol; i++)
            {
                startX[i] = startX[i - 1] + lines[numRow * i - 1].lineWidth + cellSize * Const.COL_GAP_COEF;
            }

            for (int i = 0; i < lines.Count; i++)
            {
                int lineX = i / numRow;
                int lineY = numRow - 1 - i % numRow;

                float x = startX[lineX];
                float gapY = (rt.rect.height - cellSize * numRow) / (numRow + 1);
                //float y = gapY + lineY * cellSize + gapY * (lineY - 1) * Const.CELL_GAP_COEF_Y * lines.Count + gapY / 2;
                float y = gapY + lineY * cellSize + gapY * (lineY - 1) + gapY;

                lines[i].transform.localPosition = new Vector2(x, y);
                lines[i].gameObject.AddComponent<GraphicRaycaster>();
                var canvas = lines[i].GetComponent<Canvas>();
                canvas.overrideSorting = false;
            }
        }
        else
        {
            int count = lines.Count;
            for (int i = 0; i < count; i++)
            {
                //float x = rt.rect.width / 2 - lines[i].lineWidth / 2;
                float x = /*boardHighlight.rectTransform.rect.width / 2 */-lines[i].lineWidth / 2;
                //float x = startFirstColX;
                float y;
                //if (hasLongLine)
                //{
                //    float gapY = (rt.rect.height - numRow * cellSize) / (numRow + 1);
                //    y = gapY + (cellSize + gapY) * i;
                //}
                //else
                //{
                float gapY = cellSize + cellSize * Const.CELL_GAP_COEF_Y;
                y = gapY * (count - 1 - i) + (rt.rect.height - gapY * count + cellSize * Const.CELL_GAP_COEF_Y) / 2f;
                //}
                lines[i].transform.localPosition = new Vector2(x, y);
                lines[i].gameObject.AddComponent<GraphicRaycaster>();
                var canvas = lines[i].GetComponent<Canvas>();
                canvas.overrideSorting = false;
            }
        }
    }

    public void CheckAdsIsShow()
    {
        var isAdsHintFree = CPlayerPrefs.GetBool(keyLevel + "ADS_HINT_FREE", false);
        var keyPos = CPlayerPrefs.HasKey(keyLevel + "POS_ADS_BUTTON_X");
        if (!isAdsHintFree && keyPos)
        {
            var valueX = CPlayerPrefs.GetFloat(keyLevel + "POS_ADS_BUTTON_X");
            var valueY = CPlayerPrefs.GetFloat(keyLevel + "POS_ADS_BUTTON_Y");
            var valueZ = CPlayerPrefs.GetFloat(keyLevel + "POS_ADS_BUTTON_Z");
            var line = lines.Single(l => l.isAds && !l.usedBee);

            if (_btnHintADS == null)
                _btnHintADS = Instantiate(btnAdsHintFreePfb, line.transform);
            _btnHintADS.transform.localPosition = new Vector3(valueX, valueY, valueZ);
            _btnHintADS.gameObject.SetActive(true);

            var cellTarget = CheckCellTarget(line.cells);
            CalculateRatioScaleBtnAds(cellTarget);
            _btnHintADS.Cell = cellTarget;
            _btnHintADS.SetLineFreeletter(line);
            if (!CPlayerPrefs.HasKey("CELL_ADS_TUTORIAL") && !TutorialController.instance.isShowTut)
                TutorialController.instance.ShowPopCellAdsTut();
        }
        else
        {
            if (_btnHintADS != null)
                _btnHintADS.gameObject.SetActive(false);
        }
    }

    private void CalculateRatioScaleBtnAds(Cell cell)
    {
        var ratioScale = cell.GetComponent<RectTransform>().rect.width / _btnHintADS._btnAds.img.rectTransform.rect.width;
        _btnHintADS.Cell = cell;
        _btnHintADS._btnAds.img.rectTransform.localScale = _btnHintADS._btnAds.img.rectTransform.localScale * ratioScale;
    }

    private Cell CheckCellTarget(List<Cell> cells)
    {
        foreach (var cell in cells)
        {
            if (cell.isAds)
            {
                var line = lines.Single(l => l.isAds);
                if (line != null)
                    line.SetDataLetter(CPlayerPrefs.GetString("LINE_ANSWER"));
                return cell;
            }
        }
        return null;
    }

    public void SetWordOpenInLevelAmount(string checkWord)
    {
        if (!FacebookController.instance.user.wordPassed.Contains(checkWord))
            FacebookController.instance.newWordOpenInLevel.Add(checkWord);
        else
            FacebookController.instance.existWord.Add(checkWord);
        var isComplete = lines.All(x => x.isShown);
        if (isComplete && Prefs.IsSaveLevelProgress())
            FacebookController.instance.bonusNewLevel = 10;
        else
            FacebookController.instance.bonusNewLevel = 0;
        FacebookController.instance.UpdateStaticsUser();
    }

    private LineWord _lineIsChecking;
    private int lineIndex = 0;
    public void CheckAnswer(string checkWord)
    {
        var isTut = CPlayerPrefs.GetBool("TUTORIAL", false);
        //var lineNotShown = lines.FindAll(l => !l.isShown);
        var checkLength = lines.All(l => !l.answers.Contains(checkWord) && (l.cells.Count != checkWord.Length));
        if (checkLength && !TutorialController.instance.isShowTut)
        {
            NotifyMessage.instance.ShowMessage(NotifyMessage.instance.WORD_LENGTH_REQUIREMENT);
            textPreview.ClearText();
            return;
        }
        var isExist = lines.FindAll(li => !li.isShown).All(l => l.answer != checkWord);
        //var lineLengthEqualCheckWord = lines.FindAll(li => !li.isShown && li.cells.Count == checkWord.Length);
        //var lineAnswerEmpty = lineLengthEqualCheckWord.Find(x => x.answer == "" && isExist);
        var lineIsShown = lines.FindAll(li => li.isShown);
        LineWord line = lines.Find(x => x.answers.Contains(checkWord) && !x.isShown && !TutorialController.instance.isShowTut && (/*lineAnswerEmpty != null ||*/ CheckAnswerFill(x, checkWord)));
        if (GameState.currentLevel == 0 && GameState.currentSubWorld == 0 && GameState.currentWorld == 0 && !isTut)
            line = TutorialController.instance.LineTarget.answer == checkWord ? TutorialController.instance.LineTarget : null;
        //else if (_currLevel >= 10 && !CPlayerPrefs.HasKey("TUT_EXTRA_WORD") && !isTut)
        //    line = null;
        //string meaning="";
        if (line != null)
        {
            _lineIsChecking = line;

            //if (!line.isShown)
            //{
            line.SetDataLetter(checkWord);
            textPreview.SetAnswerColor();
            line.selectID = lineIsShown.Count;
            line.ShowAnswer();
            SaveLevelProgress();
            string wordDone = _lineIsChecking != null ? _lineIsChecking.answer : "wordDone";
            CheckGameComplete(wordDone);
            SetWordOpenInLevelAmount(checkWord);

            if (lineIndex > 0)
            {
                //boardHighlight.gameObject.SetActive(true);
                //boardHighlight.color = new Color(1, 1, 1, 0);

            }

            ShowComplimentFX();

            MainController.instance.SaveWordComplete(checkWord);
            //listWordCorrect.Add(checkWord.ToLower());
            //}
            //else
            //{
            //    line.ShowFxAnswerDuplicate();
            //    textPreview.SetExistColor();
            //}
            if (textPreview.useFX)
                textPreview.ClearText();
            if (!btnDictionary.gameObject.activeInHierarchy)
                btnDictionary.gameObject.SetActive(true);
            ObjectiveManager.instance.CheckTaskComplete();
        }
        else
        {
            var lineCheckBonus = lines.FindAll(li => _lineIsChecking != null && li.cells.Count == _lineIsChecking.cells.Count);
            var isShowBonusbox = (lineCheckBonus != null && lineCheckBonus.Count > 0) ? lineCheckBonus.All(li => li.isShown) : false;
            LineWord lineExist = lines.Find(x => x.answers.Contains(checkWord) && x.isShown && !TutorialController.instance.isShowTut);
            if (lineExist != null && lineExist.answer == checkWord)
            {
                NotifyMessage.instance.ShowMessage(NotifyMessage.instance.WORD_EXIST);
                Sound.instance.Play(Sound.Others.WordAlready);
                lineExist.ShowFxAnswerDuplicate();
                textPreview.SetExistColor();
                if (textPreview.useFX)
                    textPreview.ClearText();
                if (_currLevel >= TutorialController.instance.bonusBoxLevel && isShowBonusbox && !CPlayerPrefs.HasKey("TUT_EXTRA_WORD") && lines.Any(li => li.isShown) && _lineIsChecking.answers.Count > 1)
                {
                    TutorialController.instance.ShowPopWordTut(TutorialController.instance.contentWordAgain, 0, false, "", false, _lineIsChecking);
                }
            }
            else
            {
                CheckExtraWordAndWrong(checkWord);
            }
            if (GameState.currentLevel == 0 && GameState.currentSubWorld == 0 && GameState.currentWorld == 0 && !isTut)
                TutorialController.instance.ShowPopWordTut(TutorialController.instance.contentWordAgain);
        }
        if (!textPreview.useFX)
            textPreview.ClearText();
    }

    public void ShowComplimentFX()
    {
        if (MainController.instance != null)
        {
            MainController.instance.canvasFx.gameObject.SetActive(EffectController.instance.IsEffectOn);
        }
        gameObject.GetComponent<Canvas>().overrideSorting = false;
        compliment.Show(lineIndex);
        Sound.instance.Play(Sound.instance.complimentSounds[lineIndex]);

        lineIndex++;

        var isComplete = lines.All(x => x.isShown);
        if (HoneyPointsController.instance != null)
        {
            HoneyPointsController.instance.isLevelComplete = isComplete;
            HoneyPointsController.instance.LineIndex++;
        };

        if (lineIndex > compliment.sprites.Length - 1)
        {
            lineIndex = compliment.sprites.Length - 1;
            HoneyPointsController.instance.LineIndex = lineIndex;
            //board.sprite = _spriteExcellent;
            //board.SetNativeSize();
            //boardHighlight.color = new Color(1, 1, 1, 1);
        }
        ObjectiveManager.instance.CheckTaskComplete();
    }

    private void SetupCellAds()
    {
        var countRandomShow = Random.Range(3, 6);
        var isAdsHintFree = CPlayerPrefs.GetBool(keyLevel + "ADS_HINT_FREE", false);
        _countShowAdsHintFree += 1;
        if (_countShowAdsHintFree > countRandomShow && !isAdsHintFree && (_btnHintADS == null || !_btnHintADS.gameObject.activeInHierarchy))
        {
            if (CPlayerPrefs.HasKey(keyLevel + "POS_ADS_BUTTON_X"))
            {
                var line = lines.Single(l => l.isAds);
                if (_btnHintADS == null)
                    _btnHintADS = Instantiate(btnAdsHintFreePfb, line.transform);
                _btnHintADS.gameObject.SetActive(true);
                var valueX = CPlayerPrefs.GetFloat(keyLevel + "POS_ADS_BUTTON_X");
                var valueY = CPlayerPrefs.GetFloat(keyLevel + "POS_ADS_BUTTON_Y");
                var valueZ = CPlayerPrefs.GetFloat(keyLevel + "POS_ADS_BUTTON_Z");
                _btnHintADS.transform.localPosition = new Vector3(valueX, valueY, valueZ);

                var cellTarget = CheckCellTarget(line.cells);
                CalculateRatioScaleBtnAds(cellTarget);
            }
            else
            {
                var lineNotShown = lines.FindAll(l => !l.isShown && !l.usedBee);
                if (lineNotShown == null || lineNotShown.Count <= 0)
                    return;
                var lineRandom = lineNotShown[Random.Range(0, lineNotShown.Count)];
                var indexAnswer = Random.Range(0, lineRandom.answers.Count);
                var cellNotShown = lineRandom.cells.FindAll(cell => !cell.isShown && !cell.isAds);
                var cellRandom = cellNotShown[Random.Range(0, cellNotShown.Count)];

                lineRandom.isAds = true;
                cellRandom.isAds = true;
                CPlayerPrefs.SetBool(cellRandom.name + "_ADS", cellRandom.isAds);
                CPlayerPrefs.SetBool(lineRandom.name + "_ADS", lineRandom.isAds);

                if (_btnHintADS == null)
                    _btnHintADS = Instantiate(btnAdsHintFreePfb, lineRandom.transform);
                _btnHintADS.gameObject.SetActive(true);
                CPlayerPrefs.SetString("LINE_ANSWER", lineRandom.answers[indexAnswer]);
                //lineRandom.SetDataLetter(lineRandom.answers[indexAnswer]);
                SaveLevelProgress();
                _btnHintADS.transform.position = cellRandom.transform.position;
                CPlayerPrefs.SetFloat(keyLevel + "POS_ADS_BUTTON_X", _btnHintADS.transform.localPosition.x);
                CPlayerPrefs.SetFloat(keyLevel + "POS_ADS_BUTTON_Y", _btnHintADS.transform.localPosition.y);
                CPlayerPrefs.SetFloat(keyLevel + "POS_ADS_BUTTON_Z", _btnHintADS.transform.localPosition.z);
                _btnHintADS.Cell = cellRandom;
                _btnHintADS.SetLineFreeletter(lineRandom);
                CalculateRatioScaleBtnAds(cellRandom);
            }
            if (!CPlayerPrefs.HasKey("CELL_ADS_TUTORIAL") && !TutorialController.instance.isShowTut)
                TutorialController.instance.ShowPopCellAdsTut();
        }
    }

    private void ShowAdsInOldLevel()
    {
        var countRandomShow = Random.Range(3, 6);
        var isAdsHintFree = lines.All(line => !line.isAds);
        _countShowAdsHintFreeOldLevel += 1;
        if (_countShowAdsHintFreeOldLevel > countRandomShow && isAdsHintFree)
        {
            var lineNotShown = lines.FindAll(l => !l.isShown && !l.usedBee);
            var lineRandom = lineNotShown[Random.Range(0, lineNotShown.Count)];
            var indexAnswer = Random.Range(0, lineRandom.answers.Count);
            var cellNotShown = lineRandom.cells.FindAll(cell => !cell.isShown && !cell.isAds);
            var cellRandom = cellNotShown[Random.Range(0, cellNotShown.Count)];

            lineRandom.isAds = true;
            cellRandom.isAds = true;

            if (_btnHintADS == null)
                _btnHintADS = Instantiate(btnAdsHintFreePfb, lineRandom.transform);
            _btnHintADS.gameObject.SetActive(true);
            //lineRandom.SetDataLetter(lineRandom.answers[indexAnswer]);
            SaveLevelProgress();
            _btnHintADS.transform.position = cellRandom.transform.position;
            _btnHintADS.Cell = cellRandom;
            _btnHintADS.SetLineFreeletter(lineRandom);
            CalculateRatioScaleBtnAds(cellRandom);
        }
    }

    private bool CheckAnswerFill(LineWord line, string wordFill)
    {
        var isRight = true;
        if (!line.isShown && line.cells.Count == wordFill.Length)
        {
            //var cellsIsShown = line.cells.FindAll(cell => cell.isShown);
            for (int i = 0; i < line.cells.Count; i++)
            {
                var cell = line.cells[i];
                if (cell.isShown && cell.letter != wordFill[i].ToString())
                {
                    isRight = false;
                    break;
                }
            }
        }
        else
            isRight = false;
        return isRight;
    }

    private void CheckExtraWordAndWrong(string checkWord)
    {
        var isTut = CPlayerPrefs.GetBool("TUTORIAL", false);
        var noMoreLine = lines.Find(li => li.answers.Contains(checkWord) && li.answer != checkWord && checkWord.Length == li.cells.Count && !TutorialController.instance.isShowTut);
        if (_currLevel >= TutorialController.instance.bonusBoxLevel && !CPlayerPrefs.HasKey("TUT_EXTRA_WORD") && !isTut)
            noMoreLine = lines.Find(li => li == TutorialController.instance.LineTarget && li.answers.Contains(checkWord) && li.answer != checkWord && checkWord.Length == li.cells.Count);
        if (/*validWords.Contains(checkWord.ToLower())*/noMoreLine != null)
        {
            if (_currLevel >= TutorialController.instance.bonusBoxLevel && !CPlayerPrefs.HasKey("TUT_EXTRA_WORD") && !isTut)
            {
                CPlayerPrefs.SetBool("TUTORIAL", true);
                TutorialController.instance.HidenPopTut();
            }
            ExtraWord.instance.ProcessWorld(checkWord);
            if (textPreview.useFX)
                textPreview.ClearText();
        }
        else
        {
            var lineCheckBonus = lines.FindAll(line => _lineIsChecking != null && line.cells.Count == _lineIsChecking.cells.Count);
            var isShowBonusbox = (lineCheckBonus != null && lineCheckBonus.Count > 0) ? lineCheckBonus.All(line => line.isShown) : false;
            if (_currLevel >= TutorialController.instance.bonusBoxLevel && isShowBonusbox && !CPlayerPrefs.HasKey("TUT_EXTRA_WORD") && lines.Any(line => line.isShown) && _lineIsChecking.answers.Count > 1)
            {
                TutorialController.instance.ShowPopWordTut(TutorialController.instance.contentWordAgain, 0, false, "", false, _lineIsChecking);
            }
            CPlayerPrefs.SetBool("LevelMisspelling", false);
            //boardHighlight.gameObject.SetActive(false);
            board.sprite = _spriteNormal;
            //board.SetNativeSize();
            Sound.instance.Play(Sound.Others.WordInvalid);
            if (_currLevel > 1 && !TutorialController.instance.isShowTut)
            {
                CUtils.CheckConnection(this, (result) =>
                {
                    if (result == 0)
                    {
                        if (Prefs.IsSaveLevelProgress())
                            SetupCellAds();
                        else
                            ShowAdsInOldLevel();
                    }
                });
            }
            textPreview.SetWrongColor();

            lineIndex = 0;
            if (HoneyPointsController.instance != null) { HoneyPointsController.instance.LineIndex = 0; };

            //compliment.ResetAnimTree();
        }
        ObjectiveManager.instance.CheckTaskComplete();
    }

    private void ClickTutHoney(System.Action callback = null)
    {
        if (!CPlayerPrefs.HasKey("HONEY_TUTORIAL") && TutorialController.instance.isShowTut)
        {
            TutorialController.instance.HidenPopTut();
            callback?.Invoke();
        }
    }

    public void ShowLevelClear()
    {
        var isLevelMisspelling = CPlayerPrefs.GetBool("LevelMisspelling", true);
        if (isLevelMisspelling)
        {
            Prefs.countLevelMisspelling += 1;
            Prefs.countLevelMisspellingDaily += 1;
        }

        if (TutorialController.instance.isShowTut)
        {
            TutorialController.instance.HidenPopTut();
            TutorialController.instance.isShowTut = false;
            CPlayerPrefs.SetBool("TUTORIAL", true);
        }

        SaveLevelProgress();
        BlockScreen.instance.Block(true);
        MainController.instance.IsLevelClear = true;
        ClearLevelProgress();
        MainController.instance.OnComplete();
        //if (lines.Count >= 6)
        //{
        //    compliment.ShowRandom();
        //}

        TweenControl.GetInstance().DelayCall(transform, 0.5f, () =>
        {
            MainController.instance.animatorScene.SetBool("LevelComplete", true);
            //SceneAnimate.Instance.animEvent.LevelClearCallback();
        });
    }

    public void CheckGameComplete(string wordDone = "wordDone")
    {
        var isComplete = lines.All(x => x.isShown);

        if (isComplete)
        {
            string wordIsChecking = wordDone == "wordDone" ? MainController.instance.wordDone : wordDone;
            FlagTabController.instance.CheckAndSaveCountrykWord(wordIsChecking);
            ShowLevelClear();
        }
        else
        {
            string wordIsChecking = wordDone == "wordDone" ? MainController.instance.wordDone : wordDone;
            FlagTabController.instance.CheckAndSaveCountrykWord(wordIsChecking);

            var lineCheckBonus = lines.FindAll(line => _lineIsChecking != null && line.cells.Count == _lineIsChecking.cells.Count);
            var isShowBonusbox = (lineCheckBonus != null && lineCheckBonus.Count > 0) ? lineCheckBonus.All(line => line.isShown) : false;
            var isTut = CPlayerPrefs.GetBool("TUTORIAL", false);
            if (GameState.currentLevel == 0 && GameState.currentSubWorld == 0 && GameState.currentWorld == 0 && !isTut)
            {
                TutorialController.instance.HidenPopTut();
                TutorialController.instance.HidenHandConnectWord(true);
                BlockScreen.instance.Block(true);
                TutorialController.instance.isBlockSwipe = true;
                TweenControl.GetInstance().DelayCall(transform, 2f, () =>
                {
                    TutorialController.instance.isBlockSwipe = false;
                    BlockScreen.instance.Block(false);
                    TutorialController.instance.ShowPopWordTut(TutorialController.instance.contentNext);
                });
            }
            else if (_currLevel >= TutorialController.instance.bonusBoxLevel && isShowBonusbox && !CPlayerPrefs.HasKey("TUT_EXTRA_WORD") && _lineIsChecking.isShown && _lineIsChecking.answers.Count > 1)
            {
                CPlayerPrefs.SetBool("TUTORIAL", false);
                BlockScreen.instance.Block(true);
                TutorialController.instance.isBlockSwipe = true;
                TweenControl.GetInstance().DelayCall(transform, 2f, () =>
                {
                    TutorialController.instance.isBlockSwipe = false;
                    BlockScreen.instance.Block(false);
                    TutorialController.instance.ShowPopWordTut(TutorialController.instance.contentManipulation, 0, false, TutorialController.instance.contentUnlockBonusBox, false, _lineIsChecking);
                });
            }
            else if (!CPlayerPrefs.HasKey("HONEY_TUTORIAL") && !TutorialController.instance.isShowTut && FacebookController.instance.user.unlockedFlagWords.Count > 0)
            {
                TutorialController.instance.ShowPopHoneyHeaderTut();
            }
            //else if (!CPlayerPrefs.HasKey("HONEY_TUTORIAL") && !TutorialController.instance.isShowTut && FacebookController.instance.user.unlockedFlagWords.Count > 0)
            //{
            //    TutorialController.instance.ShowPopHoneyHeaderTut();
            //}
        }
        ObjectiveManager.instance.CheckTaskComplete();
    }

    public void OnClickHintTarget()
    {
        int ballance = CurrencyController.GetBalance();
        var selectedhintFree = CurrencyController.GetSelectedHintFree();
        TutorialController.instance.HidenPopTut();
        if ((_currLevel >= TutorialController.instance.selectedHintLevel && !CPlayerPrefs.HasKey("SELECTED_HINT_TUTORIAL")) || (selectedhintFree > 0 && !CPlayerPrefs.HasKey("SELECTED_HINT_TUTORIAL")))
        {
            TutorialController.instance.ShowPopSelectedHint2Tut();
            CPlayerPrefs.SetBool("SELECTED_HINT_TUTORIAL", true);
        }

        if (selectedhintFree > 0 || ballance >= Const.HINT_TARGET_COST)
        {
            isOpenOverlay = !isOpenOverlay;
            DialogOverlay.instance.ShowOverlay(isOpenOverlay);
            var canvas = gameObject.GetComponent<Canvas>();
            if (!isOpenOverlay)
            {
                if (BtnADS != null && !BtnADS.animbutton.raycastTarget && !CPlayerPrefs.GetBool(WordRegion.instance.keyLevel + "ADS_HINT_FREE"))
                    BtnADS.animbutton.raycastTarget = true;
                canvas.sortingLayerName = "UI1";
                canvas.overrideSorting = false;
                foreach (var li in lines)
                {
                    li.HidenOverlayOfCell();
                }
            }
            else
            {
                if (BtnADS != null && BtnADS.animbutton.raycastTarget)
                    BtnADS.animbutton.raycastTarget = false;
                canvas.overrideSorting = true;
                canvas.sortingLayerName = "UI2";
                foreach (var li in lines)
                {
                    li.HighlightCellNotShown();
                }
            }
        }
        else
        {
            ConfigController.instance.isShopHint = true;
            gameObject.GetComponent<Canvas>().overrideSorting = false;
            Sound.instance.Play(Sound.Others.PopupOpen);
            DialogController.instance.ShowDialog(DialogType.Shop, DialogShow.REPLACE_CURRENT);
        }
    }

    public void OnClickCellTarget(Cell cell)
    {
        TutorialController.instance.HidenPopTut();
        Prefs.countBooster += 1;
        Prefs.countBoosterDaily += 1;
        isOpenOverlay = false;
        DialogOverlay.instance.ShowOverlay(false);
        var line = lines.Single(li => li.cells.Contains(cell));
        foreach (var li in lines)
        {
            li.HidenOverlayOfCell();
        }
        line.ShowHintCelltarget(cell);
        UserItemCallEventFirebase("item_targeted_hint");
    }

    public void BeeClick()
    {
        if (MainController.instance != null)
        {
            var currTheme = ThemesControl.instance.CurrTheme;
            MainController.instance.canvasPopup.gameObject.SetActive(true);
            animBeehive1.thisSkeletonControl.initialSkinName = currTheme.animData.skinAnim;
            animBeehive2.thisSkeletonControl.initialSkinName = currTheme.animData.skinAnim;
            animBeehive3.thisSkeletonControl.initialSkinName = currTheme.animData.skinAnim;
            animBeehive4.thisSkeletonControl.initialSkinName = currTheme.animData.skinAnim;
            //TweenControl.GetInstance().DelayCall(transform, 0.2f,()=> {
            //    animBeehive1.SetSkin(currTheme.animData.skinAnim);
            //    animBeehive2.SetSkin(currTheme.animData.skinAnim);
            //    animBeehive3.SetSkin(currTheme.animData.skinAnim);
            //    animBeehive4.SetSkin(currTheme.animData.skinAnim);
            //});
        }
        BlockScreen.instance.Block(true);
        int count = 0;
        var lineNotShow = lines.FindAll(x => !x.isShown);
        for (int i = lineNotShow.Count - 1; i > 0; i--)
        {
            var line = lineNotShow[i];
            if (BeeManager.instance.CurrBee > 0 && count < lines.Count / 2 && count < 4 && count < BeeManager.instance.CurrBee)
            {
                if (line.isAds)
                {
                    line.isAds = false;
                    CPlayerPrefs.SetBool(gameObject.name + "_ADS", line.isAds);
                }
                _posTarget[count].transform.position = line.transform.position;
                var posTarget = _posTarget[count].transform;
                posTarget.localPosition = _posTarget[count].transform.localPosition + new Vector3(-line.cellSize / 2, line.cellSize / 2, 0);
                BeeFly(line, _beehives[count].transform, posTarget.position, () =>
                 {
                     Sound.instance.audioSource.Stop();
                     Sound.instance.PlayButton(Sound.Button.Beehive);
                     line.ShowCellUseBee();
                     BeeManager.instance.DebitAmountBee(1);
                 }, () =>
                 {
                     SaveLevelProgress();
                     //CheckGameComplete();
                     Prefs.AddToNumHint(GameState.currentWorld, GameState.currentSubWorld, GameState.currentLevel);
                     MainController.instance.isBeePlay = false;
                     BlockScreen.instance.Block(false);
                     TutorialController.instance.CheckAndShowTutorial();
                     if (MainController.instance != null)
                         MainController.instance.canvasPopup.gameObject.SetActive(false);
                     CheckShowBonusBoxTut();
                 });
                count += 1;
            }
        }
        UserItemCallEventFirebase("item_beehive");
    }

    public void CheckShowBonusBoxTut()
    {
        _lineIsChecking = lines.Find(line => line.isShown && line.answers.Count > 1);
        var lineCheckBonus = lines.FindAll(line => _lineIsChecking != null && line.cells.Count == _lineIsChecking.cells.Count);
        var isShowBonusbox = (lineCheckBonus != null && lineCheckBonus.Count > 0) ? lineCheckBonus.All(line => line.isShown) : false;
        var lastLineIsShown = lines.FindAll(li => li.isShown);
        if (lastLineIsShown.Count > 0)
            _lineIsChecking = lastLineIsShown[lastLineIsShown.Count - 1];
        if (_currLevel >= TutorialController.instance.bonusBoxLevel && isShowBonusbox && !CPlayerPrefs.HasKey("TUT_EXTRA_WORD") && _lineIsChecking != null && _lineIsChecking.isShown && _lineIsChecking.answers.Count > 1)
        {
            CPlayerPrefs.SetBool("TUTORIAL", false);
            BlockScreen.instance.Block(true);
            TutorialController.instance.isBlockSwipe = true;
            TweenControl.GetInstance().DelayCall(transform, 1f, () =>
            {
                TutorialController.instance.isBlockSwipe = false;
                BlockScreen.instance.Block(false);
                TutorialController.instance.ShowPopWordTut(TutorialController.instance.contentManipulation, 0, false, TutorialController.instance.contentUnlockBonusBox, false, _lineIsChecking);
            });
        }
        else if (!CPlayerPrefs.HasKey("BONUSBOX_TUT") && ExtraWord.instance.extraWords.Count > 0)
        {
            CPlayerPrefs.SetBool("BONUSBOX_TUT", true);
            BlockScreen.instance.Block(true);
            TutorialController.instance.isBlockSwipe = true;
            TweenControl.GetInstance().DelayCall(transform, 1f, () =>
            {
                TutorialController.instance.isBlockSwipe = false;
                BlockScreen.instance.Block(false);
                TutorialController.instance.ShowPopBonusBoxTut();
            });
        }
        else if (!CPlayerPrefs.HasKey("HONEY_TUTORIAL") && !TutorialController.instance.isShowTut && FacebookController.instance.user.unlockedFlagWords.Count > 0)
        {
            TutorialController.instance.ShowPopHoneyHeaderTut();
        }
        else
            CheckShowDontLikeAdsDialog();
    }

    private List<bool> InitListRandom()
    {
        var rateShow = new List<bool>();
        int num = 100;
        var rate1 = (int)(0.02f * num);
        for (int i = 0; i < num; i++)
        {
            if (i <= rate1)
                rateShow.Add(true);
            else
                rateShow.Add(false);
        }
        return rateShow;
    }

    private bool IsShowAds()
    {
        var randomList = InitListRandom();
        var result = Random.Range(0, randomList.Count);
        return randomList[result];
    }

    private void CheckShowDontLikeAdsDialog()
    {
        if (!CUtils.IsAdsRemoved() && _currLevel > AdsManager.instance.MinLevelToLoadInterstitial && !TutorialController.instance.isShowTut && IsShowAds())
        {
            Sound.instance.Play(Sound.Others.PopupOpen);
            DialogController.instance.ShowDialog(DialogType.DontLikeAds, DialogShow.STACK_DONT_HIDEN);
        }
    }

    private void BeeFly(LineWord line, Transform beeTarget, Vector3 posTarget, System.Action callback = null, System.Action completeFly = null)
    {
        var animBee = beeTarget.GetComponentInChildren<SpineControl>();
        beeTarget.gameObject.SetActive(true);
        var tweenControl = TweenControl.GetInstance();
        //var midPoint = Vector3.Lerp(beeTarget.position, posTarget, 0.5f) - new Vector3(beeTarget.position.x / 4, posTarget.y * 2.5f, 0);
        var midPoint2 = Vector3.Lerp(beeTarget.position, posTarget, 0.5f) - new Vector3(beeTarget.position.x * 2, -posTarget.y / 2, 0);
        var points = new List<Vector3>();
        var waypoints = new Vector3[] { };
        for (int i = 0; i < 100; i++)
        {
            var t = i / (float)100;
            var point = CalculateWaypoint(t, beeTarget.position, _posBottom.position, midPoint2, posTarget);
            points.Add(point);
        }
        waypoints = points.ToArray();
        tweenControl.LocalRotate(beeTarget, new Vector3(0, 0, -90), 1.6f, null, EaseType.InOutFlash);
        tweenControl.MovePath(beeTarget, waypoints, 1.6f, DG.Tweening.PathType.CatmullRom, DG.Tweening.PathMode.TopDown2D, 5, Color.red, () =>
          {
              animBee.SetAnimation("Loop", true);
              callback?.Invoke();
              tweenControl.MoveRectX(beeTarget as RectTransform, 1080, 1.8f, () =>
              {
                  beeTarget.gameObject.SetActive(false);
                  completeFly?.Invoke();
              }, () => CheckBeeFlyAndShowCell(line, beeTarget), EaseType.InFlash);
          }, EaseType.InOutFlash);
        //tweenControl.JumpRect(beeTarget, posTarget, -800f, 1, 1.3f, false, () =>
        //{

        //}, EaseType.Linear);
    }

    private void CheckBeeFlyAndShowCell(LineWord line, Transform beeTarget)
    {
        var cellTarget = GetCellBeeTarget(line, beeTarget);
        if (cellTarget == null)
            return;
        if (cellTarget == line.cells[0])
        {
            cellTarget.ShowTextBee();
            line.CheckSetDataAnswer(line.answer);
            line.CheckLineDone();
        }
        else if (!cellTarget.isShown)
        {
            cellTarget.iconCoin.gameObject.SetActive(true);
            TweenControl.GetInstance().ScaleFromZero(cellTarget.iconCoin.gameObject, 0.5f);
        }
    }

    private Cell GetCellBeeTarget(LineWord line, Transform beeTarget)
    {
        foreach (var cell in line.cells)
        {
            var distance = Vector3.Distance(beeTarget.position, cell.transform.position);
            if (distance < 0.2)
                return cell;
        }
        return null;
    }

    private Vector3 CalculateWaypoint(float t, Vector3 pos0, Vector3 pos1, Vector3 pos2, Vector3 pos3)
    {
        float u = (1 - t);
        float t2 = t * t;
        float t3 = t * t * t;
        float u2 = u * u;
        float u3 = u * u * u;
        Vector3 p = u3 * pos0;
        p += 3 * u2 * t * pos1;
        p += 3 * u * t2 * pos2;
        p += t3 * pos3;
        return p;
    }

    public void OnClickSetting()
    {
        Sound.instance.Play(Sound.Others.PopupOpen);
        TutorialController.instance.HidenPopTut();
        if (!CPlayerPrefs.HasKey("OBJ_TUTORIAL") && Prefs.countLevelDaily >= 10)
        {
            DialogController.instance.ShowDialog(DialogType.Objective, DialogShow.STACK_DONT_HIDEN);
        }
        else
        {
            DialogController.instance.ShowDialog(DialogType.Pause, DialogShow.STACK_DONT_HIDEN);
        }
    }

    int hintLineIndex = 0;
    int cellIndex = 0;
    public void HintClick()
    {
        TutorialController.instance.HidenPopTut();
        int ballance = CurrencyController.GetBalance();
        var hintFree = CurrencyController.GetHintFree();
        if (ballance >= Const.HINT_COST || hintFree > 0)
        {
            //hintLineIndex = CPlayerPrefs.HasKey("HINT_LINE_INDEX") ? CPlayerPrefs.GetInt("HINT_LINE_INDEX") : -1;
            ShowHintLine(hintFree);
        }
        else
        {
            ConfigController.instance.isShopHint = true;
            Sound.instance.Play(Sound.Others.PopupOpen);
            DialogController.instance.ShowDialog(DialogType.Shop, DialogShow.REPLACE_CURRENT);
        }
    }

    private void ShowHintLine(int hintFree)
    {
        LineWord line = null;
        for (int i = hintLineIndex; i < lines.Count; i++)
        {
            hintLineIndex = i;
            if (cellIndex < lines[i].cells.Count && !lines[i].cells[cellIndex].isShown && !lines[i].isShown)
            {
                line = lines[i];
                break;
            }
            else if (hintLineIndex >= lines.Count - 1)
            {
                cellIndex += 1;
                hintLineIndex = 0;
                //CPlayerPrefs.DeleteKey("HINT_LINE_INDEX");
                ShowHintLine(hintFree);
            }
        }

        if (line != null)
        {
            //CPlayerPrefs.SetInt("HINT_LINE_INDEX", hintLineIndex);
            line.ShowHint(() =>
            {
                Prefs.countBooster += 1;
                Prefs.countBoosterDaily += 1;
                if (line.isAds)
                {
                    line.isAds = false;
                    CPlayerPrefs.SetBool(gameObject.name + "_ADS", line.isAds);
                }
                Sound.instance.PlayButton(Sound.Button.Hint);
                SetupNumhintFree();
                SaveLevelProgress();
                string wordDone = line.isShown ? line.answer : "wordDone";
                CheckGameComplete(wordDone);

                Prefs.AddToNumHint(GameState.currentWorld, GameState.currentSubWorld, GameState.currentLevel);
                UserItemCallEventFirebase("item_hint");
            });
            if (hintFree > 0)
            {
                CurrencyController.DebitHintFree(1);
            }
            else
            {
                CurrencyController.DebitBalance(Const.HINT_COST);
                SpendStarItemCallEventFirebase("item_hint", Const.HINT_COST);
            }
        }
    }

    private void ShowPriceHint(bool show)
    {
        _hintFree.SetActive(!show);
        _hintPrice.SetActive(show);
    }

    private void ShowPriceMultipleHint(bool show)
    {
        _MultiplehintFree.SetActive(!show);
        _MultiplehintPrice.SetActive(show);
    }

    private void ShowPriceSelectedHint(bool show)
    {
        _selectedhintFree.SetActive(!show);
        _SelectedhintPrice.SetActive(show);
    }

    public void HintRandomClick()
    {
        numStarCollect = 0;
        TutorialController.instance.HidenPopTut();
        int ballance = CurrencyController.GetBalance();
        var multiplehintFree = CurrencyController.GetMultipleHintFree();
        if (multiplehintFree > 0 && !CPlayerPrefs.HasKey("MULTIPLE_HINT_TUTORIAL"))
            CPlayerPrefs.SetBool("MULTIPLE_HINT_TUTORIAL", true);
        if (ballance >= Const.HINT_RANDOM_COST || multiplehintFree > 0)
        {
            LineWord line = null;
            for (int i = 0; i < lines.Count; i++)
            {
                if (!lines[i].isShown)
                {
                    line = lines[i];
                    if (line != null)
                    {
                        line.ShowHintRandom(() =>
                        {
                            if (line.isAds)
                            {
                                line.isAds = false;
                                CPlayerPrefs.SetBool(line.gameObject.name + "_ADS", line.isAds);
                            }
                            Sound.instance.audioSource.Stop();
                            Sound.instance.PlayButton(Sound.Button.MultipleHint);
                            SaveLevelProgress();
                            CheckGameComplete();
                            Prefs.AddToNumHint(GameState.currentWorld, GameState.currentSubWorld, GameState.currentLevel);
                        });
                    }
                }
            }
            if (multiplehintFree > 0)
            {
                CurrencyController.DebitMultipleHintFree(1);
            }
            else
            {
                CurrencyController.DebitBalance(Const.HINT_RANDOM_COST);
                SpendStarItemCallEventFirebase("item_multiple_hint", Const.HINT_RANDOM_COST);
            }
            SetupNumMultiplehintFree();
            Prefs.countBooster += 1;
            Prefs.countBoosterDaily += 1;
            UserItemCallEventFirebase("item_multiple_hint");
        }
        else
        {
            ConfigController.instance.isShopHint = true;
            DialogController.instance.ShowDialog(DialogType.Shop);
        }
    }

    public void SaveLevelProgress()
    {
        if (!Prefs.IsSaveLevelProgress()) return;

        List<string> results = new List<string>();
        List<string> resultAnswers = new List<string>();
        foreach (var line in lines)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var cell in line.cells)
            {
                sb.Append(cell.isShown ? "1" : "0");
            }
            results.Add(sb.ToString());
            resultAnswers.Add(line.answer);
        }
        Prefs.levelProgress = results.ToArray();
        Prefs.answersProgress = resultAnswers.ToArray();
        FacebookController.instance.user.levelProgress = Prefs.levelProgress;
        FacebookController.instance.user.answerProgress = Prefs.answersProgress;
        FacebookController.instance.SaveDataGame();
    }

    public string[] GetLevelProgress()
    {
        if (Prefs.IsLastLevel() || FacebookController.instance.newLevel)
            return new string[0]; //nếu đã chơi thì biết đáp án rồi nên ko lưu
        return Prefs.levelProgress;
    }

    public string[] GetAnswerProgress()
    {
        if (Prefs.IsLastLevel() || FacebookController.instance.newLevel)
            return new string[0]; //nếu đã chơi thì biết đáp án rồi nên ko lưu
        return Prefs.answersProgress;
    }

    public void ClearLevelProgress()
    {
        if (!Prefs.IsSaveLevelProgress()) return;
        CPlayerPrefs.DeleteKey("level_progress");
    }

    public void ClearAnswerProgress()
    {
        if (!Prefs.IsSaveLevelProgress()) return;
        CPlayerPrefs.DeleteKey("answer_progress");
    }

    public bool CheckLevelProgress(string[] levelProgress, List<string> wordList)
    {
        if (!Prefs.IsSaveLevelProgress())
            return false;
        var wordInLevel = wordList.GetRange(0, numWords);
        wordInLevel = wordInLevel.OrderBy(word => word.Length).ToList();
        if (levelProgress.Length != numWords) return false;

        for (int i = 0; i < numWords; i++)
        {
            if (levelProgress[i].Length != wordInLevel[i].Length) return false;
        }
        return true;
    }

    public bool CheckAnswerProgress(string[] answerProgress, List<string> wordList, int numWord)
    {
        if (!Prefs.IsSaveLevelProgress())
            return false;
        var wordInLevel = wordList.GetRange(0, numWords);
        wordInLevel = wordInLevel.OrderBy(word => word.Length).ToList();
        if (answerProgress.Length != numWord) return false;

        for (int i = 0; i < numWord; i++)
        {
            if (answerProgress[i].Length != wordInLevel[i].Length) return false;
        }
        return true;
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            Timer.Schedule(this, 0.5f, () =>
            {
                //UpdateBoard();

            });
        }
    }

    void OnDestroy()
    {
        CurrencyController.onHintFreeChanged -= UpdateHintFree;
        CurrencyController.onMultipleHintFreeChanged -= UpdateHintFree;
        CurrencyController.onSelectedHintFreeChanged -= UpdateHintFree;
    }

    private void UpdateBoard()
    {
        string[] progress = GetLevelProgress();
        string[] progressAnswer = GetAnswerProgress();
        if (progress.Length == 0) return;
        if (progressAnswer.Length == 0) return;

        int i = 0;
        foreach (var line in lines)
        {
            line.SetProgress(progress[i], progressAnswer[i]);
            i++;
        }
    }
}

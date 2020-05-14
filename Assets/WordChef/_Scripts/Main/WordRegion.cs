using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using PlayFab;
using UnityEngine.UI;
using TMPro;
using Superpow;

public class WordRegion : MonoBehaviour
{
    [SerializeField] private GameObject _hintFree;
    [SerializeField] private GameObject _hintPrice;
    [SerializeField] private GameObject _MultiplehintFree;
    [SerializeField] private GameObject _MultiplehintPrice;

    [SerializeField] private Sprite _spriteExcellent;
    [SerializeField] private Sprite _spriteNormal;
    public Image boardHighlight;
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

    private List<LineWord> lines = new List<LineWord>();
    private List<string> validWords = new List<string>();
    private int _extraWord;

    private int _countShowAdsHintFree;
    private int _countShowAdsHintFreeOldLevel;
    private GameLevel gameLevel;
    private int numWords, numCol, numRow;
    private float cellSize, startFirstColX = 0f;
    private bool hasLongLine;

    private RectTransform rt;

    [HideInInspector] public string keyLevel;
    public List<string> listWordInLevel;
    public List<string> listWordCorrect;
    [HideInInspector] public bool isOpenOverlay = false;
    public static WordRegion instance;

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

    private void Awake()
    {
        instance = this;
        rt = GetComponent<RectTransform>();
        boardHighlight.gameObject.SetActive(false);
        FacebookController.instance.bonusNewLevel = 0;
        FacebookController.instance.newWordOpenInLevel = new List<string>();
        FacebookController.instance.existWord = new List<string>();
        FacebookController.instance.GetUserData();
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

    public void Load(GameLevel gameLevel, int level)
    {
        keyLevel = level.ToString();
        this.gameLevel = gameLevel;
        _extraWord = gameLevel.numExtra;
        var wordList = CUtils.BuildListFromString<string>(this.gameLevel.answers);
        //validWords = CUtils.BuildListFromString<string>(this.gameLevel.validWords);
        //wordList = wordList.Count <= 4 ? wordList : GetExtraWordRandom(wordList);
        numWords = wordList.Count <= 4 ? wordList.Count : wordList.Count - _extraWord;
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
            maxCellInWidth += wordList[i - 1].Length;
        }

        if (numWords % numCol != 0) maxCellInWidth += wordList[numWords - 1].Length;

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
            cellSize = boardHighlight.rectTransform.rect.height / (numRow + (numRow - 1) * Const.CELL_GAP_COEF_Y);
            float maxSize = boardHighlight.rectTransform.rect.width / (maxCellInWidth + (maxCellInWidth - 1) * Const.CELL_GAP_COEF_X);

            if (maxSize < cellSize)
            {
                hasLongLine = true;
                cellSize = maxSize;
            }
        }

        if (cellSize > 130f) cellSize = 130f;

        string[] levelProgress = GetLevelProgress();
        string[] answerProgress = GetAnswerProgress();
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

        CheckAdsIsShow();
        FacebookController.instance.newLevel = false;
        //FacebookController.instance.user.levelProgress = levelProgress;
        //FacebookController.instance.user.answerProgress = answerProgress;
        //FacebookController.instance.SaveDataGame();
        //CheckGameComplete();
    }

    private void SetupLine(List<string> wordList, bool useProgress, string[] levelProgress, string[] answerProgress)
    {
        int lineIndex = 0;
        int countID = 0;
        //var countAnswer = wordList.Count < 5 ? wordList.Count : wordList.Count - _extraWord;
        //foreach (var word in wordList)
        for (int i = 0; i < numWords; i++)
        {
            var word = wordList[i];
            var words = wordList.FindAll(wd => wd.Length == word.Length);
            LineWord line = Instantiate(MonoUtils.instance.lineWord);
            //if (CPlayerPrefs.HasKey(line.name + "_Chapter_" + GameState.currentSubWorld + "_Level_" + GameState.currentLevel))
            //    line.answer = CPlayerPrefs.GetString(line.name + "_Chapter_" + GameState.currentSubWorld + "_Level_" + GameState.currentLevel);
            //else
            //line.answer = "";
            line.numLetters = word.Length;
            line.answers = words;
            line.cellSize = cellSize;
            line.name = line.name + lineIndex + "_" + (GameState.currentSubWorld + 1) + (GameState.currentLevel + 1);
            line.Build(ConfigController.Config.isWordRightToLeft);

            if (useProgress)
            {
                line.SetProgress(levelProgress[lineIndex], answerProgress[lineIndex]);
            }

            line.SetLineWidth();

            line.transform.SetParent(transform);
            line.transform.localScale = Vector3.one;
            line.transform.localPosition = Vector3.zero;
            line.usedBee = CPlayerPrefs.GetBool(line.name);
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
        var checkShowDicBtn = lines.Any(li => li.isShown);
        btnDictionary.gameObject.SetActive(checkShowDicBtn);
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
                y = gapY * (count - 1 - i) + (boardHighlight.rectTransform.rect.height - gapY * count + cellSize * Const.CELL_GAP_COEF_Y) / 2f;
                //}
                lines[i].transform.localPosition = new Vector2(x, y);
            }
        }
    }

    private void CheckAdsIsShow()
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
        }
        else
        {
            if (_btnHintADS != null)
                _btnHintADS.gameObject.SetActive(false);
        }
    }

    private void CalculateRatioScaleBtnAds(Cell cell)
    {
        var ratioScale = cell.GetComponent<RectTransform>().rect.width / _btnHintADS.GetComponentInChildren<Image>().rectTransform.rect.width;
        _btnHintADS.Cell = cell;
        _btnHintADS.animbutton.rectTransform.localScale = _btnHintADS.animbutton.rectTransform.localScale * ratioScale;
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

    private int lineIndex = 0;
    public void CheckAnswer(string checkWord)
    {
        //var isTut = CPlayerPrefs.GetBool("TUTORIAL", false);
        var lineIsShown = lines.FindAll(li => li.isShown);
        LineWord line = lines.Find(x => x.answers.Contains(checkWord) && !x.isShown/* && !TutorialController.instance.isShowTut*/ && (x.answer == "" || CheckAnswerFill(x, checkWord)));
        //if (GameState.currentLevel == 0 && GameState.currentSubWorld == 0 && GameState.currentWorld == 0 && !isTut)
        //    line = TutorialController.instance.LineTarget.answer == checkWord ? TutorialController.instance.LineTarget : null;
        //string meaning="";
        if (line != null)
        {
            //if (!line.isShown)
            //{
            line.SetDataLetter(checkWord);
            textPreview.SetAnswerColor();
            line.selectID = lineIsShown.Count;
            line.ShowAnswer();
            SaveLevelProgress();
            CheckGameComplete();
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
        }
        else
        {
            LineWord lineExist = lines.Find(x => x.answers.Contains(checkWord) && x.isShown /*&& !TutorialController.instance.isShowTut*/);
            if (lineExist != null && lineExist.answer == checkWord)
            {
                Sound.instance.Play(Sound.Others.WordAlready);
                lineExist.ShowFxAnswerDuplicate();
                textPreview.SetExistColor();
                if (textPreview.useFX)
                    textPreview.ClearText();
            }
            else
            {
                CheckExtraWordAndWrong(checkWord);
            }
            //if (GameState.currentLevel == 0 && GameState.currentSubWorld == 0 && GameState.currentWorld == 0 && !isTut)
            //    TutorialController.instance.ShowPopWordTut(TutorialController.instance.contentWordAgain);
        }
        if (!textPreview.useFX)
            textPreview.ClearText();
    }

    public void ShowComplimentFX()
    {
        gameObject.GetComponent<Canvas>().overrideSorting = false;
        compliment.Show(lineIndex);
        Sound.instance.Play(Sound.instance.complimentSounds[lineIndex]);
        lineIndex++;
        if (lineIndex > compliment.sprites.Length - 1)
        {
            lineIndex = compliment.sprites.Length - 1;
            //board.sprite = _spriteExcellent;
            //board.SetNativeSize();
            //boardHighlight.color = new Color(1, 1, 1, 1);
        }
    }

    private void SetupCellAds()
    {
        var isAdsHintFree = CPlayerPrefs.GetBool(keyLevel + "ADS_HINT_FREE", false);
        _countShowAdsHintFree += 1;
        if (_countShowAdsHintFree > 2 && !isAdsHintFree && (_btnHintADS == null || !_btnHintADS.gameObject.activeInHierarchy) /*&& !TutorialController.instance.isShowTut*/)
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
                CalculateRatioScaleBtnAds(cellRandom);
            }
        }
    }

    private void ShowAdsInOldLevel()
    {
        var isAdsHintFree = lines.All(line => !line.isAds);
        _countShowAdsHintFreeOldLevel += 1;
        if (_countShowAdsHintFreeOldLevel > 2 && isAdsHintFree)
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
            CalculateRatioScaleBtnAds(cellRandom);
        }
    }

    private bool CheckAnswerFill(LineWord line, string wordFill)
    {
        var isRight = true;
        if (line.cells.Count == wordFill.Length)
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
        var noMoreLine = lines.Find(li => li.answers.Contains(checkWord) && li.answer != checkWord && checkWord.Length == li.cells.Count /*&& !TutorialController.instance.isShowTut*/);
        if (/*validWords.Contains(checkWord.ToLower())*/noMoreLine != null)
        {
            ExtraWord.instance.ProcessWorld(checkWord);
            if (textPreview.useFX)
                textPreview.ClearText();
        }
        else
        {
            CPlayerPrefs.SetBool("LevelMisspelling", false);
            //boardHighlight.gameObject.SetActive(false);
            board.sprite = _spriteNormal;
            board.SetNativeSize();
            Sound.instance.Play(Sound.Others.WordInvalid);
            if (Prefs.IsSaveLevelProgress())
                SetupCellAds();
            else
                ShowAdsInOldLevel();
            textPreview.SetWrongColor();
            lineIndex = 0;
            compliment.ResetAnimTree();
        }
    }
    public void CheckGameComplete()
    {
        var isComplete = lines.All(x => x.isShown);
        var isLevelMisspelling = CPlayerPrefs.GetBool("LevelMisspelling", true);
        if (isComplete)
        {
            if (isLevelMisspelling)
            {
                Prefs.countLevelMisspelling += 1;
                Prefs.countLevelMisspellingDaily += 1;
            }

            if (TutorialController.instance.isShowTut)
            {
                //TutorialController.instance.HidenPopTut();
                //TutorialController.instance.isShowTut = false;
                //CPlayerPrefs.SetBool("TUTORIAL", true);
            }

            SaveLevelProgress();
            BlockScreen.instance.Block(true);
            MainController.instance.IsLevelClear = true;
            ClearLevelProgress();
            //MainController.instance.OnComplete();
            //if (lines.Count >= 6)
            //{
            //    compliment.ShowRandom();
            //}
            TweenControl.GetInstance().DelayCall(transform, 0.5f, () =>
            {
                MainController.instance.animatorScene.SetBool("LevelComplete", true);
            });
        }
        else
        {
            //var isTut = CPlayerPrefs.GetBool("TUTORIAL", false);
            //if (GameState.currentLevel == 0 && GameState.currentSubWorld == 0 && GameState.currentWorld == 0 && !isTut)
            //{
            //    TutorialController.instance.ShowPopWordTut(TutorialController.instance.contentNext);
            //}
        }
    }

    public void OnClickHintTarget()
    {
        int ballance = CurrencyController.GetBalance();
        if (ballance >= Const.HINT_TARGET_COST)
        {
            isOpenOverlay = !isOpenOverlay;
            DialogOverlay.instance.ShowOverlay(isOpenOverlay);
            if (!isOpenOverlay)
            {
                if (BtnADS != null && !BtnADS.animbutton.raycastTarget && !CPlayerPrefs.GetBool(WordRegion.instance.keyLevel + "ADS_HINT_FREE"))
                    BtnADS.animbutton.raycastTarget = true;
                gameObject.GetComponent<Canvas>().overrideSorting = false;
                foreach (var li in lines)
                {
                    li.HidenOverlayOfCell();
                }
            }
            else
            {
                if (BtnADS != null && BtnADS.animbutton.raycastTarget)
                    BtnADS.animbutton.raycastTarget = false;
                gameObject.GetComponent<Canvas>().overrideSorting = true;
                foreach (var li in lines)
                {
                    li.HighlightCellNotShown();
                }
            }
        }
        else
        {
            gameObject.GetComponent<Canvas>().overrideSorting = false;
            Sound.instance.Play(Sound.Others.PopupOpen);
            DialogController.instance.ShowDialog(DialogType.Shop, DialogShow.REPLACE_CURRENT);
        }
    }

    public void OnClickCellTarget(Cell cell)
    {
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
    }

    public void BeeClick()
    {
        int count = 0;
        var lineNotShow = lines.FindAll(x => !x.isShown);
        for (int i = lineNotShow.Count - 1; i > 0; i--)
        {
            var line = lineNotShow[i];
            if (BeeManager.instance.CurrBee > 0 && count < lines.Count / 2 && count < 4)
            {
                if (line.isAds)
                {
                    line.isAds = false;
                    CPlayerPrefs.SetBool(gameObject.name + "_ADS", line.isAds);
                }
                line.ShowCellUseBee();
                BeeManager.instance.DebitAmountBee(1);
                SaveLevelProgress();
                CheckGameComplete();
                Prefs.AddToNumHint(GameState.currentWorld, GameState.currentSubWorld, GameState.currentLevel);
                count += 1;
            }
        }
        MainController.instance.isBeePlay = false;
        Sound.instance.PlayButton(Sound.Button.Beehive);
    }

    int hintLineIndex = 0;
    int cellIndex = 0;
    public void HintClick()
    {
        int ballance = CurrencyController.GetBalance();
        var hintFree = CurrencyController.GetHintFree();
        if (ballance >= Const.HINT_COST || hintFree > 0)
        {
            //hintLineIndex = CPlayerPrefs.HasKey("HINT_LINE_INDEX") ? CPlayerPrefs.GetInt("HINT_LINE_INDEX") : -1;
            ShowHintLine(hintFree);
        }
        else
        {
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
                if (hintFree > 0)
                {
                    CurrencyController.DebitHintFree(1);
                    Sound.instance.PlayButton(Sound.Button.Hint);
                }
                else
                {
                    CurrencyController.DebitBalance(Const.HINT_COST);
                    Sound.instance.PlayButton(Sound.Button.Hint);
                }
                SetupNumhintFree();
            });
            SaveLevelProgress();
            CheckGameComplete();

            Prefs.AddToNumHint(GameState.currentWorld, GameState.currentSubWorld, GameState.currentLevel);
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

    public void HintRandomClick()
    {
        int ballance = CurrencyController.GetBalance();
        var multiplehintFree = CurrencyController.GetMultipleHintFree();
        if (ballance >= Const.HINT_RANDOM_COST /*|| hintFree > 0*/)
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
                            //if (hintFree > 0)
                            //    CurrencyController.DebitHintFree(hintFree);
                            //else
                            Sound.instance.audioSource.Stop();
                            Sound.instance.PlayButton(Sound.Button.MultipleHint);
                        });
                        SaveLevelProgress();
                        CheckGameComplete();

                        Prefs.AddToNumHint(GameState.currentWorld, GameState.currentSubWorld, GameState.currentLevel);
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
            }
            SetupNumMultiplehintFree();
            Prefs.countBooster += 1;
            Prefs.countBoosterDaily += 1;
        }
        else
        {
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
        if (levelProgress.Length != numWords) return false;

        for (int i = 0; i < numWords; i++)
        {
            if (levelProgress[i].Length != wordList[i].Length) return false;
        }
        return true;
    }

    public bool CheckAnswerProgress(string[] answerProgress, List<string> wordList, int numWord)
    {
        if (answerProgress.Length != numWord) return false;

        for (int i = 0; i < numWord; i++)
        {
            if (answerProgress[i].Length != wordList[i].Length) return false;
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

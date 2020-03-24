using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using PlayFab;
using UnityEngine.UI;
using TMPro;

public class WordRegion : MonoBehaviour
{
    public TextMeshProUGUI _textLevel;
    public TextPreview textPreview;
    public Compliment compliment;
    public ButtonVideoHintFree btnAdsHintFreePfb;
    private ButtonVideoHintFree _bntHintADS;
    public Transform parentAdsHint;

    private List<LineWord> lines = new List<LineWord>();
    private List<string> validWords = new List<string>();

    private int _countShowAdsHintFree;
    private GameLevel gameLevel;
    private int numWords, numCol, numRow;
    private float cellSize, startFirstColX = 0f;
    private bool hasLongLine;

    private RectTransform rt;

    public List<string> listWordInLevel;
    public List<string> listWordCorrect;
    public static WordRegion instance;

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
        if (_bntHintADS != null)
            Destroy(_bntHintADS.gameObject);
    }

    public void Load(GameLevel gameLevel)
    {
        this.gameLevel = gameLevel;

        var wordList = CUtils.BuildListFromString<string>(this.gameLevel.answers);
        validWords = CUtils.BuildListFromString<string>(this.gameLevel.validWords);

        numWords = wordList.Count < 5 ? wordList.Count : wordList.Count - 2;

        numCol = numWords <= 4 ? 1 :
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
            float maxSize = rt.rect.height / (numRow + (numRow + 1) * Const.CELL_GAP_COEF_Y);
            if (maxSize < cellSize)
            {
                cellSize = maxSize;
            }
        }
        else
        {
            float coef = (maxCellInWidth + (maxCellInWidth - numCol) * Const.CELL_GAP_COEF_X + (numCol - 1) * Const.COL_GAP_COEF);
            cellSize = rt.rect.height / (numRow + (numRow - 1) * Const.CELL_GAP_COEF_Y + 0f);
            float maxSize = rt.rect.width / (maxCellInWidth + (maxCellInWidth - 1) * Const.CELL_GAP_COEF_X);

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
            if (!useAnsProgress) ClearAnswerProgress();
        }

        SetupLine(wordList, useProgress, levelProgress, answerProgress);

        SetLinesPosition();

        FacebookController.instance.user.levelProgress = levelProgress;
        FacebookController.instance.user.answerProgress = answerProgress;
        FacebookController.instance.SaveDataGame();
        CheckGameComplete();
    }

    private void SetupLine(List<string> wordList, bool useProgress, string[] levelProgress, string[] answerProgress)
    {
        int lineIndex = 0;
        var countAnswer = wordList.Count < 5 ? wordList.Count : wordList.Count - 2;
        //foreach (var word in wordList)
        for (int i = 0; i < countAnswer; i++)
        {
            var word = wordList[i];
            var words = wordList.FindAll(wd => wd.Length == word.Length);
            LineWord line = Instantiate(MonoUtils.instance.lineWord);
            //line.answer = word.ToUpper();
            line.numLetters = word.Length;
            line.answers = words;
            line.cellSize = cellSize;
            line.SetLineWidth();
            line.Build(ConfigController.Config.isWordRightToLeft);
            line.name = line.name + lineIndex + "_" + (GameState.currentSubWorld + 1) + (GameState.currentLevel + 1);

            if (useProgress)
            {
                line.SetProgress(levelProgress[lineIndex], answerProgress[lineIndex]);
            }

            line.transform.SetParent(transform);
            line.transform.localScale = Vector3.one;
            line.transform.localPosition = Vector3.zero;
            line.usedBee = CPlayerPrefs.GetBool(line.name);
            if (!line.isShown)
            {
                GetCellShowHint(line);
            }
            else
            {
                listWordCorrect.Add(word.ToLower());
            }

            lines.Add(line);
            listWordInLevel.Add(word.ToLower());
            lineIndex++;
        }
    }

    private void GetCellShowHint(LineWord line)
    {
        foreach (var cell in line.cells)
        {
            //if (!cell.isBee)
            cell.bg.color = new Color(1, 1, 1, 0.5f);
            if (line.usedBee && !cell.isShown)
                cell.iconCoin.transform.localScale = Vector3.one;

        }
    }

    public bool IsUseBee()
    {
        var isUse = lines.Any(line => line.usedBee);
        return isUse;
    }

    private void SetLinesPosition()
    {
        if (numCol >= 2)
        {
            float[] startX = new float[numCol];
            startX[0] = startFirstColX;

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
                float y = (lineY + 1) * gapY + lineY * cellSize;

                lines[i].transform.localPosition = new Vector2(x, y);
            }
        }
        else
        {
            int count = lines.Count;
            for (int i = 0; i < count; i++)
            {
                float x = rt.rect.width / 2 - lines[i].lineWidth / 2;
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
            }
        }
    }
    private int lineIndex = 0;
    public void CheckAnswer(string checkWord)
    {
        //LineWord line = lines.Find(x => x.answer == checkWord);
        LineWord line = lines.Find(x => x.answers.Contains(checkWord) && !x.isShown);
        //string meaning="";
        if (line != null)
        {
            //if (!line.isShown)
            //{
            line.SetDataLetter(checkWord);
            textPreview.SetAnswerColor();
            line.ShowAnswer();
            CheckGameComplete();

            compliment.Show(lineIndex);
            lineIndex++;
            if (lineIndex > compliment.sprites.Length - 1) { lineIndex = compliment.sprites.Length - 1; }

            Sound.instance.Play(Sound.Others.Match);
            listWordCorrect.Add(checkWord.ToLower());
            //}
            //else
            //{
            //    line.ShowFxAnswerDuplicate();
            //    textPreview.SetExistColor();
            //}
            if (textPreview.useFX)
                textPreview.ClearText();
        }
        else
        {
            LineWord lineExist = lines.Find(x => x.answers.Contains(checkWord) && x.isShown);
            if (lineExist != null && lineExist.answer == checkWord)
            {
                lineExist.ShowFxAnswerDuplicate();
                textPreview.SetExistColor();
                if (textPreview.useFX)
                    textPreview.ClearText();
            }
            else
            {
                CheckExtraWordAndWrong(checkWord);
            }
        }
        if (!textPreview.useFX)
            textPreview.ClearText();
    }

    private void SetupCellAds()
    {
        var isAdsHintFree = CPlayerPrefs.GetBool(_textLevel.text + "ADS_HINT_FREE", false);
        _countShowAdsHintFree += 1;
        if (_countShowAdsHintFree > 2 && !isAdsHintFree)
        {
            var lineNotShown = lines.FindAll(l => !l.isShown);
            var lineRandom = lineNotShown[Random.Range(0, lineNotShown.Count)];
            var indexAnswer = Random.Range(0, lineRandom.answers.Count);
            lineRandom.SetDataLetter(lineRandom.answers[indexAnswer]);
            var cellNotShown = lineRandom.cells.FindAll(cell => !cell.isShown);
            var cellRandom = cellNotShown[Random.Range(0, cellNotShown.Count)];
            cellRandom.isAds = true;
            _bntHintADS = Instantiate(btnAdsHintFreePfb, parentAdsHint);
            _bntHintADS.transform.position = cellRandom.transform.position;
            _bntHintADS.Cell = cellRandom;
            _bntHintADS.gameObject.SetActive(true);
            CPlayerPrefs.SetBool(_textLevel.text + "ADS_HINT_FREE", true);
        }
    }
    private void CheckExtraWordAndWrong(string checkWord)
    {
        var noMoreLine = lines.Find(li => li.answers.Contains(checkWord) && li.answer != checkWord && checkWord.Length == li.cells.Count);
        if (/*validWords.Contains(checkWord.ToLower())*/noMoreLine != null)
        {
            ExtraWord.instance.ProcessWorld(checkWord);
            if (textPreview.useFX)
                textPreview.ClearText();
        }
        else
        {
            SetupCellAds();
            textPreview.SetWrongColor();
            lineIndex = 0;
        }
    }
    private void CheckGameComplete()
    {
        SaveLevelProgress();
        var isNotShown = lines.Find(x => !x.isShown);
        if (isNotShown == null)
        {
            ClearLevelProgress();
            MainController.instance.OnComplete();
            if (lines.Count >= 6)
            {
                compliment.ShowRandom();
            }
        }
        FacebookController.instance.SaveDataGame();
    }

    public void BeeClick()
    {
        int count = 0;

        var lineNotShow = lines.FindAll(x => !x.isShown);
        foreach (var line in lineNotShow)
        {
            if (count < 5 && line.cells.Count > 3)
            {
                line.ShowCellUseBee();
                BeeManager.instance.SetAmountBee(-1);
                CheckGameComplete();
                Prefs.AddToNumHint(GameState.currentWorld, GameState.currentSubWorld, GameState.currentLevel);
                count += 1;
            }
        }

        Sound.instance.PlayButton();
    }

    int hintLineIndex = -1;
    public void HintClick()
    {
        int ballance = CurrencyController.GetBalance();
        var hintFree = CurrencyController.GetHintFree();
        if (ballance >= Const.HINT_COST || hintFree > 0)
        {
            LineWord line = null;
            if (hintLineIndex + 1 >= lines.Count) hintLineIndex = -1;
            for (int i = hintLineIndex + 1; i < lines.Count; i++)
            {
                if (!lines[i].isShown)
                {
                    line = lines[i];
                    hintLineIndex = i;
                    break;
                }
            }

            if (line != null)
            {
                line.ShowHint(() =>
                {
                    if (hintFree > 0)
                        CurrencyController.DebitHintFree(hintFree);
                    else
                        CurrencyController.DebitBalance(Const.HINT_COST);
                });
                CheckGameComplete();

                Prefs.AddToNumHint(GameState.currentWorld, GameState.currentSubWorld, GameState.currentLevel);
            }
        }
        else
        {
            DialogController.instance.ShowDialog(DialogType.Shop2);
        }
        Sound.instance.PlayButton();
    }

    public void HintRandomClick()
    {
        int ballance = CurrencyController.GetBalance();
        //var hintFree = CurrencyController.GetHintFree();
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
                            //if (hintFree > 0)
                            //    CurrencyController.DebitHintFree(hintFree);
                            //else
                            CurrencyController.DebitBalance(Const.HINT_RANDOM_COST);
                        });
                        CheckGameComplete();

                        Prefs.AddToNumHint(GameState.currentWorld, GameState.currentSubWorld, GameState.currentLevel);
                    }
                }
            }
        }
        else
        {
            DialogController.instance.ShowDialog(DialogType.Shop2);
        }
        Sound.instance.PlayButton();
    }

    public void SaveLevelProgress()
    {
        if (!Prefs.IsLastLevel()) return;

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
    }

    public string[] GetLevelProgress()
    {
        if (!Prefs.IsLastLevel()) return new string[0]; //nếu đã chơi thì biết đáp án rồi nên ko lưu
        return Prefs.levelProgress;
    }

    public string[] GetAnswerProgress()
    {
        if (!Prefs.IsLastLevel()) return new string[0]; //nếu đã chơi thì biết đáp án rồi nên ko lưu
        return Prefs.answersProgress;
    }

    public void ClearLevelProgress()
    {
        if (!Prefs.IsLastLevel()) return;
        CPlayerPrefs.DeleteKey("level_progress");
    }

    public void ClearAnswerProgress()
    {
        if (!Prefs.IsLastLevel()) return;
        CPlayerPrefs.DeleteKey("answer_progress");
    }

    public bool CheckLevelProgress(string[] levelProgress, List<string> wordList)
    {
        if (levelProgress.Length != wordList.Count) return false;

        for (int i = 0; i < wordList.Count; i++)
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
                UpdateBoard();
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

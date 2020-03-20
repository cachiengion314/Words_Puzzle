using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using PlayFab;
using UnityEngine.UI;

public class WordRegion : MonoBehaviour
{
    public TextPreview textPreview;
    public Compliment compliment;
    public Button btnVideoAds;

    private List<LineWord> lines = new List<LineWord>();
    private List<string> validWords = new List<string>();

    private GameLevel gameLevel;
    private int numWords, numCol, numRow;
    private float cellSize, startFirstColX = 0f;
    private bool hasLongLine;

    private RectTransform rt;
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
    }

    public void Load(GameLevel gameLevel)
    {
        this.gameLevel = gameLevel;

        var wordList = CUtils.BuildListFromString<string>(this.gameLevel.answers);
        validWords = CUtils.BuildListFromString<string>(this.gameLevel.validWords);
        numWords = wordList.Count;

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
        bool useProgress = false;

        if (levelProgress.Length != 0)
        {
            useProgress = CheckLevelProgress(levelProgress, wordList);
            if (!useProgress) ClearLevelProgress();
        }

        SetupLine(wordList, useProgress, levelProgress);

        SetLinesPosition();

        FacebookController.instance.user.levelProgress = levelProgress;
        FacebookController.instance.SaveDataGame();
        CheckGameComplete();
    }

    private void SetupLine(List<string> wordList, bool useProgress, string[] levelProgress)
    {
        int lineIndex = 0;

        foreach (var word in wordList)
        {
            LineWord line = Instantiate(MonoUtils.instance.lineWord);
            line.answer = word.ToUpper();
            line.cellSize = cellSize;
            line.SetLineWidth();
            line.Build(ConfigController.Config.isWordRightToLeft);
            line.name = line.name + lineIndex + "_" + (GameState.currentSubWorld + 1) + (GameState.currentLevel + 1);

            if (useProgress)
            {
                line.SetProgress(levelProgress[lineIndex]);
            }

            line.transform.SetParent(transform);
            line.transform.localScale = Vector3.one;
            line.transform.localPosition = Vector3.zero;
            line.usedBee = CPlayerPrefs.GetBool(line.name);
            if (!line.isShown)
                GetCellShowHint(line);

            lines.Add(line);
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
        LineWord line = lines.Find(x => x.answer == checkWord);
        //string meaning="";
        if (line != null)
        {
            if (!line.isShown)
            {
                textPreview.SetAnswerColor();
                line.ShowAnswer();
                CheckGameComplete();

                compliment.Show(lineIndex);
                lineIndex++;
                if (lineIndex > compliment.sprites.Length - 1) { lineIndex = compliment.sprites.Length - 1; }

                Sound.instance.Play(Sound.Others.Match);
            }
            else
            {
                line.ShowFxAnswerDuplicate();
                textPreview.SetExistColor();
            }
            if (textPreview.useFX)
                textPreview.ClearText();
        }
        else if (validWords.Contains(checkWord.ToLower()))
        {
            ExtraWord.instance.ProcessWorld(checkWord);
            if (textPreview.useFX)
                textPreview.ClearText();
        }
        else
        {
            textPreview.SetWrongColor();
            lineIndex = 0;
        }
        if (!textPreview.useFX)
            textPreview.ClearText();
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
                line.ShowHint();
                if (hintFree > 0)
                    CurrencyController.DebitHintFree(hintFree);
                else
                    CurrencyController.DebitBalance(Const.HINT_COST);
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
                        line.ShowHintRandom();
                        //if (hintFree > 0)
                        //    CurrencyController.DebitHintFree(hintFree);
                        //else
                        CurrencyController.DebitBalance(Const.HINT_RANDOM_COST);
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
        foreach (var line in lines)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var cell in line.cells)
            {
                sb.Append(cell.isShown ? "1" : "0");
            }
            results.Add(sb.ToString());
        }

        Prefs.levelProgress = results.ToArray();
        FacebookController.instance.user.levelProgress = Prefs.levelProgress;
    }

    public string[] GetLevelProgress()
    {
        if (!Prefs.IsLastLevel()) return new string[0]; //nếu đã chơi thì biết đáp án rồi nên ko lưu
        return Prefs.levelProgress;
    }

    public void ClearLevelProgress()
    {
        if (!Prefs.IsLastLevel()) return;
        CPlayerPrefs.DeleteKey("level_progress");
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
        if (progress.Length == 0) return;

        int i = 0;
        foreach (var line in lines)
        {
            line.SetProgress(progress[i]);
            i++;
        }
    }
}

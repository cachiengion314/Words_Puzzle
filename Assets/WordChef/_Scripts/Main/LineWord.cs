using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LineWord : MonoBehaviour
{
    public float spacing = 20f;
    public string answer;
    public List<string> answers;
    public float cellSize;
    public List<Cell> cells = new List<Cell>();
    public int numLetters;
    public float lineWidth;

    public int selectID;
    public bool isShown, RTL;

    public bool usedBee;
    public bool isAds;

    [Space]
    [SerializeField] private Button _btnMeanWord;
    [SerializeField] private Image _fxAnswerDuplicate;
    [Space]
    [SerializeField] private GameObject _fxShowHintPfb;

    private string answerrandom;
    private bool _isResetDataAnswer;

    public void Build(bool RTL)
    {
        this.RTL = RTL;
        answerrandom = answers[Random.Range(0, answers.Count)];
        //numLetters = answer.Length;
        float cellGap = cellSize * Const.CELL_GAP_COEF_X + spacing;

        for (int i = 0; i < numLetters; i++)
        {
            int index = i;
            Cell cell = Instantiate(MonoUtils.instance.cell);
            cell.letter = answer.Length > 0 ? answer[i].ToString() : "";
            //cell.letterText.transform.localScale = Vector3.one * (cellSize / 80f);
            float a = (cellSize / 135f);
            cell.letterText.fontSize = (int)(ConfigController.Config.fontSizeInCellMainScene * a);
            cell.letterText.margin = new Vector4(cell.letterText.margin.x, cell.letterText.margin.y, cell.letterText.margin.z, cell.letterText.margin.w * a);
            cell.bg.rectTransform.sizeDelta = new Vector2(cell.bg.rectTransform.sizeDelta.x * a, cell.bg.rectTransform.sizeDelta.y * a);

            RectTransform cellTransform = cell.GetComponent<RectTransform>();
            cellTransform.SetParent(transform);
            cellTransform.sizeDelta = new Vector2(cellSize, cellSize);
            cellTransform.localScale = Vector3.one;

            float x = cellSize / 2 + i * (cellSize + cellGap);
            float y = cellSize / 2;

            cellTransform.localPosition = new Vector3(x, y);
            cell.name = gameObject.name + "_" + cell.name + index /*+ "_" + (GameState.currentSubWorld + 1) + (GameState.currentLevel + 1)*/;
            cell.isBee = CPlayerPrefs.GetBool(cell.name);
            cell.isAds = CPlayerPrefs.GetBool(cell.name + "_ADS", false);
            cells.Add(cell);
        }
    }

    public void SetLineWidth()
    {
        //int numLetters = answer.Length;
        var rt = GetComponent<RectTransform>();
        lineWidth = numLetters * cellSize + (numLetters - 1) * cellSize * Const.CELL_GAP_COEF_X + spacing * (numLetters - 1);
        rt.sizeDelta = new Vector2(lineWidth, cellSize);
    }

    public void SetDataLetter(string word)
    {
        var lines = WordRegion.instance.Lines;
        if (answer != "")
        {
            foreach (var line in lines)
            {
                if (!line.answers.Contains(answer) && line.cells.Count == answer.Length)
                    line.answers.Add(answer);
            }
        }
        answer = word;
        CPlayerPrefs.SetString(gameObject.name + "_Chapter_" + GameState.currentSubWorld + "_Level_" + GameState.currentLevel, answer);

        for (int i = 0; i < cells.Count; i++)
        {
            int index = i;
            cells[index].letter = word[index].ToString();
        }
    }

    public void SetProgress(string progress, string progressAnswer)
    {
        answer = progressAnswer;
        if (answer != "")
        {
            SetDataLetter(answer);
        }
        isShown = true;
        int i = 0;
        foreach (var cell in cells)
        {
            if (answer.Length == cells.Count)
                cell.letter = answer[i].ToString();
            if (progress[i] == '1')
            {
                cell.isShown = true;
                cell.ShowText();
            }
            else
            {
                isShown = false;
            }
            i++;
        }
        if (isShown)
            WordRegion.instance.listWordCorrect.Add(answer.ToLower());
        ShowBtnMeanByWord();
    }

    public void ShowFxAnswerDuplicate()
    {
        if (_fxAnswerDuplicate != null)
        {
            _fxAnswerDuplicate.gameObject.SetActive(true);
            TweenControl.GetInstance().Scale(_fxAnswerDuplicate.gameObject, Vector3.one * 1.1f, 0.3f, () =>
            {
                TweenControl.GetInstance().Scale(_fxAnswerDuplicate.gameObject, Vector3.one, 0.3f, () =>
                {
                    _fxAnswerDuplicate.gameObject.SetActive(false);
                });
            });
        }
    }

    public void ShowAnswer()
    {
        if (isAds)
        {
            CPlayerPrefs.SetBool(WordRegion.instance.keyLevel + "ADS_HINT_FREE", true);
            WordRegion.instance.BtnADS?.gameObject.SetActive(false);
            isAds = false;
            CPlayerPrefs.SetBool(gameObject.name + "_ADS", isAds);
            CPlayerPrefs.Save();
        }
        Prefs.countSpell += 1;
        Prefs.countSpellDaily += 1;
        _isResetDataAnswer = false;
        isShown = true;
        foreach (var cell in cells)
        {
            cell.isShown = true;
        }
        foreach (var line in WordRegion.instance.Lines)
        {
            if (line != this)
            {
                line.answers.Remove(answer);
                if (!line.isShown && line.cells.Count == answer.Length)
                {
                    line.answer = "";
                    foreach (var an in line.answers)
                    {
                        if (an != answer && !_isResetDataAnswer)
                            ResetAnswer(line, an);
                    }
                }
            }
        }
        WordRegion.instance.listWordCorrect.Add(answer.ToLower());
        ShowBtnMeanByWord();
        StartCoroutine(IEShowAnswer());
    }

    private void ResetAnswer(LineWord line, string ansRight)
    {
        for (int i = 0; i < ansRight.Length; i++)
        {
            var ans = ansRight[i];
            if (line.cells[i].isShown && line.cells[i].letter == ans.ToString())
            {
                line.SetDataLetter(ansRight);
                _isResetDataAnswer = true;
                break;
            }
            else
                line.answer = "";
        }
    }

    public IEnumerator IEShowAnswer()
    {
        if (!RTL)
        {
            foreach (var cell in cells)
            {
                cell.isShown = true;
                cell.Animate();
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            for (int i = cells.Count - 1; i >= 0; i--)
            {
                var cell = cells[i];
                cell.isShown = true;
                cell.Animate();
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private void ShowFxShowHint(Transform objStart, Cell cell, System.Action callback = null)
    {
        BlockScreen.instance.Block(true);
        TutorialController.instance.isBlockSwipe = true;
        var tweenControl = TweenControl.GetInstance();
        var fxShow = Instantiate(_fxShowHintPfb, transform);
        fxShow.transform.position = objStart.position;
        tweenControl.JumpRect(fxShow.transform as RectTransform, cell.transform.localPosition, -800f, 1, 1.3f, false, () =>
        {
            BlockScreen.instance.Block(false);
            TutorialController.instance.isBlockSwipe = false;
            Destroy(fxShow);
            callback?.Invoke();
        }, EaseType.Linear);
    }

    private void ShowCellTarget(bool active)
    {
        foreach (var cell in cells)
        {
            if (!cell.isShown)
            {
                //cell.GetComponent<GraphicRaycaster>().enabled = active;
                cell.btnCellTarget.gameObject.SetActive(active);
                cell.btnCellTarget.onClick.RemoveAllListeners();
                cell.btnCellTarget.onClick.AddListener(() => WordRegion.instance.OnClickCellTarget(cell));
            }
            else
            {
                cell.imgHiden.gameObject.SetActive(true);
                cell.imgHiden.transform.SetAsLastSibling();
            }
        }
    }

    public void HidenOverlayOfCell()
    {
        foreach (var cell in cells)
        {
            cell.imgHiden.gameObject.SetActive(false);
            cell.btnCellTarget.gameObject.SetActive(false);
        }
    }


    public void HighlightCellNotShown()
    {
        ShowCellTarget(true);
    }

    public void ShowHintCelltarget(Cell cellTarget)
    {
        if (answer == "")
        {
            var tempAnswers = answers;
            for (int i = 0; i < WordRegion.instance.Lines.Count; i++)
            {
                var line = WordRegion.instance.Lines[i];
                if (line != this && !line.isShown && line.answer != "")
                    tempAnswers.Remove(line.answer);
            }
            SetDataLetter(tempAnswers[Random.Range(0, tempAnswers.Count)]);
        }
        ShowFxShowHint(WordRegion.instance.btnHintTarget.transform, cellTarget, () =>
        {
            cellTarget.ShowHint();
            CurrencyController.DebitBalance(Const.HINT_TARGET_COST);
            Sound.instance.PlayButton(Sound.Button.Hint);
            CheckSetDataAnswer(answer);
            CheckLineDone();
            WordRegion.instance.SaveLevelProgress();
            WordRegion.instance.CheckGameComplete();
            ClearAds();
        });
    }

    public void OnClickLine()
    {
        DialogController.instance.ShowDialog(DialogType.MeanInGameDialog, DialogShow.REPLACE_CURRENT);
        Sound.instance.Play(Sound.Others.PopupOpen);
        DictionaryInGameDialog.instance.GetIndexByWord(answer);
    }

    private void ShowBtnMeanByWord()
    {
        _btnMeanWord.gameObject.SetActive(isShown);
        if (isShown)
            _btnMeanWord.transform.SetAsLastSibling();
    }

    private void ShowDoneAllCell()
    {
        Prefs.countSpell += 1;
        Prefs.countSpellDaily += 1;
        WordRegion.instance.SetWordOpenInLevelAmount(answer);
        if (answer != "")
        {
            MainController.instance.SaveWordComplete(answer);
            WordRegion.instance.listWordCorrect.Add(answer.ToLower());
        }
        foreach (var cell in cells)
        {
            cell.bg.color = new Color(1, 1, 1, 1);
        }
        WordRegion.instance.ShowComplimentFX();
        WordRegion.instance.ShowBtnDictionaryInGamePlay();
    }

    public void ShowHint(System.Action callback = null)
    {
        if (answer == "")
        {
            //answer = answers[Random.Range(0, answers.Count)];
            //SetDataLetter(answer);
            var tempAnswers = answers;
            for (int i = 0; i < WordRegion.instance.Lines.Count; i++)
            {
                var line = WordRegion.instance.Lines[i];
                if (line != this && !line.isShown && line.answer != "")
                    tempAnswers.Remove(line.answer);
            }
            SetDataLetter(tempAnswers[Random.Range(0, tempAnswers.Count)]);
        }
        var cellNotShow = cells.FindAll(cell => !cell.isShown);
        var indexAnswer = answer.Length - cellNotShow.Count;
        if (!RTL)
        {
            for (int i = 0; i < cellNotShow.Count; i++)
            {
                var cell = cellNotShow[i];
                if (!cell.isShown)
                {
                    //cell.letter = answer[i + indexAnswer].ToString();
                    ShowFxShowHint(WordRegion.instance.btnHint.transform, cell, () =>
                    {
                        cell.ShowHint();
                        CheckSetDataAnswer(answer);
                        CheckLineDone();
                        callback?.Invoke();
                    });
                    return;
                }
            }
        }
        else
        {
            for (int i = cellNotShow.Count - 1; i >= 0; i--)
            {
                var cell = cellNotShow[i];
                if (!cell.isShown)
                {
                    //cell.letter = answer[i + indexAnswer].ToString();
                    ShowFxShowHint(WordRegion.instance.btnHint.transform, cell, () =>
                    {
                        cell.ShowHint();
                        CheckSetDataAnswer(answer);
                        CheckLineDone();
                        callback?.Invoke();
                    });
                    return;
                }
            }
        }
        ClearAds();
    }

    public void CheckLineDone()
    {
        var showDone = cells.All(cel => cel.isShown);
        if (showDone)
        {
            isShown = true;
            ShowDoneAllCell();
        }
    }

    public void ShowHintRandom(System.Action callback = null)
    {
        if (answer == "")
        {
            //answer = answers[Random.Range(0, answers.Count)];
            //SetDataLetter(answer);
            var tempAnswers = answers;
            for (int i = 0; i < WordRegion.instance.Lines.Count; i++)
            {
                var line = WordRegion.instance.Lines[i];
                if (line != this && !line.isShown && line.answer != "")
                    tempAnswers.Remove(line.answer);
            }
            SetDataLetter(tempAnswers[Random.Range(0, tempAnswers.Count)]);
        }
        var cellNotShow = cells.FindAll(cell => !cell.isShown);
        if (cellNotShow != null && cellNotShow.Count > 0)
        {
            var cellRandom = GetRandomCell(cellNotShow);
            ShowFxShowHint(WordRegion.instance.btnMultipleHint.transform, cellRandom, () => {
                if (cellRandom != null)
                {
                    cellRandom.ShowHint();
                }
                CheckSetDataAnswer(answer);
                CheckLineDone();
                ClearAds();
                callback?.Invoke();
            });
        }
    }

    public void ShowCellUseBee(System.Action callback = null)
    {
        if (!CPlayerPrefs.GetBool(gameObject.name))
        {
            Prefs.countBooster += 1;
            Prefs.countBoosterDaily += 1;
            if (answer == "")
            {
                //answer = answers[Random.Range(0, answers.Count)];
                //SetDataLetter(answer);
                var tempAnswers = answers;
                for (int i = 0; i < WordRegion.instance.Lines.Count; i++)
                {
                    var line = WordRegion.instance.Lines[i];
                    if (line != this && !line.isShown && line.answer != "")
                        tempAnswers.Remove(line.answer);
                }
                SetDataLetter(tempAnswers[Random.Range(0, tempAnswers.Count)]);
            }


            CheckSetDataAnswer(answer);
            CheckLineDone();
            usedBee = true;
            CPlayerPrefs.SetBool(gameObject.name, usedBee);
            ClearAds();
        }
    }

    //private IEnumerator CellShowBee()
    //{
    //    var cellNotShow = cells.FindAll(cell => !cell.isShown);
    //    var indexAnswer = answer.Length - cellNotShow.Count;
    //    for (int i = 0; i < cellNotShow.Count; i++)
    //    {
    //        var cell = cellNotShow[i];
    //        if (i == 0)
    //        {
    //            //cell.letter = answer[i + indexAnswer].ToString();
    //            cell.ShowTextBee();
    //        }
    //        else
    //        {
    //            TweenControl.GetInstance().ScaleFromZero(cell.iconCoin.gameObject, 0.5f);
    //        }
    //        yield return new WaitForSeconds(0.2f);
    //    }
    //}

    private void CheckSetDataAnswer(string word)
    {
        var lines = WordRegion.instance.Lines;
        var numOfwordsTheSameCharacter = new List<string>();
        for (int i = 0; i < answers.Count; i++)
        {
            var isRight = false;
            var indexCell = 0;
            foreach (var cell in cells)
            {
                if (cell.isShown && cell.letter == answers[i][indexCell].ToString())
                    isRight = true;
                indexCell++;
            }
            if (isRight)
                numOfwordsTheSameCharacter.Add(answers[i]);
        }

        if (numOfwordsTheSameCharacter.Count < 2)
        {
            foreach (var line in lines)
            {
                if (line != this)
                    line.answers.Remove(word);
            }
        }
    }

    private void ClearAds()
    {
        if (isAds)
        {
            CPlayerPrefs.SetBool(WordRegion.instance.keyLevel + "ADS_HINT_FREE", true);
            WordRegion.instance.BtnADS.gameObject.SetActive(false);
            isAds = false;
            CPlayerPrefs.SetBool(gameObject.name + "_ADS", isAds);
        }
    }

    private void UpdateAnswers()
    {
        foreach (var line in WordRegion.instance.Lines)
        {
            if (line != this)
                line.answers.Remove(answer);
        }
    }
    private Cell GetRandomCell(List<Cell> cells)
    {
        var indexAnswer = answer.Length - cells.Count;
        var index = Random.Range(0, cells.Count - 1);
        //cells[index].letter = answer[index + indexAnswer].ToString();
        return cells[index];
    }
}

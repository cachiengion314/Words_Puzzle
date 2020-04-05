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

    [Space]
    [SerializeField] private Button _btnMeanWord;
    [SerializeField] private Image _fxAnswerDuplicate;

    private string answerrandom;

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
            cell.name = cell.name + index + "_" + (GameState.currentSubWorld + 1) + (GameState.currentLevel + 1);
            cell.isBee = CPlayerPrefs.GetBool(cell.name);
            cells.Add(cell);
        }
    }

    public void SetLineWidth()
    {
        //int numLetters = answer.Length;
        var rt = GetComponent<RectTransform>();
        lineWidth = numLetters * cellSize + (numLetters - 1) * cellSize * Const.CELL_GAP_COEF_X;
        rt.sizeDelta = new Vector2(lineWidth, cellSize);
    }

    public void SetDataLetter(string word)
    {
        answer = word;
        CPlayerPrefs.SetString(gameObject.name + "_Chapter_" + GameState.currentSubWorld + "_Level_" + GameState.currentLevel, answer);
        var lines = WordRegion.instance.Lines;
        foreach (var line in lines)
        {
            if (line != this)
                line.answers.Remove(word);
        }
        for (int i = 0; i < cells.Count; i++)
        {
            int index = i;
            cells[index].letter = word[index].ToString();
        }
    }

    public void SetProgress(string progress, string progressAnswer)
    {
        answer = progressAnswer.Length > 0 ? progressAnswer : "";
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
        Prefs.countSpell += 1;
        Prefs.countSpellDaily += 1;
        isShown = true;
        foreach (var cell in cells)
        {
            cell.isShown = true;
        }
        ShowBtnMeanByWord();
        StartCoroutine(IEShowAnswer());
    }

    public IEnumerator IEShowAnswer()
    {
        if (!RTL)
        {
            foreach (var cell in cells)
            {
                cell.isShown = true;
                cell.Animate();
                yield return new WaitForSeconds(0.15f);
            }
        }
        else
        {
            for (int i = cells.Count - 1; i >= 0; i--)
            {
                var cell = cells[i];
                cell.isShown = true;
                cell.Animate();
                yield return new WaitForSeconds(0.15f);
            }
        }
    }

    public void OnClickLine()
    {
        DialogController.instance.ShowDialog(DialogType.MeanInGameDialog, DialogShow.REPLACE_CURRENT);
        Sound.instance.Play(Sound.Others.PopupOpen);
        DictionaryInGameDialog.instance.ShowMeanWordByID(selectID);
    }

    private void ShowBtnMeanByWord()
    {
        _btnMeanWord.gameObject.SetActive(isShown);
    }

    private void ShowDoneAllCell()
    {
        foreach (var cell in cells)
        {
            cell.bg.color = new Color(1, 1, 1, 1);
        }
    }

    public void ShowHint(System.Action callback = null)
    {
        if (answer == "")
        {
            answer = answers[Random.Range(0, answers.Count)];
            UpdateAnswers();
        }
        var cellNotShow = cells.FindAll(cell => !cell.isShown && !cell.isAds);
        var indexAnswer = answer.Length - cellNotShow.Count;
        if (!RTL)
        {
            for (int i = 0; i < cellNotShow.Count; i++)
            {
                var cell = cellNotShow[i];
                if (!cell.isShown && !cell.isAds)
                {
                    cell.letter = answer[i + indexAnswer].ToString();
                    cell.ShowHint();
                    callback?.Invoke();
                    var showDone = cells.All(cel => cel.isShown);
                    if (showDone)
                    {
                        isShown = true;
                        ShowDoneAllCell();
                    }
                    return;
                }
            }
        }
        else
        {
            for (int i = cellNotShow.Count - 1; i >= 0; i--)
            {
                var cell = cellNotShow[i];
                if (!cell.isShown && !cell.isAds)
                {
                    cell.letter = answer[i + indexAnswer].ToString();
                    cell.ShowHint();
                    callback?.Invoke();
                    var showDone = cells.All(cel => cel.isShown);
                    if (showDone)
                    {
                        isShown = true;
                        ShowDoneAllCell();
                        return;
                    }
                }
            }
        }
    }

    public void ShowHintRandom(System.Action callback = null)
    {
        if (answer == "")
        {
            answer = answers[Random.Range(0, answers.Count)];
            UpdateAnswers();
        }
        var cellNotShow = cells.FindAll(cell => !cell.isShown && !cell.isAds);
        var cellRandom = GetRandomCell(cellNotShow);
        if (cellRandom != null)
        {
            cellRandom.ShowHint();
            callback?.Invoke();
        }
        var showDone = cells.All(cell => cell.isShown);
        if (showDone)
        {
            isShown = true;
            ShowDoneAllCell();
        }

    }

    public void ShowCellUseBee(System.Action callback = null)
    {
        if (!CPlayerPrefs.GetBool(gameObject.name))
        {
            if (answer == "")
            {
                answer = answers[Random.Range(0, answers.Count)];
                UpdateAnswers();
            }
            var cellNotShow = cells.FindAll(cell => !cell.isShown && !cell.isAds);
            var indexAnswer = answer.Length - cellNotShow.Count;
            for (int i = 0; i < cellNotShow.Count; i++)
            {
                var cell = cellNotShow[i];
                if (i == 0)
                {
                    cell.letter = answer[i + indexAnswer].ToString();
                    cell.ShowTextBee();
                }
                else
                {
                    TweenControl.GetInstance().ScaleFromZero(cell.iconCoin.gameObject, 0.5f);
                }
            }
            var showDone = cells.All(cell => cell.isShown);
            if (showDone)
            {
                isShown = true;
                ShowDoneAllCell();
            }
            usedBee = true;
            CPlayerPrefs.SetBool(gameObject.name, usedBee);
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
        cells[index].letter = answer[index + indexAnswer].ToString();
        return cells[index];
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LineWord : MonoBehaviour
{

    public string answer;
    public List<string> answers;
    public float cellSize;
    public List<Cell> cells = new List<Cell>();
    public int numLetters;
    public float lineWidth;

    //[HideInInspector]
    public bool isShown, RTL;

    public bool usedBee;

    [Space]

    [SerializeField] private Image _fxAnswerDuplicate;

    public void Build(bool RTL)
    {
        this.RTL = RTL;
        //numLetters = answer.Length;
        float cellGap = cellSize * Const.CELL_GAP_COEF_X;

        for (int i = 0; i < numLetters; i++)
        {
            int index = i;
            Cell cell = Instantiate(MonoUtils.instance.cell);
            cell.letter = /*answer[i].ToString()*/"";
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

    public void SetProgress(string progress)
    {
        isShown = true;
        int i = 0;
        foreach (var cell in cells)
        {
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

    private void ShowDoneAllCell()
    {
        foreach (var cell in cells)
        {
            cell.bg.color = new Color(1, 1, 1, 1);
        }
    }

    public void ShowHint(System.Action callback = null)
    {
        var cellNotShow = cells.FindAll(cell => !cell.isShown && !cell.isAds);
        if (!RTL)
        {
            for (int i = 0; i < cellNotShow.Count; i++)
            {
                var cell = cellNotShow[i];
                if (!cell.isShown && !cell.isAds)
                {
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
                var showDone = cells.All(cel => cel.isShown);
                if (showDone)
                {
                    cell.ShowHint();
                    if (i == 0)
                    {
                        isShown = true;
                        ShowDoneAllCell();
                    }
                    return;
                }
            }
        }
    }

    public void ShowHintRandom(System.Action callback = null)
    {
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
            var cellNotShow = cells.FindAll(cell => !cell.isShown && !cell.isAds);
            for (int i = 0; i < cellNotShow.Count; i++)
            {
                var cell = cellNotShow[i];
                if (i == 0)
                {
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

    private Cell GetRandomCell(List<Cell> cells)
    {
        var index = Random.Range(0, cells.Count - 1);
        return cells[index];
    }
}

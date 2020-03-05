using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineWord : MonoBehaviour
{

    public string answer;
    public float cellSize;
    public List<Cell> cells = new List<Cell>();
    public int numLetters;
    public float lineWidth;

    //[HideInInspector]
    public bool isShown, RTL;

    public bool usedBee;

    public void Build(bool RTL)
    {
        this.RTL = RTL;
        numLetters = answer.Length;
        float cellGap = cellSize * Const.CELL_GAP_COEF_X;

        for (int i = 0; i < numLetters; i++)
        {
            Cell cell = Instantiate(MonoUtils.instance.cell);
            cell.letter = answer[i].ToString();
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
            cells.Add(cell);
        }
    }

    public void SetLineWidth()
    {
        int numLetters = answer.Length;
        var rt = GetComponent<RectTransform>();
        lineWidth = numLetters * cellSize + (numLetters - 1) * cellSize * Const.CELL_GAP_COEF_X;
        rt.sizeDelta = new Vector2(lineWidth, cellSize);
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

    public void ShowAnswer()
    {
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

    public void ShowHint()
    {
        if (!RTL)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                var cell = cells[i];
                if (!cell.isShown)
                {
                    cell.ShowHint();
                    if (i == cells.Count - 1)
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
            for (int i = cells.Count - 1; i >= 0; i--)
            {
                var cell = cells[i];
                if (!cell.isShown)
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

    public void ShowHintRandom()
    {
        var cellNotShow = cells.FindAll(cell => !cell.isShown);
        var cellRandom = GetRandomCell(cellNotShow);
        if (cellRandom != null)
            cellRandom.ShowHint();
        var showDone = cells.All(cell => cell.isShown);
        if (showDone)
        {
            isShown = true;
            ShowDoneAllCell();
        }

    }

    public void ShowCellUseBee()
    {
        if (!CPlayerPrefs.GetBool(gameObject.name))
        {
            var cellNotShow = cells.FindAll(cell => !cell.isShown);
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

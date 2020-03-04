using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class TextPreview : MonoBehaviour {
    public GameObject content;
    public RectTransform backgroundRT;
    public string word;
    public TextInImg textPrefab;
    public Transform textGrid;

    [Header("")]
    public Color answerColor;
    public Color validColor;
    public Color wrongColor;
    public Color existColor;
    public Color defaultColor;

    public static TextPreview instance;

    CanvasGroup canvasGroup;
    float timeFade = 0f;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        canvasGroup = content.GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (textGrid.childCount > 0)
        {
            canvasGroup.alpha = 1f;
            timeFade = 0.5f;
        }
        else
        {
            canvasGroup.alpha = timeFade / 0.5f;
            timeFade -= Time.deltaTime;
        }
    }

    public void SetIndexes(List<int> indexes)
    {
        StringBuilder sb = new StringBuilder();

        if (!ConfigController.Config.isWordRightToLeft)
        {
            foreach (var i in indexes)
            {
                sb.Append(word[i]);
            }
        }
        else
        {
            for(int i = indexes.Count - 1; i >= 0; i--)
            {
                int letterIndex = indexes[i];
                sb.Append(word[letterIndex]);
            }
        }

        foreach (Transform item in textGrid)
        {
            Destroy(item.gameObject);
        }

        for (int i = 0; i < sb.Length; i++)
        {
            var text = GameObject.Instantiate<TextInImg>(textPrefab, textGrid);
            text.text.text = sb[i].ToString();
            text.gameObject.SetActive(true);
        }
        backgroundRT.sizeDelta = new Vector2(sb.Length * 120 + 200, backgroundRT.sizeDelta.y);
    }

    public void SetActive(bool isActive)
    {
        content.SetActive(isActive && textGrid.childCount > 0);
    }

    public void ClearText()
    {
        foreach (Transform item in textGrid)
        {
            Destroy(item.gameObject);
        }
    }

    public string GetText()
    {
        string s = "";
        foreach (Transform item in textGrid)
        {
            s += item.GetComponent<TextInImg>().text.text;
        }
        return s;
    }

    public void SetAnswerColor()
    {
        backgroundRT.GetComponent<Image>().color = answerColor;
    }

    public void SetValidColor()
    {
        backgroundRT.GetComponent<Image>().color = validColor;
    }

    public void SetWrongColor()
    {
        backgroundRT.GetComponent<Image>().color = wrongColor;
    }

    public void SetDefaultColor()
    {
        backgroundRT.GetComponent<Image>().color = defaultColor;
    }

    public void SetExistColor()
    {
        backgroundRT.GetComponent<Image>().color = existColor;
    }
}

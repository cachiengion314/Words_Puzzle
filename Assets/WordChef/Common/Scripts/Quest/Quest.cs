using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Utilities.Components;

[System.Serializable]
public class Quest :MonoBehaviour
{
    //public string title;
    //public int reward;
    //public int numGoal; 
    public QuestGoal goal;
    public ComboType combo;

    [SerializeField]
    GameObject titleText;
    [SerializeField]
    GameObject rewardText;

    [SerializeField] private Slider _fillProgress;
    [SerializeField] private Button _btnGo;
    [SerializeField] private Button _btnReward;
    [SerializeField] private Image _iconCompleta;

    bool iscomplete;
    void OnEnable()
    {
        _iconCompleta.gameObject.SetActive(false);
        ShowQuest();
    }

    void Update()
    {
        if (_fillProgress.value >= goal.requiredAmount)
        {
            _fillProgress.value = _fillProgress.maxValue;
            iscomplete = goal.isReached();
            ShowReward(true);
        }
    }

    private void ShowReward(bool show)
    {
        _btnReward.gameObject.SetActive(show);
        _btnGo.gameObject.SetActive(!show);
        var iscompleted = CPlayerPrefs.GetBool("Completed" + gameObject.name + transform.GetSiblingIndex(), false);
        if(iscompleted)
        {
            _btnReward.gameObject.SetActive(false);
            _btnGo.gameObject.SetActive(false);
            _iconCompleta.gameObject.SetActive(true);
            _fillProgress.value = _fillProgress.maxValue;
        }
    }
    /// <summary>
    /// Clear Task by key name
    /// </summary>
    /// <param name="key">Key name</param>
    private void ClearTask(string key)
    {
        CPlayerPrefs.DeleteKey(key);
    }

    /// <summary>
    /// Clear All Task
    /// </summary>
    private void ClearTask()
    {
        CPlayerPrefs.DeleteKey("Spelling_goal");
        CPlayerPrefs.DeleteKey("Level_Amount");
        CPlayerPrefs.DeleteKey("Chapter_Amount");
        CPlayerPrefs.DeleteKey("Good_Amount");
        CPlayerPrefs.DeleteKey("Great_Amount");
        CPlayerPrefs.DeleteKey("Amazing_Amount");
        CPlayerPrefs.DeleteKey("Awesome_Amount");
        CPlayerPrefs.DeleteKey("Excellent_Amount");
    }

    public void OnReward()
    {
        CurrencyController.CreditBalance(goal.reward);
        _btnReward.gameObject.SetActive(false);
        _iconCompleta.gameObject.SetActive(true);
        CPlayerPrefs.SetBool("Completed" + gameObject.name + transform.GetSiblingIndex(), true);
        ClearTaskByKey();
    }

    void ShowQuest()
    {
        gameObject.GetComponent<SimpleTMPButton>().labelTMP.SetText("+" + goal.requiredAmount);
        _fillProgress.maxValue = goal.requiredAmount;
        switch (goal.goalType)
        {
            case GoalType.Spelling:
                titleText.GetComponent<TextMeshProUGUI>().text = "Spell " + goal.requiredAmount + " Words";
                rewardText.GetComponent<TextMeshProUGUI>().text = "+" + goal.reward.ToString();
                _fillProgress.value = Prefs.countSpell;
                break;
            case GoalType.LevelClear:
                titleText.GetComponent<TextMeshProUGUI>().text = "Clear " + goal.requiredAmount + " levels";
                rewardText.GetComponent<TextMeshProUGUI>().text = "+" + goal.reward.ToString();
                _fillProgress.value = Prefs.countLevel;
                break;
            case GoalType.ChappterClear:
                titleText.GetComponent<TextMeshProUGUI>().text = "Clear " + goal.requiredAmount + " levels";
                rewardText.GetComponent<TextMeshProUGUI>().text = "+" + goal.reward.ToString();
                _fillProgress.value = Prefs.countChapter;
                break;
            case GoalType.Combos:
                if (combo == ComboType.amazing)
                {
                    titleText.GetComponent<TextMeshProUGUI>().text = "Get " + goal.requiredAmount + " AMAZING combos";
                    _fillProgress.value = Prefs.countAmazing;
                }
                else if (combo == ComboType.awesome)
                {
                    titleText.GetComponent<TextMeshProUGUI>().text = "Get " + goal.requiredAmount + " AWESOME combos";
                    _fillProgress.value = Prefs.countAwesome;
                }
                else if (combo == ComboType.excelent)
                {
                    titleText.GetComponent<TextMeshProUGUI>().text = "Get " + goal.requiredAmount + " EXCELENT combos";
                    _fillProgress.value = Prefs.countExcellent;
                }
                else if (combo == ComboType.good)
                {
                    titleText.GetComponent<TextMeshProUGUI>().text = "Get " + goal.requiredAmount + " GOOD combos";
                    _fillProgress.value = Prefs.countGood;
                }
                else if (combo == ComboType.great)
                {
                    titleText.GetComponent<TextMeshProUGUI>().text = "Get " + goal.requiredAmount + " GREAT combos";
                    _fillProgress.value = Prefs.countGreat;
                }
                rewardText.GetComponent<TextMeshProUGUI>().text = "+" + goal.reward.ToString();
                break;      
        }
        if(_fillProgress.value < _fillProgress.maxValue)
        {
            ShowReward(false);
        }
    }

    private void ClearTaskByKey()
    {
        switch (goal.goalType)
        {
            case GoalType.Spelling:
                ClearTask("Spelling_goal");
                break;
            case GoalType.LevelClear:
                ClearTask("Level_Amount");
                break;
            case GoalType.ChappterClear:
                ClearTask("Chapter_Amount");
                break;
            case GoalType.Combos:
                if (combo == ComboType.amazing)
                {
                    ClearTask("Amazing_Amount");
                }
                else if (combo == ComboType.awesome)
                {
                    ClearTask("Awesome_Amount");
                }
                else if (combo == ComboType.excelent)
                {
                    ClearTask("Excellent_Amount");
                }
                else if (combo == ComboType.good)
                {
                    ClearTask("Good_Amount");
                }
                else if (combo == ComboType.great)
                {
                    ClearTask("Great_Amount");
                }
                break;
        }
    }

}



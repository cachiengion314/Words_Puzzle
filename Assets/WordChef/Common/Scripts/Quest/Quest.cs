using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


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
    bool iscomplete;
    private void Start()
    {
        ShowQuest();
    }

    void ShowQuest()
    {
        switch (goal.goalType)
        {
            case GoalType.Spelling:
                titleText.GetComponent<TextMeshProUGUI>().text = "Spell " + goal.requiredAmount + " Words";
                rewardText.GetComponent<TextMeshProUGUI>().text = "+" + goal.reward.ToString();
                break;
            case GoalType.LevelClear:
                titleText.GetComponent<TextMeshProUGUI>().text = "Clear " + goal.requiredAmount + " levels";
                rewardText.GetComponent<TextMeshProUGUI>().text = "+" + goal.reward.ToString();
                break;
            case GoalType.Combos:
                if (combo == ComboType.amazing)
                {
                    titleText.GetComponent<TextMeshProUGUI>().text = "Get " + goal.requiredAmount + " AMAZING combos";
                }
                else if (combo == ComboType.awesome)
                {
                    titleText.GetComponent<TextMeshProUGUI>().text = "Get " + goal.requiredAmount + " AWESOME combos";
                }
                else if (combo == ComboType.excelent)
                {
                    titleText.GetComponent<TextMeshProUGUI>().text = "Get " + goal.requiredAmount + " EXCELENT combos";
                }
                else if (combo == ComboType.good)
                {
                    titleText.GetComponent<TextMeshProUGUI>().text = "Get " + goal.requiredAmount + " GOOD combos";
                }
                else if (combo == ComboType.great)
                {
                    titleText.GetComponent<TextMeshProUGUI>().text = "Get " + goal.requiredAmount + " GREAT combos";
                }
                rewardText.GetComponent<TextMeshProUGUI>().text = "+" + goal.reward.ToString();
                break;      
        }
    }

}



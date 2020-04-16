using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExtraWordDialog : Dialog
{
    public Transform claimTr;
    public ExtraProgress extraProgress;
    public GameObject claimButton;
    public Text progressText;
    public TextMeshProUGUI wordText;
    public TextMeshProUGUI claimQuantityText;

    private int numWords, claimQuantity;

    protected override void Start()
    {
        base.Start();
        Prefs.extraTarget = 2;
        extraProgress.target = Prefs.extraTarget;
        extraProgress.current = Prefs.extraProgress;
        claimQuantity = (int)extraProgress.target / 2 * 40;

        UpdateUI();
    }

    public void OnClickHTPL(int selectID)
    {
        DialogController.instance.ShowDialog(DialogType.HowtoPlay, DialogShow.STACK);
        Sound.instance.Play(Sound.Others.PopupOpen);
        HowToPlayDialog.instance.ShowMeanWordByID(selectID);
    }

    public void Claim()
    {
        extraProgress.current -= (int)extraProgress.target;
        Prefs.extraProgress = (int)extraProgress.current;
        UpdateUI();

        StartCoroutine(ClaimEffect());
        ExtraWord.instance.OnClaimed();

        if (Prefs.extraTarget == 2 && Prefs.totalExtraAdded > 2)
        {
            Prefs.extraTarget = 4;
            extraProgress.target = 4;
            claimQuantity = (int)extraProgress.target / 2 * 40;
            UpdateUI();
        }
    }

    private IEnumerator ClaimEffect()
    {
        Transform rubyBalance = GameObject.FindWithTag("RubyBalance").transform;
        //var middlePoint = CUtils.GetMiddlePoint(claimTr.position, rubyBalance.position, -0.4f);
        //Vector3[] waypoints = { claimTr.position, middlePoint, rubyBalance.position };
        for (int i = 0; i < claimQuantity; i++)
        {
            if(i < 2)
                MonoUtils.instance.ShowEffect(claimQuantity / 2, rubyBalance);
            yield return new WaitForSeconds(0.02f);
        }
    }

    private void UpdateUI()
    {
        claimQuantityText.text = claimQuantity.ToString();
        claimButton.SetActive(extraProgress.current >= extraProgress.target);
        progressText.text = extraProgress.current + "/" + extraProgress.target;
        wordText.text = "";
        foreach (var word in ExtraWord.instance.extraWords)
        {
            wordText.text += "  " + word.ToUpper();
        }
    }
}

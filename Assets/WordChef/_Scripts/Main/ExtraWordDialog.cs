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
        extraProgress.target = Prefs.extraTarget;
        extraProgress.current = Prefs.extraProgress;
        claimQuantity = (int)extraProgress.target / 5 * 40;

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

        if (Prefs.extraTarget == 5 && Prefs.totalExtraAdded > 10)
        {
            Prefs.extraTarget = 10;
            extraProgress.target = 10;
            claimQuantity = (int)extraProgress.target / 5 * 40;
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
            if(i < 5)
                MonoUtils.instance.ShowEffect(claimQuantity / 5, rubyBalance);
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

    public override void Close()
    {
        base.Close();
        ExtraWord.instance.effectLightLoop.gameObject.SetActive(true);
    }
}

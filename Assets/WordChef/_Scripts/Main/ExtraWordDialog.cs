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

    private int numWords, claimQuantity;

    protected override void Start()
    {
        base.Start();
        extraProgress.target = Prefs.extraTarget;
        extraProgress.current = Prefs.extraProgress;

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
        claimQuantity = (int)extraProgress.target / 5 * 2;

        extraProgress.current -= (int)extraProgress.target;
        Prefs.extraProgress = (int)extraProgress.current;
        UpdateUI();

        StartCoroutine(ClaimEffect());
        ExtraWord.instance.OnClaimed();

        if (Prefs.extraTarget == 5 && Prefs.totalExtraAdded > 10)
        {
            Prefs.extraTarget = 10;
            extraProgress.target = 10;
            UpdateUI();
        }
    }

    private IEnumerator ClaimEffect()
    {
        var tweenControl = TweenControl.GetInstance();
        Transform rubyBalance = GameObject.FindWithTag("RubyBalance").transform;
        var middlePoint = CUtils.GetMiddlePoint(claimTr.position, rubyBalance.position, -0.4f);
        Vector3[] waypoints = { claimTr.position, middlePoint, rubyBalance.position };

        for (int i = 0; i < claimQuantity; i++)
        {
            GameObject gameObj = Instantiate(MonoUtils.instance.rubyFly, MonoUtils.instance.textFlyTransform);
            gameObj.transform.position = /*waypoints[0]*/Vector3.zero;
            gameObj.transform.localScale = Vector3.one;

            tweenControl.Move(gameObj.transform, rubyBalance.position, 0.5f, () =>
            {
                CurrencyController.CreditBalance(claimQuantity / 4);
                Sound.instance.Play(Sound.Collects.CoinCollect);
                Destroy(gameObj);
            }, EaseType.InBack);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void UpdateUI()
    {
        claimButton.SetActive(extraProgress.current >= extraProgress.target);
        progressText.text = extraProgress.current + "/" + extraProgress.target;
        wordText.text = "";
        foreach (var word in ExtraWord.instance.extraWords)
        {
            wordText.text += "  " + word.ToUpper();
        }
    }
}

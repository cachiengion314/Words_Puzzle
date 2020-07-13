using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class MonoUtils : MonoBehaviour
{
    public Text letter;
    public Cell cell;
    public LineWord lineWord;
    public Transform textFlyTransform;
    public Transform posDefault;
    public Transform rootDefault;
    public GameObject rubyFly;
    public GameObject levelButton;
    public TextMeshProUGUI textCollectDefault;

    private bool showCollect;
    public static MonoUtils instance;

    private void Awake()
    {
        instance = this;
    }

    public void ShowEffect(int value, Transform currBalance = null, Transform root = null, Transform posStart = null, GameObject starPfb = null, float power = -800f)
    {
        var tweenControl = TweenControl.GetInstance();
        var star = Instantiate(starPfb == null ? rubyFly : starPfb, root == null ? rootDefault : root);
        star.gameObject.SetActive(true);
        star.transform.SetAsFirstSibling();
        star.transform.position = (posStart != null ? posStart : root == null ? rootDefault : root).position;
        star.transform.localScale = Vector3.zero;
        //tweenControl.Move(star.transform, (currBalance != null ? currBalance : GameObject.FindWithTag("RubyBalance").transform).position, 0.5f, () =>
        //{
        //    CurrencyController.CreditBalance(value);
        //    Sound.instance.Play(Sound.Collects.CoinCollect);
        //    Destroy(star);
        //}, EaseType.InBack);
        var targetShow = new Vector3(star.transform.localPosition.x, star.transform.localPosition.y -
            (posStart != null ? (posStart as RectTransform).rect.height /** 1.3f */: (star.transform as RectTransform).rect.height));
        //tweenControl.MoveLocal(star.transform, targetShow, 0.3f, () =>
        //{
        //tweenControl.MoveLocal(star.transform, targetShow - new Vector3(100, 50, 0), 0.2f, () =>
        //  {
        tweenControl.JumpRect(star.transform as RectTransform, (currBalance != null ? currBalance as RectTransform : posDefault as RectTransform).anchoredPosition, power, 1, 1.3f, false, () =>
        {
            CurrencyController.CreditBalance(value);
            Sound.instance.Play(Sound.Collects.CoinCollect);
            star.gameObject.SetActive(false);
            Destroy(star, 0.1f);
        }, EaseType.Linear);
        //});
        //});
        tweenControl.Scale(star.gameObject, Vector3.one * 0.6f, 0.3f, () =>
        {
            tweenControl.Scale(star.gameObject, Vector3.one, 1f);
        });
    }

    public void ShowTotalStarCollect(int value, TextMeshProUGUI textCollect, float timeDelay = 1.6f)
    {
        textCollectDefault.font = ThemesControl.instance.CurrTheme.fontData.fontAsset;
        if (showCollect)
            return;
        var tweenControl = TweenControl.GetInstance();
        (textCollect != null ? textCollect : textCollectDefault).text = "X" + value;
        tweenControl.DelayCall((textCollect != null ? textCollect : textCollectDefault).transform, timeDelay, () =>
        {
            showCollect = true;
            tweenControl.FadeAnfaText(textCollect != null ? textCollect : textCollectDefault, 1, 0.5f, () =>
            {
                tweenControl.FadeAnfaText(textCollect != null ? textCollect : textCollectDefault, 0, 0.3f, () =>
                {
                    showCollect = false;
                });
            });
        });
    }
}

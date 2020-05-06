using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MonoUtils : MonoBehaviour {
    public Text letter;
    public Cell cell;
    public LineWord lineWord;
    public Transform textFlyTransform;
    public Transform posDefault;
    public Transform rootDefault;
    public GameObject rubyFly;
    public GameObject levelButton;

    public static MonoUtils instance;

    private void Awake()
    {
        instance = this;
    }

    public void ShowEffect(int value,Transform currBalance = null, Transform root = null)
    {
        var tweenControl = TweenControl.GetInstance();
        var star = Instantiate(rubyFly, root == null ? rootDefault : root);
        star.transform.position = (root == null ? rootDefault : root).position;
        star.transform.localScale = Vector3.one;
        //tweenControl.Move(star.transform, (currBalance != null ? currBalance : GameObject.FindWithTag("RubyBalance").transform).position, 0.5f, () =>
        //{
        //    CurrencyController.CreditBalance(value);
        //    Sound.instance.Play(Sound.Collects.CoinCollect);
        //    Destroy(star);
        //}, EaseType.InBack);
        tweenControl.JumpRect(star.transform as RectTransform, (currBalance != null ? currBalance as RectTransform : posDefault as RectTransform).anchoredPosition, -500f, 1, 1f, false,()=> {
            CurrencyController.CreditBalance(value);
            Sound.instance.Play(Sound.Collects.CoinCollect);
            Destroy(star);
        },EaseType.OutQuad);
    }
}

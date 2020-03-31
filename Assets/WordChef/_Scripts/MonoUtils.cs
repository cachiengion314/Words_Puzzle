using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MonoUtils : MonoBehaviour {
    public Text letter;
    public Cell cell;
    public LineWord lineWord;
    public Transform textFlyTransform;
    public GameObject rubyFly;
    public GameObject levelButton;

    public static MonoUtils instance;

    private void Awake()
    {
        instance = this;
    }

    public void ShowEffect(int value,Transform currBalance = null)
    {
        var tweenControl = TweenControl.GetInstance();
        var star = Instantiate(rubyFly, textFlyTransform);
        star.transform.position = Vector3.zero;
        star.transform.localScale = Vector3.one;
        tweenControl.Move(star.transform, (currBalance != null ? currBalance : GameObject.FindWithTag("RubyBalance").transform).position, 0.5f, () =>
        {
            CurrencyController.CreditBalance(value);
            Sound.instance.Play(Sound.Collects.CoinCollect);
            Destroy(star);
        }, EaseType.InBack);
    }
}

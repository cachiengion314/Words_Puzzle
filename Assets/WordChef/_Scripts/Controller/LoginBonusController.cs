using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginBonusController : MonoBehaviour
{
    public Action onCollectSpinCallback;
    public static LoginBonusController instance;
    [HideInInspector] public bool isShowLoginbonus;
    [SerializeField] private bool _hidenSpin;
    [SerializeField] private GameObject _root;
    [SerializeField] private GameObject _objSpin;
    [SerializeField] private GameObject _imageHighlight;
    [SerializeField] private CurrencyBallance _currencyBallance;
    [SerializeField] private int _numGift = 6;
    [SerializeField] private int _angleStart = 0;
    [SerializeField] private ItemManager _itemManager;
    [SerializeField] private PanelCollectItem _panelCollect;
    [SerializeField] private AudioClip _fxSpin;
    [Header("Data Spin")]
    [SerializeField] private List<DataItem> _dataItems;

    private int _currAngle;

    void Awake()
    {
        if (instance == null)
            instance = this;
        SetupSpin();
    }

    void Start()
    {
        CheckToday();
    }

    private void SetupSpin()
    {
        _root.transform.localScale = Vector3.zero;
        _panelCollect.transform.localScale = Vector3.zero;
        _imageHighlight.SetActive(false);
        for (int i = 0; i < _itemManager.items.Count; i++)
        {
            var item = _itemManager.items[i];
            item.txtValue.text = _dataItems[i].value.ToString();
            item.image.sprite = _dataItems[i].sprite;
            item.image.SetNativeSize();
        }
    }

    private void CheckToday()
    {
        var firstPlay = CPlayerPrefs.GetBool("FIRST", true);
        var oldDate = DateTime.FromBinary(CPlayerPrefs.GetLong("Daily", DateTime.Now.Date.ToBinary()));
        var currDate = DateTime.Now.Date;
        var showBonus = (DateTime.Compare(currDate, oldDate) > 0) ? true : false;
        BlockScreen.instance.Block(true);
        if (showBonus || firstPlay)
        {
            isShowLoginbonus = true;
            TweenControl.GetInstance().DelayCall(transform, 2f, () =>
            {
                BlockScreen.instance.Block(false);
                if (!_hidenSpin)
                    TweenControl.GetInstance().ScaleFromZero(_root, 0.3f, null);
            });
        }
        else
        {
            isShowLoginbonus = false;
            BlockScreen.instance.Block(false);
        }
    }

    public void OnSpinClick()
    {
        CPlayerPrefs.SetLong("Daily", DateTime.Now.Date.ToBinary());
        Spin();
    }

    public void OnSpinAgainClick()
    {
        var balance = CurrencyController.GetBalance();
        if (balance >= Const.SPIN_AGAIN)
        {
            CurrencyController.DebitBalance(Const.SPIN_AGAIN);
            _panelCollect.transform.localScale = Vector3.zero;
            Spin();
        }
        else
        {
            DialogController.instance.ShowDialog(DialogType.Shop);
        }
    }

    public void OnCollectClick()
    {
        Sound.instance.Play(Sound.Collects.CoinKeep);
        var itemValue = _dataItems[_currAngle].value;
        _panelCollect.transform.localScale = Vector3.zero;
        _root.transform.localScale = Vector3.zero;
        switch (_dataItems[_currAngle].itemType)
        {
            case ItemType.CURRENCY_BALANCE:
                StartCoroutine(ShowEffectCollect(itemValue));
                break;
        }
        RemoteConfigFirebase.instance.notifyIngameCall?.Invoke();
        onCollectSpinCallback?.Invoke();
    }

    private void Spin(Action callback = null)
    {
        _imageHighlight.SetActive(false);
        CPlayerPrefs.SetBool("FIRST", false);
        var itemRandom = UnityEngine.Random.Range(0, _numGift);
        _currAngle = itemRandom;
        var angle = Angle() - (360f / _numGift) * itemRandom;
        Sound.instance.Play(_fxSpin);
        TweenControl.GetInstance().LocalRotate(_objSpin.transform, new Vector3(0, 0, -angle), _fxSpin.length, () =>
        {
            isShowLoginbonus = false;
            _imageHighlight.SetActive(true);
            var currItem = _dataItems[_currAngle];
            _panelCollect.ShowItemCollect(currItem.spriteDone, currItem.value);
            var itemValue = currItem.value;
            switch (_dataItems[_currAngle].itemType)
            {
                case ItemType.HINT:
                    CurrencyController.CreditHintFree(itemValue);
                    break;
                case ItemType.HINT_RANDOM:
                    CurrencyController.CreditMultipleHintFree(itemValue);
                    break;
                case ItemType.HINT_SELECT:
                    CurrencyController.CreditSelectedHintFree(itemValue);
                    break;
            }
            TweenControl.GetInstance().ScaleFromZero(_panelCollect.gameObject, 0.3f, () => { callback?.Invoke(); });
        }, EaseType.OutQuad);
    }

    private float Angle()
    {
        var angle = _angleStart + 360f * 5f;
        return angle;
    }

    private IEnumerator ShowEffectCollect(int value)
    {
        for (int i = 0; i < value; i++)
        {
            if (i < 5)
            {
                MonoUtils.instance.ShowEffect(value / 5);
            }
            yield return new WaitForSeconds(0.06f);
        }

    }
}

[Serializable]
public struct DataItem
{
    public ItemType itemType;
    public int value;
    public Sprite sprite;
    public Sprite spriteDone;
}

public enum ItemType
{
    HINT,
    HINT_RANDOM,
    HINT_SELECT,
    CURRENCY_BALANCE
}
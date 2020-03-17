using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginBonusController : MonoBehaviour
{
    public static LoginBonusController instance;
    [SerializeField] private GameObject _root;
    [SerializeField] private GameObject _objSpin;
    [SerializeField] private int _numGift = 6;
    [SerializeField] private int _angleStart = 0;
    [SerializeField] private ItemManager _itemManager;
    [SerializeField] private PanelCollectItem _panelCollect;
    [Header("Data Spin")]
    [SerializeField] private List<DataItem> _dataItems;

    private int _currAngle;

    void Awake()
    {
        if (instance == null)
            instance = this;
        _root.transform.localScale = Vector3.zero;
        _panelCollect.transform.localScale = Vector3.zero;
        SetupSpin();
    }

    void Start()
    {
        CheckToday();
    }

    private void SetupSpin()
    {
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
            TweenControl.GetInstance().DelayCall(transform, 2f, () =>
            {
                BlockScreen.instance.Block(false);
                TweenControl.GetInstance().ScaleFromZero(_root, 0.3f, null);
            });
        }
        else
        {
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
        _panelCollect.transform.localScale = Vector3.zero;
        _root.transform.localScale = Vector3.zero;
        var itemValue = _dataItems[_currAngle].value;
        switch (_dataItems[_currAngle].itemType)
        {
            case ItemType.HINT:
                CurrencyController.SetHintFree(itemValue + CurrencyController.GetHintFree());
                break;
            case ItemType.HINT_RANDOM:

                break;
            case ItemType.CURRENCY_BALANCE:
                CurrencyController.CreditBalance(itemValue);
                break;
        }
    }

    private void Spin(Action callback = null)
    {
        CPlayerPrefs.SetBool("FIRST", false);
        var itemRandom = UnityEngine.Random.Range(0, _numGift);
        _currAngle = itemRandom;
        var angle = Angle() - (360f / _numGift) * itemRandom;
        TweenControl.GetInstance().LocalRotate(_objSpin.transform, new Vector3(0, 0, -angle), 5f, () =>
        {
            var currItem = _dataItems[_currAngle];
            _panelCollect.ShowItemCollect(currItem.spriteDone, currItem.value);
            TweenControl.GetInstance().ScaleFromZero(_panelCollect.gameObject, 0.3f, () => { callback?.Invoke(); });
        }, EaseType.OutQuad);
    }

    private float Angle()
    {
        var angle = _angleStart + 360f * 5f;
        return angle;
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
    CURRENCY_BALANCE
}
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using static CUtils;

public class CurrencyBallance : MonoBehaviour
{
    [SerializeField] private GameObject _iconStar;
    [SerializeField] private ParticleSystem _fxLight;

    private void Start()
    {
        UpdateBalance();
        CurrencyController.onBalanceChanged += OnBalanceChanged;
    }

    private void UpdateBalance()
    {
        var currency = AbbrevationUtility.AbbreviateNumber(CurrencyController.GetBalance());
        gameObject.SetText(currency);
    }

    private void OnBalanceChanged()
    {
        UpdateBalance();
        if (_fxLight != null)
            _fxLight.Play();
        if (_iconStar != null)
        {
            TweenControl.GetInstance().Scale(_iconStar, Vector3.one * 1.2f, 0.3f, () =>
            {
                TweenControl.GetInstance().Scale(_iconStar, Vector3.one, 0.3f);
            });
        }
        TweenControl.GetInstance().Scale(gameObject, Vector3.one * 1.2f, 0.3f, () =>
        {
            TweenControl.GetInstance().Scale(gameObject, Vector3.one, 0.3f);
        });
    }

    private void OnDestroy()
    {
        CurrencyController.onBalanceChanged -= OnBalanceChanged;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingController : MonoBehaviour
{
    [SerializeField] private Image _iconPlayer;
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private TextMeshProUGUI _playerValue;

    public void UpdateRankingPlayer(string name, int value, Sprite sprite = null)
    {
        if (sprite != null)
        {
            _iconPlayer.sprite = sprite;
            _iconPlayer.SetNativeSize();
        }
        _playerName.text = name;
        _playerValue.text = value.ToString();
    }
}

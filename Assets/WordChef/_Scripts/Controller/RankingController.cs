using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingController : MonoBehaviour
{
    [SerializeField] private Image _iconPlayer;
    [SerializeField] private Image _avatarPlayer;
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private TextMeshProUGUI _playerValue;

    public void UpdateRankingPlayer(string name, int value, string urlAvatar, Sprite sprite = null)
    {
        if (sprite != null)
        {
            _iconPlayer.sprite = sprite;
            _iconPlayer.SetNativeSize();
        }

        _playerName.text = name;
        _playerValue.text = value.ToString();

        StartCoroutine(ShowAvatar(urlAvatar));
    }

    private IEnumerator ShowAvatar(string urlAvatar)
    {
        WWW www = new WWW(urlAvatar);
        yield return www;
        _avatarPlayer.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
    }
}

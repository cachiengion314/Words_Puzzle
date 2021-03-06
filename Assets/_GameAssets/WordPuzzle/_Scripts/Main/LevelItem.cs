﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
    public GameData gameData;
    [Space]
    public Text levelText;
    public int world, subWorld, level, numlevels;
    //public Transform centerPoint;
    public Image background;
    public Color colorTextLock;
    public Color colorTextUnlock;

    public Sprite solvedSprite, currentSprite, lockedSprite;
    public GameObject solvedBtn, currentBtn, lockedBtn;

    //private List<Vector3> letterLocalPositions = new List<Vector3>();
    //private List<Text> letterTexts = new List<Text>();
    private GameLevel gameLevel;
    private const float RADIUS = 40f;

    public void Start()
    {
        //levelText.text = "Level " + (!(world == 0 && subWorld == 0) ? (level + 1) + GetNumberLevel() : (level + 1));
        levelText.text = "Level " + ((level + numlevels * subWorld + world * gameData.words[0].subWords.Count * numlevels) + 1);
        GetComponent<Button>().onClick.AddListener(OnButtonClick);

        //gameLevel = Resources.Load<GameLevel>("World_" + world + "/SubWorld_" + subWorld + "/Level_" + level);
        gameLevel = gameData.words[world].subWords[subWorld].gameLevels[level];
        //if (gameLevel != null)
        //{
        //    Load();
        //}

        int unlockedWorld = Prefs.unlockedWorld;
        int unlockedSubWorld = Prefs.unlockedSubWorld;
        int unlockedLevel = Prefs.unlockedLevel;

        if (world < unlockedWorld ||
            (world == unlockedWorld && subWorld < unlockedSubWorld) ||
            (world == unlockedWorld && subWorld <= unlockedSubWorld && level < unlockedLevel) || Prefs.IsLevelEnd)
        {
            background.sprite = solvedSprite;
            solvedBtn.SetActive(true);
            currentBtn.SetActive(false);
            lockedBtn.SetActive(false);
            levelText.color = colorTextUnlock;
        }
        else if (world == unlockedWorld && subWorld == unlockedSubWorld && level == unlockedLevel)
        {
            background.sprite = currentSprite;
            solvedBtn.SetActive(false);
            currentBtn.SetActive(true);
            lockedBtn.SetActive(false);
            levelText.color = colorTextUnlock;
        }
        else
        {
            background.sprite = lockedSprite;
            GetComponent<Button>().interactable = false;
            solvedBtn.SetActive(false);
            currentBtn.SetActive(false);
            lockedBtn.SetActive(true);
            levelText.color = colorTextLock;
        }
    }

    private int GetNumberLevel()
    {
        var levels = 0;
        for (int i = 0; i <= world; i++)
        {
            int wordId = i;
            for (int j = 0; j <= subWorld; j++)
            {
                int subWordId = j;
                if (!(wordId == 0 && subWordId == 0))
                    levels += Superpow.Utils.GetNumLevels(wordId, subWordId);
            }
        }
        return levels;
    }

    //public void Load()
    //{
    //    int numLetters = gameLevel.word.Trim().Length;
    //    if (numLetters == 0) return;

    //    float delta = 360f / numLetters;

    //    float angle = 90;
    //    for (int i = 0; i < numLetters; i++)
    //    {
    //        float angleRadian = angle * Mathf.PI / 180f;
    //        float x = Mathf.Cos(angleRadian);
    //        float y = Mathf.Sin(angleRadian);
    //        Vector3 position = RADIUS * new Vector3(x, y, 0);

    //        letterLocalPositions.Add(position);

    //        angle += delta;
    //    }

    //    for (int i = 0; i < numLetters; i++)
    //    {
    //        Text letter = Instantiate(MonoUtils.instance.letter);
    //        letter.transform.SetParent(centerPoint);
    //        letter.transform.localScale = Vector3.one;
    //        letter.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-15, 15)));
    //        letter.text = gameLevel.word[i].ToString().ToUpper();
    //        letter.fontSize = ConfigController.Config.fontSizeInDiskSelectLevel;
    //        letterTexts.Add(letter);
    //    }

    //    List<int> indexes = Prefs.GetPanWordIndexes(world, subWorld, level).ToList();
    //    if (indexes.Count != numLetters)
    //    {
    //        indexes = Enumerable.Range(0, numLetters).ToList();
    //        indexes.Shuffle(level);
    //        Prefs.SetPanWordIndexes(world, subWorld, level, indexes.ToArray());
    //    }

    //    for (int i = 0; i < numLetters; i++)
    //    {
    //        letterTexts[i].transform.localPosition = letterLocalPositions[indexes.IndexOf(i)];
    //    }
    //}

    public void OnButtonClick()
    {
        GameState.currentWorld = world;
        GameState.currentSubWorld = subWorld;
        GameState.currentLevel = level;

        CUtils.LoadScene(Const.SCENE_MAIN, false);
        Sound.instance.Play(Sound.Others.PopupOpen);

        // Set the music
        if (!Music.instance.audioSource.isPlaying)
            Music.instance.Play(CUtils.GetRandom(Music.Type.Main_0, Music.Type.Main_1, Music.Type.Main_2));
    }
}

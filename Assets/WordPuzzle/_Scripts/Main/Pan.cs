using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Text;
using System;

public class Pan : MonoBehaviour
{
    private int numLetters;
    private string word, panWord;
    private GameLevel gameLevel;
    private const float RADIUS = 250;
    private List<Vector3> letterPositions = new List<Vector3>();
    private List<Vector3> letterLocalPositions = new List<Vector3>();
    private List<Text> letterTexts = new List<Text>();
    private List<int> indexes = new List<int>();

    private int world, subWorld, level;
    private int soundIndex = 0;

    public Transform centerPoint;
    public TextPreview textPreview;

    public static Pan instance;

    public List<Text> LetterTexts
    {
        get
        {
            return letterTexts;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        world = GameState.currentWorld;
        subWorld = GameState.currentSubWorld;
        level = GameState.currentLevel;
    }

    public void Load(GameLevel gameLevel)
    {
        this.gameLevel = gameLevel;
        numLetters = gameLevel.word.Trim().Length;

        if (numLetters <= 3) transform.localPosition += new Vector3(0f, 40f, 0f);

        float delta = 360f / numLetters;

        float angle = 150;
        for (int i = 0; i < numLetters; i++)
        {
            float angleRadian = angle * Mathf.PI / 180f;
            float x = Mathf.Cos(angleRadian);
            float y = Mathf.Sin(angleRadian);
            Vector3 position = RADIUS * new Vector3(x, y, 0);

            letterLocalPositions.Add(position);
            letterPositions.Add(centerPoint.TransformPoint(position));

            //Debug.Log(centerPoint.position);

            angle += delta;
        }

        LineDrawer.instance.letterPositions = letterPositions;

        for (int i = 0; i < numLetters; i++)
        {
            Text letter = Instantiate(MonoUtils.instance.letter, centerPoint);
            //letter.transform.SetParent(centerPoint);
            letter.transform.localScale = Vector3.one;
            letter.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, UnityEngine.Random.Range(-10, 10)));
            letter.text = gameLevel.word[i].ToString().ToUpper();
            if (!ThemesControl.instance.CurrTheme.fontData.fontScale)
                letter.fontSize = ThemesControl.instance.CurrTheme.fontData.fontSizeMax;
            else
                letter.fontSize = ConfigController.Config.fontSizeInDiskMainScene;
            letterTexts.Add(letter);
        }

        indexes = Prefs.GetPanWordIndexes(world, subWorld, level).ToList();

        //foreach(int i in indexes)
        //{
        //    Debug.Log("index: " + i);
        //}

        if (indexes.Count != numLetters)
        {
            indexes = Enumerable.Range(0, numLetters).ToList();
            indexes.Shuffle(level);
            Prefs.SetPanWordIndexes(world, subWorld, level, indexes.ToArray());
        }

        GetPanWord();

        Timer.Schedule(this, 0, () =>
        {
            for (int i = 0; i < numLetters; i++)
            {
                letterTexts[i].transform.localPosition = letterLocalPositions[indexes.IndexOf(i)];
                letterTexts[i].transform.localScale = Vector3.zero;
            }
        });
    }

    private void GetShuffeWord()
    {
        List<int> origin = new List<int>();
        origin.AddRange(indexes);
        while (true)
        {
            indexes.Shuffle();
            if (!origin.SequenceEqual(indexes)) break;
        }
        GetPanWord();
    }

    private void GetPanWord()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < numLetters; i++)
        {
            sb.Append(gameLevel.word[indexes[i]]);
        }
        panWord = sb.ToString();
        textPreview.word = panWord.ToUpper();
    }

    public void Shuffle()
    {
        TutorialController.instance.HidenPopTut();
        var tweenControl = TweenControl.GetInstance();
        GetShuffeWord();
        Prefs.SetPanWordIndexes(world, subWorld, level, indexes.ToArray());

        int i = 0;
        BlockScreen.instance.Block(true);
        tweenControl.LocalRotate(centerPoint, new Vector3(0, 0, 360), 0.3f/*);tweenControl.ScaleFromOne(centerPoint.gameObject, 0.3f*/, () =>
        {
            tweenControl.DelayCall(transform, 0.15f, () =>
            {
                tweenControl.LocalRotate(centerPoint, new Vector3(0, 0, centerPoint.localRotation.z + 360), 0.2f/*);tweenControl.ScaleFromZero(centerPoint.gameObject, 0.3f*/, () =>
                {
                    BlockScreen.instance.Block(false);
                });
            });
        }, EaseType.InQuad);
        foreach (var text in letterTexts)
        {
            text.transform.localPosition = letterLocalPositions[indexes.IndexOf(i)];
            var oldPos = text.transform.localPosition;
            //var distance = Vector3.Distance(text.transform.localPosition, Vector3.zero);
            //var velocityArg = RADIUS / 0.15f;
            var timeMove = /*distance / velocityArg*/0.2f;
            if (i != letterTexts.Count - 1)
                tweenControl.FadeAnfaText(text, 0, timeMove);
            //iTween.MoveTo(text.gameObject, iTween.Hash("position", letterLocalPositions[indexes.IndexOf(i)], "time", 0.15f, "isLocal", true));
            tweenControl.LocalRotate(text.transform, new Vector3(0, 0, 360 * 2 + 360 / letterTexts.Count * i), 0.3f, () =>
            {
                tweenControl.LocalRotate(text.transform, new Vector3(0, 0, (text.transform.localPosition.z + 360f * 2)), timeMove, () =>
                {
                    text.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, UnityEngine.Random.Range(-10, 10)));
                });
            });
            tweenControl.MoveLocal(text.transform, /*oldPos / 3.5f*/Vector3.zero, 0.3f, () =>
                 {
                     tweenControl.PauseTweener(text.transform);
                     tweenControl.DelayCall(transform, 0.15f, () =>
                     {
                         if (i != letterTexts.Count - 1)
                             tweenControl.FadeAnfaText(text, 1, timeMove);
                         tweenControl.PlayTweener(text.transform);
                         tweenControl.MoveLocal(text.transform, oldPos, timeMove);
                     });
                 });

            //text.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, UnityEngine.Random.Range(-10, 10)));
            i++;
        }
        Sound.instance.PlayButton(Sound.Button.Shuffe);
        WordRegion.instance.UserItemCallEventFirebase("item_shuffle");
    }

    public void ScaleWord(Vector3 letterPos, Action callback = null)
    {
        BlockScreen.instance.Block(false);
        TweenControl.GetInstance().KillTweener(textPreview.transform);
        textPreview.transform.localPosition = new Vector3(0, textPreview.transform.localPosition.y, 0);
        var letterTarget = letterTexts.Single(let => Vector3.Distance(letterPos, let.transform.position) < 1);
        if (letterTarget.transform.localScale == ((LetterTexts.Count < 4 && ThemesControl.instance.CurrTheme.fontData.fontScale) ? Vector3.one * 1.2f : Vector3.one))
        {
            Sound.instance.Play(Sound.instance.lettersTouch[soundIndex]);
            soundIndex++;
            if (soundIndex > Sound.instance.lettersTouch.Length - 1) { soundIndex = Sound.instance.lettersTouch.Length - 1; }
            TweenControl.GetInstance().Scale(letterTarget.gameObject, (LetterTexts.Count < 4 && ThemesControl.instance.CurrTheme.fontData.fontScale) ? Vector3.one * 1.3f : Vector3.one * 1.2f, 0.3f, () =>
            {
                callback?.Invoke();
            });
        }
    }

    public void ResetScaleWord(Action callback = null)
    {
        soundIndex = 0;
        foreach (var word in letterTexts)
        {
            TweenControl.GetInstance().Scale(word.gameObject, (LetterTexts.Count < 4 && ThemesControl.instance.CurrTheme.fontData.fontScale) ? Vector3.one * 1.2f : Vector3.one, 0.3f, () =>
            {
                callback?.Invoke();
            });
        }
    }
}

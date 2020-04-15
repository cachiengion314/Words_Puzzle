using UnityEngine;
using System.Collections;
using Spine;
using System;

public class Compliment : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer sRenderer;
    public SpriteRenderer sRendererBG;
    public Transform rootParticle;
    public Sprite[] sprites;
    public Sprite[] spritesBg;
    public ParticleSystem[] particleSystems;
    public ParticleSystem fxLevelClear;
    public ParticleSystem fxHidenLetter;
    [Space]
    [SerializeField] private bool _useSpine;
    [SerializeField] private SpineControl _animCompliment;
    [SerializeField] private string[] nameAnim;

    int idAnim;
    ParticleSystem _particle;

    private void Awake()
    {
        if (_animCompliment != null)
            _animCompliment.onEventAction = OnShowEffect;
    }

    public void PlayParticle()
    {
        if (_particle != null)
        {
            if (_particle.GetComponent<Canvas>() != null)
                _particle.GetComponent<Canvas>().overrideSorting = true;
            Debug.Log("overrideSorting: " + _particle.GetComponent<Canvas>().overrideSorting);
            _particle.gameObject.SetActive(true);
            _particle.Play();
            Debug.Log("IsRunning: " + _particle.isPlaying);
        }
    }

    public void Hidenarticle()
    {
        if (_particle != null)
        {
            _particle.gameObject.SetActive(false);
        }
    }

    public void Show(int type)
    {
        if (_particle != null)
            Destroy(_particle.gameObject);
        _particle = Instantiate(particleSystems[type], rootParticle);
        _particle.gameObject.SetActive(false);
        Debug.Log("_particle: " + _particle.name);
        if (_useSpine)
        {
            _animCompliment.gameObject.SetActive(true);
            _animCompliment.SetAnimation(nameAnim[type], false);
        }
        else
        {
            if (!IsAvailable2Show()) return;
            sRenderer.sprite = sprites[type];
            sRendererBG.sprite = spritesBg[type];
            _particle.gameObject.SetActive(false);
        }
        switch (type)
        {
            case 0:
                Prefs.countGood += 1;
                Prefs.countGoodDaily += 1;
                break;
            case 1:
                Prefs.countGreat += 1;
                Prefs.countGreatDaily += 1;
                break;
            case 2:
                Prefs.countAmazing += 1;
                Prefs.countAmazingDaily += 1;
                break;
            case 3:
                Prefs.countAwesome += 1;
                Prefs.countAwesomeDaily += 1;
                break;
            case 4:
                Prefs.countExcellent += 1;
                Prefs.countExcellentDaily += 1;
                break;
        }
        if (!_useSpine)
            anim.SetTrigger("show");
    }

    private void OnShowEffect(Spine.Event e)
    {
        if (e.Data.Name == "EF")
        {
            Debug.Log("Play Event Spine GGAAE");
            PlayParticle();
        }
        else
        {
            Debug.LogError("Name Event null !");
        }
    }

    public void ShowRandom()
    {
        if (!IsAvailable2Show()) return;

        sRenderer.sprite = CUtils.GetRandom(sprites);
        anim.SetTrigger("show");
    }

    private bool IsAvailable2Show()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        return info.IsName("Idle");
    }
}

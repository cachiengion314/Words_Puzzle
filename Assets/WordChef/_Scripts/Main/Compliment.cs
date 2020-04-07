using UnityEngine;
using System.Collections;

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

    ParticleSystem _particle;

    public void PlayParticle()
    {
        if (_particle != null)
        {
            _particle.gameObject.SetActive(true);
            _particle.Play();
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
        if (!IsAvailable2Show()) return;

        sRenderer.sprite = sprites[type];
        sRendererBG.sprite = spritesBg[type];
        if (_particle != null)
            Destroy(_particle.gameObject);
        _particle = Instantiate(particleSystems[type], rootParticle);
        _particle.gameObject.SetActive(false);
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
        anim.SetTrigger("show");
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

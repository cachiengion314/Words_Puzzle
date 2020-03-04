using UnityEngine;
using System.Collections;

public class Compliment : MonoBehaviour {
    public Animator anim;
    public SpriteRenderer sRenderer;
    public Sprite[] sprites;

    public void Show(int type)
    {
        if (!IsAvailable2Show()) return;

        sRenderer.sprite = sprites[type];
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

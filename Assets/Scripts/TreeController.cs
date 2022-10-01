using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TreeController : Singleton<TreeController>
{
    public List<Sprite> StateSprites = new List<Sprite>();
    public ParticleSystem TransitionParticleSystem;

    int currentSpriteIndex = 0;
    SpriteRenderer TreeSprite;

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        TreeSprite = GetComponent<SpriteRenderer>();
        SetSprite(0, true);
    }



    void SetSprite(int index, bool silent = false)
    {
        if (!silent)
        {
            TransitionParticleSystem.Play();
            DOVirtual.DelayedCall(1f, () => TransitionParticleSystem.Stop());
        }
        DOVirtual.DelayedCall(0.5f, () => TreeSprite.sprite = StateSprites[index]);
        currentSpriteIndex = index;
    }

    public void Upgrade()
    {
        if (currentSpriteIndex + 1 > StateSprites.Count) return;
        SetSprite(currentSpriteIndex + 1);
    }


}

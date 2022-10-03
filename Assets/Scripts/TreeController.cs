using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TreeController : Singleton<TreeController>
{
    public List<Sprite> StateSprites = new List<Sprite>();
    public ParticleSystem TransitionParticleSystem;
    public ParticleSystem ShimmerParticleSystem;
    public Animator CircleAnimation;


    public ParticleSystem UIParticleSystem_1;
    public ParticleSystem UIParticleSystem_2;

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
        ShimmerParticleSystem.Stop();
        index_1 = 0;
        index_2 = 1;
    }


    [ContextMenu("set 6")]
    public void TestUpgrade()
    {
        SetSprite(6);
    }


    void SetSprite(int index, bool silent = false)
    {
        if (!silent)
        {
            TransitionParticleSystem.Play();
            UIController.Instance.ProgressSlider.DOValue(UIController.Instance.ProgressSlider.value + 1, 1f);
            DOVirtual.DelayedCall(1f, () => TransitionParticleSystem.Stop());
        }
        DOVirtual.DelayedCall(0.5f, () => TreeSprite.sprite = StateSprites[index]);
        currentSpriteIndex = index;
        if (currentSpriteIndex == 6) ShimmerParticleSystem.Play();
        Debug.Log($"{currentSpriteIndex} {ShimmerParticleSystem.isPlaying}");
    }

    public void Upgrade()
    {
        if (currentSpriteIndex + 1 > StateSprites.Count) return;
        SetSprite(currentSpriteIndex + 1);
        SetUpgradeParticleMaterials(currentSpriteIndex);
        UIParticleSystem_1.Play();
        UIParticleSystem_2.Play();
        DOVirtual.DelayedCall(3f, () =>
        {
            UIController.Instance.bubbles[index_1].DOColor(Color.white, 0.5f);
            UIController.Instance.bubbles[index_2].DOColor(Color.white, 0.5f);
        });
    }

    public int GetUpgradeLevel()
    {
        return currentSpriteIndex;
    }
    int index_1;
    int index_2;

    void SetUpgradeParticleMaterials(int level)
    {
        switch (level)
        {
            case 1:
                index_1 = 0;
                index_2 = 1;

                UIParticleSystem_1.GetComponent<ParticleSystemRenderer>().material.SetTexture("_MainTex", UIController.Instance.bubbleSprites[0].texture);
                UIParticleSystem_2.GetComponent<ParticleSystemRenderer>().material.SetTexture("_MainTex", UIController.Instance.bubbleSprites[1].texture);
                break;
            case 2:
                index_1 = 2;
                index_2 = 3;
                UIParticleSystem_1.GetComponent<ParticleSystemRenderer>().material.SetTexture("_MainTex", UIController.Instance.bubbleSprites[2].texture);
                UIParticleSystem_2.GetComponent<ParticleSystemRenderer>().material.SetTexture("_MainTex", UIController.Instance.bubbleSprites[3].texture);
                break;
            case 3:
                index_1 = 4;
                index_2 = 5;
                UIParticleSystem_1.GetComponent<ParticleSystemRenderer>().material.SetTexture("_MainTex", UIController.Instance.bubbleSprites[4].texture);
                UIParticleSystem_2.GetComponent<ParticleSystemRenderer>().material.SetTexture("_MainTex", UIController.Instance.bubbleSprites[5].texture);
                break;
            case 4:
                index_1 = 6;
                index_2 = 7;
                UIParticleSystem_1.GetComponent<ParticleSystemRenderer>().material.SetTexture("_MainTex", UIController.Instance.bubbleSprites[6].texture);
                UIParticleSystem_2.GetComponent<ParticleSystemRenderer>().material.SetTexture("_MainTex", UIController.Instance.bubbleSprites[7].texture);
                break;
            case 5:
                index_1 = 8;
                index_2 = 9;
                UIParticleSystem_1.GetComponent<ParticleSystemRenderer>().material.SetTexture("_MainTex", UIController.Instance.bubbleSprites[8].texture);
                UIParticleSystem_2.GetComponent<ParticleSystemRenderer>().material.SetTexture("_MainTex", UIController.Instance.bubbleSprites[9].texture);
                break;
            case 6:
                index_1 = 10;
                index_2 = 11;
                UIParticleSystem_1.GetComponent<ParticleSystemRenderer>().material.SetTexture("_MainTex", UIController.Instance.bubbleSprites[10].texture);
                UIParticleSystem_2.GetComponent<ParticleSystemRenderer>().material.SetTexture("_MainTex", UIController.Instance.bubbleSprites[11].texture);
                break;
            case 7:
                index_1 = 12;
                index_2 = 13;
                UIParticleSystem_1.GetComponent<ParticleSystemRenderer>().material.SetTexture("_MainTex", UIController.Instance.bubbleSprites[12].texture);
                UIParticleSystem_2.GetComponent<ParticleSystemRenderer>().material.SetTexture("_MainTex", UIController.Instance.bubbleSprites[13].texture);
                break;
            case 8:
                index_1 = 14;
                index_2 = 15;
                UIParticleSystem_1.GetComponent<ParticleSystemRenderer>().material.SetTexture("_MainTex", UIController.Instance.bubbleSprites[14].texture);
                UIParticleSystem_2.GetComponent<ParticleSystemRenderer>().material.SetTexture("_MainTex", UIController.Instance.bubbleSprites[15].texture);
                break;
            default:
                break;
        }
    }



}

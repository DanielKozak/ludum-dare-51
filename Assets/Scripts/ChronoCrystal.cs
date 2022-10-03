using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChronoCrystal : MonoBehaviour
{
    SpriteRenderer mRenderer;

    public ParticleSystem particle;

    float minDisplacement = -2f;
    float maxDisplacement = 2f;
    public bool isRed = false;

    public TrailRenderer Trail;

    void Start()
    {
        mRenderer = GetComponent<SpriteRenderer>();
        SetSprite();
        // Vector3 diplacement = transform.position + new Vector3(Random.Range(minDisplacement, maxDisplacement), Random.Range(minDisplacement, maxDisplacement), 0);
        // transform.DOJump(diplacement, 3, 1, Random.Range(0.5f, 3f));
    }


    void SetSprite()
    {
        Sprite[] sprites;

        if (!isRed) sprites = Resources.LoadAll<Sprite>("Textures/chrystal_chrono");
        else sprites = Resources.LoadAll<Sprite>("Textures/chrystal_matter");
        mRenderer.sprite = sprites[UnityEngine.Random.Range(0, sprites.Length)];
    }

    public void Hit()
    {
        Debug.Log("hit crystal");
        particle.Play();
        mRenderer.enabled = false;
        if (isRed)
        {
            // UIController.Instance.TweenRedCrystal(transform.position);
            GameConroller.Instance.AddRedCrystal();
        }
        else
        {
            // UIController.Instance.TweenBlueCrystal(transform.position);
            GameConroller.Instance.AddBlueCrystal();
            TimeFieldController.Instance.RemoveValue(transform.position, 0.2f);
        }
        Destroy(gameObject, 1f);
    }
    public void Consume()
    {
        particle.Play();
        mRenderer.enabled = false;
        Destroy(gameObject, 0.7f);
        TimeFieldController.Instance.RemoveValue(transform.position, 0.2f);
    }
}

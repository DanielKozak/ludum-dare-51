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

    void Start()
    {
        mRenderer = GetComponent<SpriteRenderer>();
        SetSprite();
        Vector3 diplacement = transform.position + new Vector3(Random.Range(minDisplacement, maxDisplacement), Random.Range(minDisplacement, maxDisplacement), 0);
        transform.DOJump(diplacement, 3, 1, Random.Range(0.5f, 3f));
    }


    void SetSprite()
    {
        var sprites = Resources.LoadAll<Sprite>("Textures/chrystal_chrono");
        mRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }

    public void Hit()
    {
        particle.Play();
        mRenderer.enabled = false;
        Destroy(gameObject, 0.7f);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FMODUnity;

public class GameConroller : Singleton<GameConroller>
{
    // Start is called before the first frame update
    void Start()
    {
        mCam = Camera.main;
    }

    Camera mCam;
    [NonSerialized] public int BlueCount = 40;
    [NonSerialized] public int RedCount = 10;
    [NonSerialized] public int Seconds = 50;

    public List<GameObject> Rifts;
    public List<GameObject> Forges;
    public GameObject ForgeContainer;
    public GameObject ForgePrefab;




    public GameObject RedCrystalPrefab;
    public GameObject BlueCrystalPrefab;

    public Transform RedCrystalContainer;
    public Transform BlueCrystalContainer;


    public void StartNewGame(bool tutorial)
    {
        ResetState();
    }

    void ResetState()
    {
        BlueCount = 40;
        RedCount = 8;
        Seconds = 0;

        TimeFieldController.Instance.ResetState();
        UIController.Instance.ResetState();
        WormSpawner.Instance.ResetState();
        GodController.Instance.ResetState();
        TimeController.Instance.ResetState();
        TreeController.Instance.ResetState();
        foreach (var item in Rifts)
        {
            item.GetComponent<BoxCollider2D>().enabled = true;
        }
        Forges = new List<GameObject>();
        while (ForgeContainer.transform.childCount > 0)
        {
            Destroy(ForgeContainer.transform.GetChild(0));
        }
        while (RedCrystalContainer.transform.childCount > 0)
        {
            Destroy(ForgeContainer.transform.GetChild(0));
        }
        while (BlueCrystalContainer.transform.childCount > 0)
        {
            Destroy(ForgeContainer.transform.GetChild(0));
        }
        // Debug.Log("Game Start Spawn");
        SpawnInitialMatter(15, 15, 49);

    }
    [ContextMenu("red")]
    public void AddRedCrystal()
    {
        RuntimeManager.PlayOneShot("event:/SFX/crystal_hit");
        RedCount += 1;
        UIController.Instance.RedIndicatorLabel.text = RedCount.ToString();
        UIController.Instance.RedIndicatorLabel.transform.parent.DOScale(new Vector3(1.2f, 1.2f, 1), 0.8f)
            .OnComplete(() => UIController.Instance.RedIndicatorLabel.transform.parent.DOScale(Vector3.one, 0.8f));
    }
    [ContextMenu("blue")]

    public void AddBlueCrystal()
    {
        BlueCount += 1;
        UIController.Instance.BlueIndicatorLabel.text = BlueCount.ToString();
        UIController.Instance.BlueIndicatorLabel.transform.parent.DOScale(new Vector3(1.2f, 1.2f, 1), 0.8f)
            .OnComplete(() => UIController.Instance.BlueIndicatorLabel.transform.parent.DOScale(Vector3.one, 0.8f));
    }
    public void RemoveRedCrystal()
    {
        RedCount -= 1;
        UIController.Instance.RedIndicatorLabel.text = RedCount.ToString();
    }
    public void RemoveBlueCrystal()
    {
        BlueCount -= 1;
        UIController.Instance.RedIndicatorLabel.text = BlueCount.ToString();
    }
    [ContextMenu("sec")]
    public void AddSecond()
    {
        Seconds += 1;
        UIController.Instance.SecondsIndicatorLabel.text = Seconds.ToString();
        UIController.Instance.SecondsIndicatorLabel.transform.parent.DOScale(new Vector3(1.2f, 1.2f, 1), 0.8f)
            .OnComplete(() => UIController.Instance.SecondsIndicatorLabel.transform.parent.DOScale(Vector3.one, 0.8f));

        if (Seconds >= 10)
        {
            TreeController.instance.CircleAnimation.SetBool("pulse", true);
        }
        if (Seconds <= 10)
        {
            TreeController.instance.CircleAnimation.SetBool("pulse", false);
        }
    }

    public void RemoveSeconds()
    {
        Seconds -= 1;
        UIController.Instance.SecondsIndicatorLabel.text = Seconds.ToString();
    }

    public void SpawnInitialMatter(int countRed, int countBlue, int radiusAllowed)
    {
        for (int i = 0; i < countRed; i++)
        {
            Vector2 randCoords = UnityEngine.Random.insideUnitCircle * radiusAllowed;
            var go = Instantiate(RedCrystalPrefab);
            go.transform.SetParent(RedCrystalContainer);
            go.transform.position = randCoords;
        }
        for (int i = 0; i < countBlue; i++)
        {
            Vector2 randCoords = UnityEngine.Random.insideUnitCircle * radiusAllowed;
            var go = Instantiate(BlueCrystalPrefab);
            go.transform.SetParent(BlueCrystalContainer);
            go.transform.position = randCoords;
            TimeFieldController.Instance.AddValue(randCoords, 0.2f);

        }
    }


    public ParticleSystem MeteorIndicator;

    public void SpawnMeteor(int countLoot, int radiusAllowed, int radiusSpread)
    {
        Vector2 spawnCoords = UnityEngine.Random.insideUnitCircle * radiusAllowed;
        // Debug.Log($"SpawnMeteor, c= {countLoot} r={radiusAllowed} {spawnCoords}");
        for (int i = 0; i < countLoot; i++)
        {
            Vector2 randCoords = spawnCoords + UnityEngine.Random.insideUnitCircle * radiusSpread;
            var go = Instantiate(BlueCrystalPrefab);
            go.GetComponent<ChronoCrystal>().Trail.gameObject.SetActive(true);
            go.transform.SetParent(BlueCrystalContainer);
            go.transform.position = new Vector3(randCoords.x - 20, randCoords.y + 100);

            go.transform.DOMove(randCoords, 3f).SetEase(Ease.Linear).OnComplete(() =>
            {
                go.GetComponent<ChronoCrystal>().Trail.gameObject.SetActive(false);
                TimeFieldController.Instance.AddValue(randCoords, 0.2f);
                go.transform.DOJump(randCoords, 2f, 2, 1f);
                mCam.DOShakePosition(0.2f, 4f);
            });
        }

        var screenSpaceMeteorCoords = mCam.WorldToViewportPoint(spawnCoords);
        if (screenSpaceMeteorCoords.x < 0f || screenSpaceMeteorCoords.x > 1f || screenSpaceMeteorCoords.y < 0f || screenSpaceMeteorCoords.y > 0f)
        {
            // Debug.Log($"MeteorIndicator Up");
            if (MeteorIndicator != null)
            {
                MeteorIndicator.transform.position = spawnCoords;
                MeteorIndicator.transform.LookAt(GodController.Instance.transform.position, Vector3.right);
                DOVirtual.DelayedCall(3f, () => MeteorIndicator.Play());
            }
        }
    }

    public void SpawnRedCrystal(Vector3 position)
    {
        Vector3 randCoords = (Vector3)UnityEngine.Random.insideUnitCircle + position;
        var go = Instantiate(RedCrystalPrefab);
        go.transform.SetParent(RedCrystalContainer);
        go.transform.position = randCoords;
    }


    public void PlaceForge(Vector3 position)
    {
        var forge = Instantiate(ForgePrefab, position, Quaternion.identity);
        forge.transform.SetParent(ForgeContainer.transform);
        Forges.Add(forge);
    }
}

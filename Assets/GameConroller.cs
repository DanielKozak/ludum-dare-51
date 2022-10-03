using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameConroller : Singleton<GameConroller>
{
    // Start is called before the first frame update
    void Start()
    {

    }

    [NonSerialized] public int BlueCount = 40;
    [NonSerialized] public int RedCount = 10;
    [NonSerialized] public int Seconds = 50;

    public List<GameObject> Rifts;
    public List<GameObject> Forges;
    public GameObject ForgeContainer;

    public void StartNewGame()
    {
        ResetState();
    }

    void ResetState()
    {
        BlueCount = 40;
        RedCount = 10;
        Seconds = 0;

        TimeFieldController.Instance.ResetState();
        WormSpawner.Instance.ResetState();
        GodController.Instance.ResetState();
        TimeController.Instance.ResetState();
        TreeController.Instance.ResetState();
        UIController.Instance.ResetState();
        foreach (var item in Rifts)
        {
            item.GetComponent<BoxCollider2D>().enabled = true;
        }
        Forges = new List<GameObject>();
        while (ForgeContainer.transform.childCount > 0)
        {
            Destroy(ForgeContainer.transform.GetChild(0));
        }

    }
    [ContextMenu("red")]
    public void AddRedCrystal()
    {
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
}

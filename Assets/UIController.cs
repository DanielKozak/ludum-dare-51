using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class UIController : Singleton<UIController>
{
    public TMP_Text MessageLabel;
    bool isLabelShown;

    public Slider ProgressSlider;

    public List<Image> bubbles;

    public Image BlueIndicator;
    public Image RedIndicator;
    public Image SecondsIndicator;

    public TMP_Text BlueIndicatorLabel;
    public TMP_Text RedIndicatorLabel;
    public TMP_Text SecondsIndicatorLabel;

    public TMP_Text GodSupportLabel;

    public List<Sprite> bubbleSprites;

    public GameObject BlueIconPrefab;
    public GameObject RedIconPrefab;
    public GameObject SecondsIconPrefab;

    Camera mCam;

    private void Start()
    {
        mCam = Camera.main;
        ResetState();
    }

    public void ResetState()
    {
        ProgressSlider.value = 0;
        BlueIndicatorLabel.text = GameConroller.Instance.BlueCount.ToString();
        RedIndicatorLabel.text = GameConroller.Instance.RedCount.ToString();
        SecondsIndicatorLabel.text = GameConroller.Instance.Seconds.ToString();

        foreach (var item in bubbles)
        {
            item.color = Color.clear;
        }
    }

    public void TweenBubble(int index)
    {
        if (index == 15)
        {
            bubbles[index].DOColor(Color.white, 0.5f);
            //TODO END GAME

        }
        else
            bubbles[index].DOColor(Color.white, 0.5f);
    }

    public void TweenBlueCrystal(Vector3 worldPos)
    {
        var go = Instantiate(BlueIconPrefab);
        go.transform.SetParent(transform);
        go.transform.position = mCam.WorldToScreenPoint(worldPos);
        var rt = go.transform as RectTransform;
        rt.DOAnchorPos(BlueIndicator.transform.position, 2f).SetEase(Ease.InQuad);
        Destroy(go, 2f);
    }
    public void TweenRedCrystal(Vector3 worldPos)
    {
        var go = Instantiate(RedIconPrefab);
        go.transform.SetParent(transform);

        go.transform.position = mCam.WorldToScreenPoint(worldPos);
        go.transform.DOMove(RedIndicator.transform.position, 2f).SetEase(Ease.InQuad);
        Destroy(go, 2f);
    }
    public void TweenSecondsCrystal(Vector3 worldPos)
    {
        var go = Instantiate(SecondsIconPrefab);
        go.transform.SetParent(transform);

        go.transform.position = mCam.WorldToScreenPoint(worldPos);
        go.transform.DOMove(SecondsIndicator.transform.position, 2f).SetEase(Ease.InQuad);
        Destroy(go, 2f);
    }



    public void ShowMessageLabel(string label, float duration = 3f)
    {
        MessageLabel.transform.position = mCam.ViewportToScreenPoint(new Vector3(0.5f, 1.2f, 0f));
        MessageLabel.color = Color.white;
        float newY = mCam.ViewportToScreenPoint(new Vector3(0.5f, 0.9f, 0f)).y;
        MessageLabel.text = label;
        MessageLabel.transform.DOMoveY(newY, 0.5f);
        DOVirtual.DelayedCall(duration, () =>
        {
            MessageLabel.DOColor(Color.clear, 1f);
        });
    }

}

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
            bubbles[index].DOColor(Color.white, 2f);
            //TODO END GAME

        }
        else
            bubbles[index].DOColor(Color.white, 2f);
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

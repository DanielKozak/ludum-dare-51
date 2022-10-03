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

    Camera mCam;

    private void Start()
    {
        mCam = Camera.main;
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

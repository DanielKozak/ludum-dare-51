using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(Canvas))]
public class ToolTipController : Singleton<ToolTipController>
{
    public RectTransform ToolTipRectTransform;
    public TMP_Text TooltipLabel;

    RectTransform canvasRectTransform;
    Camera mCam;
    Image Image;

    bool _isShownFlag = false;
    float padding = 4f;
    void Start()
    {
        mCam = Camera.main;
        canvasRectTransform = GetComponent<RectTransform>();
    }

    Vector2 newPosition;
    void Update()
    {
        if (!_isShownFlag) return;
        ToolTipRectTransform.anchoredPosition = Input.mousePosition;
    }

    public void ShowTooltip(string toolTipText)
    {
        ToolTipRectTransform.gameObject.SetActive(true);
        // TooltipLabel.text = toolTipText;
        TooltipLabel.text = "SDJKfhkhjhkae\n<color=red>fhhjhjkekjh</color>\nflfalflfslsl22222";

        TooltipLabel.ForceMeshUpdate();
        Vector2 newSize = new Vector2(TooltipLabel.preferredWidth + padding * 2f, TooltipLabel.preferredHeight + padding * 2f);
        ToolTipRectTransform.sizeDelta = newSize;
        _isShownFlag = true;
    }

    public void HideTooltip()
    {
        ToolTipRectTransform.gameObject.SetActive(false);
        _isShownFlag = false;
    }


    public void OnButtonClick()
    {
        Debug.Log("LFOF");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MessageBox : MonoBehaviour
{
  public Text text_message;
  public Button button_ok;

  public Ease ease_animationDisplay;
  public Ease ease_animationHide;
  [SerializeField]
  private float animation_duration = 0.1f;

  /*private void Update()
  {
    if (Input.GetKeyDown(KeyCode.X))
    {
      HideMessageBox();
    }
    if (Input.GetKeyDown(KeyCode.C))
    {
      DisplayMessageBoxWithMessage("Test Message!!!");
    }
  }*/

  public void DisplayMessageBoxWithMessage(string message)
  {
    if (text_message == null)
    {
      return;
    }
    text_message.text = message;

    RectTransform rectTransform = GetComponent<RectTransform>();
    rectTransform.DOScale(new Vector3(1f, 1f, 1f), animation_duration).SetEase(ease_animationDisplay);
  }

  public void HideMessageBox()
  {
    RectTransform rectTransform = GetComponent<RectTransform>();
    rectTransform.DOScale(new Vector3(0f, 0f, 0f), animation_duration).SetEase(ease_animationHide);
  }

}
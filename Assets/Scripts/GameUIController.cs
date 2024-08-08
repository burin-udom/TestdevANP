using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GamePageStatus
{
  Page_LoginAndRegister,
  Page_GameLobby
}

public class GameUIController : MonoBehaviour
{
  public RectTransform panel_LogIn;
  public RectTransform panel_SignUp;

  public RectTransform panel_LogInAndSignUp;
  public RectTransform panel_GameLobby;

  public float hearts_maxnumber = 10f;
  public Image image_heartFill;
  public Text text_diamonsNumber;

  [SerializeField]
  private float animated_duration = 0.5f;
  public Ease ease_animationDisplay;
  public Ease ease_animationHide;

  private bool isAnimatingTransitionDisplay = false;
  private bool isAnimatingTransitionHide = false;

  private GamePageStatus current_pagestatus = GamePageStatus.Page_LoginAndRegister;
  public GamePageStatus PageStatus
  {
    get { return current_pagestatus; }
    set 
    {
      current_pagestatus = value; 
      switch(current_pagestatus) 
      { 
        case GamePageStatus.Page_LoginAndRegister:
          if (panel_LogInAndSignUp == null || panel_GameLobby == null)
            return;
          panel_LogInAndSignUp.gameObject.SetActive(true);
          panel_GameLobby.gameObject.SetActive(false);
          break;
        case GamePageStatus.Page_GameLobby:
          if (panel_LogInAndSignUp == null || panel_GameLobby == null)
            return;
          panel_LogInAndSignUp.gameObject.SetActive(false);
          panel_GameLobby.gameObject.SetActive(true);
          break;
      }
    }
  }

  void Start()
  {
    AnimateDisplayRectTransform(panel_LogIn);
  }

  public void UpdatePageStatus(int pagestatusIndex) 
  {
    PageStatus = (GamePageStatus)pagestatusIndex;
  }
  public void DisplayLogInPanel()
  {
    if (panel_LogIn == null || panel_SignUp == null)
      return;

    AnimateHideRectTransform(panel_SignUp);
    AnimateDisplayRectTransform(panel_LogIn);
    /*panel_LogIn.gameObject.SetActive(true);
    panel_SignUp.gameObject.SetActive(false);*/
  }
  public void DisplayRegisterPanel()
  {
    if (panel_LogIn == null || panel_SignUp == null)
      return;

    AnimateHideRectTransform(panel_LogIn);
    AnimateDisplayRectTransform(panel_SignUp);
    /*panel_LogIn.gameObject.SetActive(false);
    panel_SignUp.gameObject.SetActive(true);*/
  }

  public void UpdateUserDataDisplay(UserData userData)
  {
    image_heartFill.fillAmount = userData.user_hearts / hearts_maxnumber;

    if (text_diamonsNumber == null)
      return;
    text_diamonsNumber.text = userData.user_diamonds.ToString();
  }

  void AnimateDisplayRectTransform(RectTransform rectTransform)
  {
    if (!isAnimatingTransitionDisplay)
    {
      isAnimatingTransitionDisplay = true;
      rectTransform.gameObject.SetActive(true);
      rectTransform.DOAnchorPos(new Vector2(rectTransform.anchoredPosition.x, 0f), animated_duration).SetEase(ease_animationDisplay).OnComplete(() =>
      {
        isAnimatingTransitionDisplay = false;
      });
    }
  }
  void AnimateHideRectTransform(RectTransform rectTransform)
  {
    if (!isAnimatingTransitionHide)
    {
      isAnimatingTransitionHide = true;
      rectTransform.DOAnchorPos(new Vector2(rectTransform.anchoredPosition.x, 3000f), animated_duration).SetEase(ease_animationHide).OnComplete(() =>
      {
        isAnimatingTransitionHide = false;
      rectTransform.gameObject.SetActive(false);
      });
    }
  }


}

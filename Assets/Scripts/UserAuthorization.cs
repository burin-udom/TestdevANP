using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

[System.Serializable]
public class LoginResponse
{
  public bool success;
  public string message;
  public int user_id;
  public int diamonds;
  public int hearts;
}

[System.Serializable]
public class UserData
{
  public int user_id;
  public string user_username;
  public int user_diamonds;
  public int user_hearts;
}

public class UserAuthorization : MonoBehaviour
{
  public static UserAuthorization instance;

  [SerializeField]
  private string authorize_username;
  [SerializeField]
  private string authorize_password;

  public UserData authorize_userdata;

  public UnityEvent<UserData> onAuthorizeSuccessEvent;
  public UnityEvent<string> onAuthorizeFailedEvent;

  private void Awake()
  {
    if(instance != null)
    {
      return;
    }
    instance = this;
  }

  /*void Start()
  {

  }

  void Update()
  {
    if(Input.GetKeyDown(KeyCode.T))
    {
      Login(authorize_username, authorize_password);
    }
  }*/

  public void StartLogIn()
  {
    Login(authorize_username, authorize_password);
  }
  public void Login(string username, string password)
  {
    StartCoroutine(LoginUserCoroutine(username, password));
  }
  private IEnumerator LoginUserCoroutine(string username, string password)
  {
    WWWForm form = new WWWForm();
    form.AddField("username", username);
    form.AddField("password", password);

    using (UnityWebRequest www = UnityWebRequest.Post("https://test-piggy.codedefeat.com/worktest/dev02/login.php", form))
    {
      yield return www.SendWebRequest();

      if (www.result != UnityWebRequest.Result.Success)
      {
        Debug.Log(www.error);
      }
      else
      {
        // Parse JSON response
        string jsonResponse = www.downloadHandler.text;
        LoginResponse response = JsonUtility.FromJson<LoginResponse>(jsonResponse);

        if (response.success)
        {
          authorize_userdata = new UserData();
          authorize_userdata.user_id = response.user_id;
          authorize_userdata.user_username = username;
          authorize_userdata.user_diamonds = response.diamonds;
          authorize_userdata.user_hearts = response.hearts;
          Debug.Log("Login Successful: " + response.message);
          Debug.Log("Diamonds: " + response.diamonds);
          Debug.Log("Hearts: " + response.hearts);
          onAuthorizeSuccessEvent?.Invoke(authorize_userdata);
        }
        else
        {
          Debug.Log("Login Failed: " + response.message);
          onAuthorizeFailedEvent?.Invoke(response.message);
        }
      }
    }
  }

  public void UpdateAuthorizingUsername(string username)
  {
    authorize_username = username;
  }
  public void UpdateAuthorizingPassword(string password)
  {
    authorize_password = password;
  }

}

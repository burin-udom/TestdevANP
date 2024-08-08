using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

[System.Serializable]
public class RegistrationResponse
{
  public bool success;
  public string message;
}

public class UserRegistration : MonoBehaviour
{
  public static UserRegistration instance;

  [SerializeField]
  private string register_username;
  [SerializeField]
  private string register_password;
  private string register_confirmpassword;

  public UnityEvent<string> onRegisterSuccessEvent;
  public UnityEvent<string> onRegisterFailedEvent;

  public UnityEvent<bool> onPasswordConfirmMatch;

  private void Awake()
  {
    if (instance != null)
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
    if (Input.GetKeyDown(KeyCode.R))
    {
      RegisterNewUser();
    }
  }*/

  public void RegisterNewUser()
  {
    Register(register_username, register_password);
  }
  public void Register(string username, string password)
  {
    StartCoroutine(RegisterUserCoroutine(username, password));
  }
  private IEnumerator RegisterUserCoroutine(string username, string password)
  {
    WWWForm form = new WWWForm();
    form.AddField("username", username);
    form.AddField("password", password);

    using (UnityWebRequest www = UnityWebRequest.Post("https://test-piggy.codedefeat.com/worktest/dev02/register.php", form))
    {
      yield return www.SendWebRequest();

      if (www.result != UnityWebRequest.Result.Success)
      {
        Debug.Log(www.error);
      }
      else
      {
        string jsonResponse = www.downloadHandler.text;
        Debug.Log("Register Response Message: " + jsonResponse);
        RegistrationResponse response = JsonUtility.FromJson<RegistrationResponse>(jsonResponse);

        if (response.success)
        {
          onRegisterSuccessEvent?.Invoke(response.message);
          Debug.Log("User Registered: " + response.message);
        }
        else
        {
          onRegisterFailedEvent?.Invoke(response.message);
          Debug.Log("Registration Failed: " + response.message);
        }
      }
    }
  }

  public void UpdateRegisterUsername(string username)
  {
    register_username = username;
  }
  public void UpdateRegisterPassword(string password)
  {
    register_password = password;
  }
  public void UpdateRegisterConfirmPassword(string password)
  {
    register_confirmpassword = password;

    bool isConfimPasswordMatch = (register_password == register_confirmpassword);
    onPasswordConfirmMatch?.Invoke(isConfimPasswordMatch);
  }

}
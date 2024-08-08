using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[System.Serializable]
public class UpdateResponse
{
  public bool success;
  public string message;
  public int diamonds;
  public int hearts;
}

public class UpdateUserData : MonoBehaviour
{
  public UnityEvent<UserData> onUpdateUserdataSuccessEvent;
  public UnityEvent<string> onUpdateUserdataResponseSuccessEvent;
  public UnityEvent<string> onUpdateUserdataResponseFailedEvent;
  private UserData authorized_userdata;

  private void Start()
  {
    UserAuthorization.instance.onAuthorizeSuccessEvent.AddListener(UpdateAuthorizedUserData);
  }
  /*void Start()
  {

  }

  void Update()
  {

  }*/

  public void UpdateAuthorizedUserData(UserData userdata)
  {
    authorized_userdata = userdata;
  }

  public void AutoIncrementUserDiamonds()
  {
    int newDiamonds = authorized_userdata.user_diamonds + 100;
    UpdateUserDiamonds(newDiamonds);
  }

  public void UpdateUserDiamonds(int diamonds)
  {
    UpdateUser(authorized_userdata.user_id, diamonds, authorized_userdata.user_hearts);
  }
  public void UpdateUserHearts(int hearts)
  {
    UpdateUser(authorized_userdata.user_id, authorized_userdata.user_diamonds, hearts);
  }

  public void UpdateUser(int userId, int diamonds, int hearts)
  {
    StartCoroutine(UpdateUserDataCoroutine(userId, diamonds, hearts));
  }
  private IEnumerator UpdateUserDataCoroutine(int userId, int diamonds, int hearts)
  {
    WWWForm form = new WWWForm();
    form.AddField("user_id", userId);
    form.AddField("diamonds", diamonds);
    form.AddField("hearts", hearts);

    using (UnityWebRequest www = UnityWebRequest.Post("https://test-piggy.codedefeat.com/worktest/dev02/update_user_data.php", form))
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
        UpdateResponse response = JsonUtility.FromJson<UpdateResponse>(jsonResponse);

        if (response.success)
        {
          Debug.Log("Update Successful: " + response.message);
          Debug.Log("Diamonds: " + response.diamonds);
          Debug.Log("Hearts: " + response.hearts);
          authorized_userdata.user_hearts = response.hearts;
          authorized_userdata.user_diamonds = response.diamonds;
          onUpdateUserdataSuccessEvent?.Invoke(authorized_userdata);
          onUpdateUserdataResponseSuccessEvent?.Invoke(response.message);
        }
        else
        {
          Debug.Log("Update Failed: " + response.message);
          onUpdateUserdataResponseFailedEvent?.Invoke(response.message);
        }
      }
    }
  }

}
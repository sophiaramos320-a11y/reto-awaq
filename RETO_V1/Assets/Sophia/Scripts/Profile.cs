using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using static System.Net.WebRequestMethods;
using UnityEngine.UI;

public class User
{
   public int User_ID;
   public string First_Name;
   public string Last_Name;
   public string Email;
   public string Role;
   public string Profile;
}
public class Email_Template
{
    public int Template_ID;
    public string Template_Name;
    public string Subject;
    public string Body;
    public string Registration_Type;
    public bool Available;
}

public class Case
{
    public int CaseNumber;
    public string First_Name;
    public string Last_Name;
    public string Email;
    public string Registration_Type;
    public string Organization_Type;
    public string Organization_Name;
    public string Role;
    public string Phone;
    public string Country_Code;
    public string City;
    public string Country;
    public string Message;
    public bool Privacy_Consent;
    public bool Marketing_Consent;
    public string Status;
    public string Priority;
    public string Subject;
    public DateTime CreatedDate;
    public DateTime ClosedDate;
}
public class Profile : MonoBehaviour
{

    private User currentUser = new User
    {
        First_Name = "Jhon",
        Last_Name = "Ramon"
    };
    
    public Image profileImage;
    void Start()
    {
        string initials = GetInitials(currentUser.First_Name, currentUser.Last_Name);
        string apiProfileUrl = $"https://api.dicebear.com/9.x/initials/png?seed={initials}&radius=20&size=256";

        StartCoroutine(FetchProfilePicture(apiProfileUrl));
    }

    string GetInitials(string firstName, string lastName)
    {
        string firstInitial = firstName[0].ToString();
        string lastInitial = lastName[0].ToString();
        return firstInitial + lastInitial;
    }

    IEnumerator FetchProfilePicture(string url)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error downloading avatar: " + request.error);
            }
            else
            {
                Texture2D downloadedTexture = DownloadHandlerTexture.GetContent(request);

                Sprite avatarSprite = Sprite.Create(
                    downloadedTexture,
                    new Rect(0, 0, downloadedTexture.width, downloadedTexture.height),
                    new Vector2(0.5f, 0.5f)
                );

                profileImage.sprite = avatarSprite;
            }
        }
    }
    void Update()
    {
        
    }
}

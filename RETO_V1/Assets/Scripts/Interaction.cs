using UnityEngine;
using TMPro;

public class Interaction : MonoBehaviour
{
    public string playerTag = "Player";
    public float displayDuration = 3f;
    public string interactionMessage = "Parece que encontrastes el estacionamiento";

    private TMP_Text notifText;
    private float hideTime;

    void Start()
    {
        FindNotifText();
        if (notifText != null)
            notifText.text = string.Empty;
    }

    void Update()
    {
        if (notifText != null && hideTime > 0f && Time.time >= hideTime)
            notifText.text = string.Empty;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
            ShowMessage();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
            ShowMessage();
    }

    private void ShowMessage()
    {
        if (notifText == null)
            FindNotifText();

        Debug.Log(interactionMessage);

        if (notifText != null)
        {
            notifText.text = interactionMessage;
            hideTime = Time.time + displayDuration;
        }
    }

    private void FindNotifText()
    {
        GameObject found = GameObject.FindGameObjectWithTag("notif");
        if (found != null)
            notifText = found.GetComponent<TMP_Text>();
    }
}

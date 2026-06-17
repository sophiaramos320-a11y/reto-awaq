using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UICardElement : MonoBehaviour
{
    [Header("Componentes Visuales Internos")]
    [SerializeField] private TextMeshProUGUI titleText;      
    [SerializeField] private TextMeshProUGUI tagText;        
    [SerializeField] private Button actionButton;            

    private string webUrl; 

    private OrganizationData misDatos; 
    private PopupManager popupManager;

    void Start()
    {
        if (GetComponent<Button>() != null)
        {
            GetComponent<Button>().onClick.AddListener(AlDarClick);
        }
        
        
        if (actionButton != null)
        {
            actionButton.onClick.AddListener(AlDarClick);
        }
    }

    
    public void SetPopupManager(PopupManager manager)
    {
        popupManager = manager;
    }

    public void SetupCard(OrganizationData data)
    {
        misDatos = data;

        titleText.text = data.nombre;
        tagText.text = data.tags;
        webUrl = data.url_redes;
    }

    private void AlDarClick()
    {
        if (popupManager != null && misDatos != null)
        {
            popupManager.MostrarDatos(misDatos);
        }
        else
        {
            if (popupManager == null) Debug.LogError("¡La tarjeta no tiene asignado el PopupManager!");
        }
    }
}
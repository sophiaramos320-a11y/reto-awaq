using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

// Estructura de datos que mapea el JSON de Salesforce
[System.Serializable]
public class OrganizationData
{
    public string nombre;
    public string tags;
    public string url_redes;
    
    public string pais;
    public string descripcion;
    public string url_logo;
}

public class ScrollListManager : MonoBehaviour
{
    [Header("Configuración del Scroll")]
    [SerializeField] private GameObject boxPrefab;       
    [SerializeField] private Transform contentContainer;  

    [Header("NUEVO: Referencia al Popup del Pergamino")]
    [SerializeField] private PopupManager popupGeneral; 

    [Header("Configuración de la API")]
    [SerializeField] private string apiURL = "https://awaq.my.salesforce-sites.com/services/apexrest/organizaciones"; 

    void Start()
    {
        StartCoroutine(FetchDataFromAPI());
    }

    IEnumerator FetchDataFromAPI()
    {
        if (string.IsNullOrEmpty(apiURL))
        {
            Debug.LogError("Por favor, asigna la URL de la API.");
            yield break;
        }

        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiURL))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || 
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error al conectar con la API: {webRequest.error}");
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                BuildListFromJSON(jsonResponse);
            }
        }
    }

    public void BuildListFromJSON(string jsonString)
    {
        // 1. Limpiar el contenedor por si había cajas previas de prueba
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        // Convertir el texto del JSON a un arreglo
        OrganizationData[] organizations = JsonHelper.FromJson<OrganizationData>(jsonString);

        // MEJORA DE SEGURIDAD: 
        if (organizations == null || organizations.Length == 0)
        {
            Debug.LogWarning("No se recibieron organizaciones de la API.");
            return;
        }

        // 2. MODIFICACIÓN: Ahora recorremos TODO el arreglo sin importar cuántas organizaciones vengan
        for (int i = 0; i < organizations.Length; i++)
        {
            GameObject newBox = Instantiate(boxPrefab, contentContainer);
            
            UICardElement cardScript = newBox.GetComponent<UICardElement>();
            if (cardScript != null)
            {
                // Inyectamos la referencia del popup directo en su mano
                cardScript.SetPopupManager(popupGeneral);

                // Enviamos la info completa a la tarjeta
                cardScript.SetupCard(organizations[i]);
            }
        }
    }
}

// Clase estática auxiliar para procesar arreglos puros de JSON en Unity
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string newJson = "{\"Items\":" + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        
        if (wrapper == null || wrapper.Items == null)
        {
            return new T[0];
        }

        return wrapper.Items.ToArray(); 
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public List<T> Items;
    }
}
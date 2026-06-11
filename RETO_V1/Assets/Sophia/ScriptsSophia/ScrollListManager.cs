using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq; // Agregado para poder filtrar y limpiar duplicados fácilmente

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
    [Header("Configuración del Scroll Principal")]
    [SerializeField] private GameObject boxPrefab;       
    [SerializeField] private Transform contentContainer;  

    [Header("Control del Panel de Filtros")]
    [SerializeField] private GameObject filterPanelObject; // El objeto del Panel Pop-up entero
    [SerializeField] private GameObject togglePrefab;      // El prefab que tiene el script UIToggleFilter
    [SerializeField] private Transform toggleContainer;    // El contenedor UI (Grid o Vertical Layout) del panel
    [SerializeField] private UnityEngine.UI.Button openFilterButton; // El botón físico en pantalla para abrir/cerrar el filtro
    
    [Header("NUEVO: Botón de Confirmación")]
    [SerializeField] private UnityEngine.UI.Button confirmFilterButton; // El nuevo botón de "Confirmar" o "Aplicar"

    [Header("Referencia al Popup del Pergamino")]
    [SerializeField] private PopupManager popupGeneral; 

    [Header("Configuración de la API")]
    [SerializeField] private string apiURL = "https://awaq.my.salesforce-sites.com/services/apexrest/organizaciones"; 

    // Listas de control interno
    private List<OrganizationData> allOrganizations = new List<OrganizationData>();
    private List<UIToggleFilter> activeToggles = new List<UIToggleFilter>();
    
    private string currentCountryFilter = "";  // El filtro real aplicado actualmente en el scroll visible
    private string temporaryCountryFilter = ""; // El filtro elegido temporalmente en el panel antes de confirmar

    void Start()
    {
        // Aseguramos que el panel de filtros empiece OCULTO al iniciar la escena
        if (filterPanelObject != null)
        {
            filterPanelObject.SetActive(false);
        }

        // Configuramos el botón principal para que abra o cierre el panel al hacer clic
        if (openFilterButton != null)
        {
            openFilterButton.onClick.AddListener(ToggleFilterPanel);
        }

        // NUEVO: Vinculamos el botón de confirmar con su nueva función
        if (confirmFilterButton != null)
        {
            confirmFilterButton.onClick.AddListener(ConfirmAndApplyFilter);
        }

        StartCoroutine(FetchDataFromAPI());
    }

    // Función que invierte el estado del panel
    public void ToggleFilterPanel()
    {
        if (filterPanelObject != null)
        {
            bool willOpen = !filterPanelObject.activeSelf;
            filterPanelObject.SetActive(willOpen);

            // Si el usuario abre el panel, sincronizamos la selección temporal con lo que esté guardado de antes
            if (willOpen)
            {
                temporaryCountryFilter = currentCountryFilter;
                SyncTogglesVisuals(temporaryCountryFilter);
            }
        }
    }

    // NUEVO: Aplica el filtro acumulado y cierra la ventana
    private void ConfirmAndApplyFilter()
    {
        currentCountryFilter = temporaryCountryFilter;
        ApplyFilter();

        if (filterPanelObject != null)
        {
            filterPanelObject.SetActive(false); // Cierra el panel al confirmar
        }
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
        OrganizationData[] organizations = JsonHelper.FromJson<OrganizationData>(jsonString);

        if (organizations == null || organizations.Length == 0)
        {
            Debug.LogWarning("No se recibieron organizaciones de la API.");
            return;
        }

        allOrganizations = new List<OrganizationData>(organizations);

        GenerateCountryToggles();
        RenderList(allOrganizations);
    }

    private void GenerateCountryToggles()
    {
        foreach (Transform child in toggleContainer)
        {
            Destroy(child.gameObject);
        }
        activeToggles.Clear();

        List<string> uniqueCountries = allOrganizations
            .Select(o => o.pais)
            .Where(p => !string.IsNullOrEmpty(p))
            .Distinct()
            .OrderBy(p => p)
            .ToList();

        foreach (string country in uniqueCountries)
        {
            GameObject newToggle = Instantiate(togglePrefab, toggleContainer);
            newToggle.name = country; 
            
            UIToggleFilter toggleScript = newToggle.GetComponent<UIToggleFilter>();
            if (toggleScript != null)
            {
                toggleScript.SetupToggle(country, this);
                activeToggles.Add(toggleScript);
            }
        }
    }

    // MODIFICADO: Ahora este método solo altera variables temporales, no refresca el scroll del fondo
    internal void OnFilterToggleChanged(string countryName, bool isOn)
    {
        if (isOn)
        {
            foreach (var t in activeToggles)
            {
                if (t != null && t.gameObject.name != countryName)
                {
                    t.SetIsOnWithoutNotify(false);
                }
            }

            temporaryCountryFilter = countryName;
        }
        else
        {
            if (temporaryCountryFilter == countryName)
            {
                temporaryCountryFilter = "";
            }
        }
    }

    // NUEVO: Asegura que los checkboxes se marquen/desmarquen correctamente de acuerdo al filtro real actual al abrir
    private void SyncTogglesVisuals(string targetFilter)
    {
        foreach (var t in activeToggles)
        {
            if (t != null)
            {
                t.SetIsOnWithoutNotify(t.gameObject.name == targetFilter);
            }
        }
    }

    private void ApplyFilter()
    {
        if (string.IsNullOrEmpty(currentCountryFilter))
        {
            RenderList(allOrganizations);
        }
        else
        {
            List<OrganizationData> filteredList = allOrganizations
                .Where(o => o.pais == currentCountryFilter)
                .ToList();
            
            RenderList(filteredList);
        }
    }

    private void RenderList(List<OrganizationData> targetList)
    {
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < targetList.Count; i++)
        {
            GameObject newBox = Instantiate(boxPrefab, contentContainer);
            
            UICardElement cardScript = newBox.GetComponent<UICardElement>();
            if (cardScript != null)
            {
                cardScript.SetPopupManager(popupGeneral);
                cardScript.SetupCard(targetList[i]);
            }
        }
    }
}

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
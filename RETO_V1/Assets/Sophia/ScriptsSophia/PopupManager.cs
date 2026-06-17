using UnityEngine;
using TMPro; 
using UnityEngine.UI; 

public class PopupManager : MonoBehaviour
{
    [Header("Referencias de Texto en el Panel")]
    [SerializeField] private TextMeshProUGUI campoNombre;       
    [SerializeField] private TextMeshProUGUI campoDescripcion;  
    [SerializeField] private TextMeshProUGUI campoPais;         
    [SerializeField] private TextMeshProUGUI campoTags;

    [Header("Configuración del Botón Web")]
    [SerializeField] private Button botonRedes;                 

    private string urlActual;                                   

    void Start()
    {
        
        if (botonRedes != null)
        {
            botonRedes.onClick.AddListener(AbrirEnlaceWeb);
        }

        CerrarPanel(); 
    }

    public void MostrarDatos(OrganizationData datos)
    {
        campoNombre.text = datos.nombre;
        campoDescripcion.text = datos.descripcion;
        campoPais.text = datos.pais;
        campoTags.text = datos.tags;

        
        urlActual = datos.url_redes;

    
        if (botonRedes != null)
        {
            botonRedes.gameObject.SetActive(!string.IsNullOrEmpty(urlActual));
        }

    
        gameObject.SetActive(true);
    }

    
    private void AbrirEnlaceWeb()
    {
        if (!string.IsNullOrEmpty(urlActual))
        {
            
            string urlFinal = urlActual;
            if (!urlFinal.StartsWith("http://") && !urlFinal.StartsWith("https://"))
            {
                urlFinal = "https://" + urlFinal;
            }

            Debug.Log($"Abriendo sitio web: {urlFinal}");
            Application.OpenURL(urlFinal);
        }
    }

    
    public void CerrarPanel()
    {
        gameObject.SetActive(false);
    }
}
using UnityEngine;
using TMPro; // Necesario para TextMeshPro
using UnityEngine.UI; // Necesario para manejar componentes Button

public class PopupManager : MonoBehaviour
{
    [Header("Referencias de Texto en el Panel")]
    [SerializeField] private TextMeshProUGUI campoNombre;       // Tu objeto NombOrg
    [SerializeField] private TextMeshProUGUI campoDescripcion;  // Tu texto de Descripción
    [SerializeField] private TextMeshProUGUI campoPais;         // Tu texto de País
    [SerializeField] private TextMeshProUGUI campoTags;

    [Header("Configuración del Botón Web")]
    [SerializeField] private Button botonRedes;                 // El botón físico en tu UI

    private string urlActual;                                   // Guarda temporalmente la URL de la Org activa

    void Start()
    {
        // Vinculamos la función del click por código para que nunca se pierda la referencia
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

        // Guardamos la URL que viene desde Salesforce para esta organización específica
        urlActual = datos.url_redes;

        // Opcional: Si la organización no tiene URL cargada, podemos apagar el botón para que no confunda
        if (botonRedes != null)
        {
            botonRedes.gameObject.SetActive(!string.IsNullOrEmpty(urlActual));
        }

        // Mostramos el pergamino en pantalla
        gameObject.SetActive(true);
    }

    // Función que se activa al presionar el botón web
    private void AbrirEnlaceWeb()
    {
        if (!string.IsNullOrEmpty(urlActual))
        {
            // Forzamos un formateo rápido por si la URL de Salesforce no incluye el protocolo
            string urlFinal = urlActual;
            if (!urlFinal.StartsWith("http://") && !urlFinal.StartsWith("https://"))
            {
                urlFinal = "https://" + urlFinal;
            }

            Debug.Log($"Abriendo sitio web: {urlFinal}");
            Application.OpenURL(urlFinal);
        }
    }

    // Vincula este método al evento OnClick de tu botón de cerrar
    public void CerrarPanel()
    {
        gameObject.SetActive(false);
    }
}
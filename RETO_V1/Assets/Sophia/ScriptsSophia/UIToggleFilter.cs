using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIToggleFilter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countryText;
    [SerializeField] private Toggle toggleComponent;

    private string countryName;
    private ScrollListManager listManager;

    public void SetupToggle(string country, ScrollListManager manager)
    {
        countryName = country;
        listManager = manager;

        // Si el país viene vacío en la API, ponemos un texto genérico
        countryText.text = string.IsNullOrEmpty(country) ? "Otros" : country;

        // Limpiamos eventos previos para evitar errores al reutilizar o instanciar
        toggleComponent.onValueChanged.RemoveAllListeners();
        
        // Nos suscribimos al evento cuando el toggle cambia de estado (marcado/desmarcado)
        toggleComponent.onValueChanged.AddListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged(bool isOn)
    {
        // Le avisamos al manager que este toggle en específico ha cambiado
        listManager.OnFilterToggleChanged(countryName, isOn);
    }

    // Método público por si el manager necesita desmarcar el toggle desde fuera
    public void SetIsOnWithoutNotify(bool isOn)
    {
        toggleComponent.SetIsOnWithoutNotify(isOn);
    }
}
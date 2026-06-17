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

        countryText.text = string.IsNullOrEmpty(country) ? "Otros" : country;

        
        toggleComponent.onValueChanged.RemoveAllListeners();
        
    
        toggleComponent.onValueChanged.AddListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged(bool isOn)
    {
        
        listManager.OnFilterToggleChanged(countryName, isOn);
    }

   
    public void SetIsOnWithoutNotify(bool isOn)
    {
        toggleComponent.SetIsOnWithoutNotify(isOn);
    }
}
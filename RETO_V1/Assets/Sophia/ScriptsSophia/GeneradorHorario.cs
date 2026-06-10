using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Estructuras de datos para reflejar el formato exacto del JSON
[Serializable]
public class Taller
{
    public string hora_inicio;
    public string hora_fin;
    public string titulo;
}

[Serializable]
public class DiaCronograma
{
    public int dia;
    public List<Taller> talleres;
}

[Serializable]
public class EventoHorario
{
    public string evento;
    public List<DiaCronograma> cronograma;
}

public class GeneradorHorario : MonoBehaviour
{
    [Header("UI Config")]
    public Transform contenedor;     // Arrastra aquí 'ListaDeTalleres'
    public GameObject prefabTaller;   // Arrastra aquí el Prefab 'ElementoTaller'
    
    [Header("Datos")]
    [TextArea(5, 10)]
    public string jsonSimulado;       // Pegaremos el JSON aquí para probar

    void Start()
    {
        CargarHorario();
    }

    public void CargarHorario()
    {
        // 1. Limpiar elementos viejos por si se actualiza en vivo
        foreach (Transform hijo in contenedor)
        {
            Destroy(hijo.gameObject);
        }

        // 2. Convertir el texto JSON en objetos de C#
        EventoHorario datosEvento = JsonUtility.FromJson<EventoHorario>(jsonSimulado);

        // 3. Recorrer los días y los talleres para clonar el Prefab
        foreach (DiaCronograma dia in datosEvento.cronograma)
        {
            // Opcional: Podrías instanciar un título para el "Día 1", "Día 2", etc.
            
            foreach (Taller taller in dia.talleres)
            {
                // Clonar el molde dentro del contenedor
                GameObject nuevoTaller = Instantiate(prefabTaller, contenedor);
                
                // Buscar el componente de texto en el clon
                TextMeshProUGUI textoTaller = nuevoTaller.GetComponentInChildren<TextMeshProUGUI>();
                
                // Darle formato al texto con los datos del JSON
                textoTaller.text = $"<b>[{taller.hora_inicio} - {taller.hora_fin}]</b> {taller.titulo}";
            }
        }
    }
}
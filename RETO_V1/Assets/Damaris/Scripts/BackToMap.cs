using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMap : MonoBehaviour
{
    public void GoToMap()
    {
        Debug.Log("Botón presionado");
        SceneManager.LoadScene("Mapa");
    }
}
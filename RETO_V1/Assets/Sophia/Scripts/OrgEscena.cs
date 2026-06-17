using UnityEngine;
using UnityEngine.SceneManagement;

public class OrgEscena : MonoBehaviour
{
    public void CargarRecomendaciones()
    {
        SceneManager.LoadScene("Recomendaciones");
    }
}
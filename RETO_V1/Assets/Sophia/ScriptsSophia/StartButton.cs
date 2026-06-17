using UnityEngine;
using UnityEngine.SceneManagement;
public class StartButton : MonoBehaviour
{
    
    public void LoadMapScene()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
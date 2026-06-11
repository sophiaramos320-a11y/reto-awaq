using UnityEngine;
using UnityEngine.SceneManagement;
public class StartButton : MonoBehaviour
{
    // 2. Put the code inside a public function so your button can see it
    public void LoadMapScene()
    {
        // 3. Call the Unity SceneManager
        SceneManager.LoadScene("Mapa");
    }
}
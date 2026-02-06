using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenemanager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Hallway");
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Application Quitted");
    }
}

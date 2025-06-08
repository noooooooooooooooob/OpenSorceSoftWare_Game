using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public void LoadScene(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }
    public void ReloadScene()
    {
        Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene.name);
    }
}
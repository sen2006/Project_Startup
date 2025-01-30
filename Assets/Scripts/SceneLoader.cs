using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void switchToScene(string Scene)
    {
       SceneManager.LoadScene(Scene);
    }
}

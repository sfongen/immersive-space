using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadPlaygroundScene : MonoBehaviour
{
    public void LoadPlayground()
    {
        SceneManager.LoadScene("Playground");
    }
}

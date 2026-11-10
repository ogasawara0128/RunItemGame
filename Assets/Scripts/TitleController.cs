using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    public string nextScene;

    public void NewGame()
    {
        SceneManager.LoadScene(nextScene);
    }
}

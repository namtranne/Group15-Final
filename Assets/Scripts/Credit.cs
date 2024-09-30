using UnityEngine;
using UnityEngine.SceneManagement;

public class Credit : MonoBehaviour
{
    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameSelection : MonoBehaviour
{
	public void QuitButton()
	{
		SceneManager.LoadScene(0, LoadSceneMode.Single);
	}

	public void StartGame()
	{
		SceneManager.LoadScene(1, LoadSceneMode.Single);
	}
}

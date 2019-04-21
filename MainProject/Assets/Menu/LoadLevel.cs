using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
  public GameObject loadingScreen;
  public Slider slider;
  public Text textValue;

  public void PlayGame()
  {
    int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
    StartCoroutine(LoadNextLevel(nextLevel));
  }

  IEnumerator LoadNextLevel(int sceneIndex)
  {
    AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

    loadingScreen.SetActive(true);

    while (!operation.isDone)
    {
      float progress = Mathf.Clamp01(operation.progress / .9f);
      slider.value = progress;
      textValue.text = progress + "%";

      yield return null;
    }

  }

}
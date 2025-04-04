using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuInicial : MonoBehaviour
{
    public AudioSource musicaFondo;
    public float fadeDuration = 1f;

    public void Start()
    {
        musicaFondo.Play();
    }

    public void Jugar()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(DesvanecerMusicaYCambiarEscena(nextSceneIndex));
        }
        else
        {
            Debug.LogWarning("No hay más escenas en el Build Settings.");
        }
    }

    public void Salir()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Debug.LogWarning("Salir no funciona en WebGL.");
        }
        else
        {
            Debug.Log("Saliendo del juego...");
            Application.Quit();
        }
    }

    public void CambiarEscena(string nombreEscena)
    {
        StartCoroutine(DesvanecerMusicaYCambiarEscena(nombreEscena));
    }

    private IEnumerator DesvanecerMusicaYCambiarEscena(int sceneIndex)
    {
        float startVolume = musicaFondo.volume;

        while (musicaFondo.volume > 0)
        {
            musicaFondo.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicaFondo.Stop();

        SceneManager.LoadScene(sceneIndex);
    }

    private IEnumerator DesvanecerMusicaYCambiarEscena(string sceneName)
    {
        float startVolume = musicaFondo.volume;

        while (musicaFondo.volume > 0)
        {
            musicaFondo.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicaFondo.Stop();

        SceneManager.LoadScene(sceneName);
    }
}

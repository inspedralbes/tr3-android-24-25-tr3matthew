using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public AudioSource musicaFondo;
    public float fadeDuration = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

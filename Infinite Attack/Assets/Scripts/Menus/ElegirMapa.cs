using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ElegirMapa : MonoBehaviour
{
    public AudioSource musicaFondo;
    public float fadeDuration = 1f;

    void Start()
    {
        musicaFondo = GetComponent<AudioSource>();
        musicaFondo.Play();
    }

    public void CambiarEscena(string nombreEscena)
    {
        StartCoroutine(DesvanecerMusicaYCambiarEscena(nombreEscena));
    }

    public void Atras(string nombreEscena)
    {
        StartCoroutine(DesvanecerMusicaYCambiarEscena(nombreEscena));
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

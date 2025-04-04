using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
public class SueloProperties : MonoBehaviour
{
    public AudioSource musicaFondo;
    public float fadeDuration = 1f;
    public int caidos = 0;
    public int limitCaidos;
    public bool gameOver = false;
    public TextMeshProUGUI recogidosText;
    public LimintesManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        recogidosText.text = $"Vidas: {limitCaidos-caidos}";
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            caidos++;
            Debug.Log("Caido: " + caidos);
            Destroy(other.gameObject);
            if (caidos >= limitCaidos ) {
                gameManager.EndGame();
                SceneManager.LoadScene("GameOver");
            }
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

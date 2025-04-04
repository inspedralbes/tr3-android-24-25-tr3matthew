using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class LimintesManager : MonoBehaviour
{
    public SueloProperties sueloProperties;
    public playercontroller playercontroller;

    public int limitescaidas = 10;
    public int recogido;
    public string serverUrl = "http://localhost:3000/partidas";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sueloProperties.limitCaidos = limitescaidas;
        recogido = playercontroller.recogidos;
    }
    public void EndGame()
    {
        int objetosRecogidos = playercontroller.recogidos;
        StartCoroutine(SendGameData(objetosRecogidos));
    }
    
    IEnumerator SendGameData(int recogidos)
    {
        // Crear el objeto JSON manualmente
        string jsonData = $"{{\"objetosRecogidos\":{recogidos}}}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest www = new UnityWebRequest(serverUrl, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.timeout = 10;

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error al enviar datos: {www.error}");
                Debug.LogError($"Respuesta del servidor: {www.downloadHandler.text}");
            }
            else
            {
                Debug.Log($"Datos guardados correctamente: {www.downloadHandler.text}");
            }
        }
    }
}


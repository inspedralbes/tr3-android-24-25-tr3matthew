using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class RainImageSpawner : MonoBehaviour
{
    [Header("Server Configuration")]
    public string serverUrl = "http://localhost:3000/imagenes";
    
    [Header("Spawn Settings")]
    public GameObject imagePrefab; // Asegúrate que tiene SpriteRenderer
    public float spawnRate = 1f;
    public float fallSpeed = 2f;
    public Vector2 xRange = new Vector2(-5f, 5f);
    public float spawnHeight = 10f;

    private Queue<string> imageUrls = new Queue<string>();
    private float nextSpawnTime;
    private bool dataLoaded = false;

    void Start()
    {
        StartCoroutine(LoadImageUrls());
    }

    void Update()
    {
        if (!dataLoaded || Time.time < nextSpawnTime) return;

        SpawnFallingImage();
        nextSpawnTime = Time.time + 1f / spawnRate;
    }

    IEnumerator LoadImageUrls()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(serverUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = webRequest.downloadHandler.text;
                ImageDataWrapper wrapper = JsonUtility.FromJson<ImageDataWrapper>("{\"images\":" + jsonResponse + "}");

                foreach (ImageData imgData in wrapper.images)
                {
                    imageUrls.Enqueue(imgData.url);
                }

                dataLoaded = true;
                nextSpawnTime = Time.time;
            }
            else
            {
                Debug.LogError("Error loading images: " + webRequest.error);
            }
        }
    }

    void SpawnFallingImage()
    {
        if (imageUrls.Count == 0) return;

        string url = imageUrls.Dequeue();
        imageUrls.Enqueue(url); // Recycle URL for infinite loop

        float randomX = Random.Range(xRange.x, xRange.y);
        Vector3 spawnPosition = new Vector3(randomX, spawnHeight, 0);

        StartCoroutine(LoadAndAssignSprite(url, spawnPosition));
    }

    IEnumerator LoadAndAssignSprite(string imageUrl, Vector3 position)
    {
        using (UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return imageRequest.SendWebRequest();

            if (imageRequest.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(imageRequest);
                Sprite sprite = Sprite.Create(
                    texture, 
                    new Rect(0, 0, texture.width, texture.height), 
                    Vector2.one * 0.5f
                );

                GameObject newImage = Instantiate(imagePrefab, position, Quaternion.identity);
                
                // ÚNICO CAMBIO ESENCIAL: Asignación directa al SpriteRenderer
                SpriteRenderer renderer = newImage.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    renderer.sprite = sprite;
                }

                // Configuración del movimiento de caída (sin cambios)
                FallingImage falling = newImage.AddComponent<FallingImage>();
                falling.speed = fallSpeed;
            }
        }
    }
}

// Clase para el movimiento de caída (sin cambios)
public class FallingImage : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
}

[System.Serializable]
public class ImageData
{
    public string url;
}

[System.Serializable]
public class ImageDataWrapper
{
    public List<ImageData> images;
}
/*using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ImageGridManager : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform gridParent;   
    public string battleSceneName = "batalla";

    public static Texture2D SelectedBackgroundTexture { get; private set; }

    private string apiUrl = "http://localhost:3000/imagenes";

    void Start()
    {
        StartCoroutine(FetchImagesAndCreateButtons());
    }

    IEnumerator FetchImagesAndCreateButtons()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            ImageData[] images = JsonUtility.FromJson<ImageArray>("{\"imagenes\":" + request.downloadHandler.text + "}").imagenes;
            
            foreach (var img in images)
            {
                StartCoroutine(CreateImageButton(img.url));
            }
        }
        else
        {
            Debug.LogError("Error al cargar imágenes: " + request.error);
        }
    }

    IEnumerator CreateImageButton(string imageUrl)
    {
        UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return textureRequest.SendWebRequest();

        if (textureRequest.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = ((DownloadHandlerTexture)textureRequest.downloadHandler).texture;

            GameObject buttonObj = Instantiate(buttonPrefab, gridParent);
            RawImage buttonImage = buttonObj.GetComponentInChildren<RawImage>();
            if (buttonImage != null)
            {
                buttonImage.texture = texture;
            }

            Button button = buttonObj.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => OnSelectBackground(texture));
            }
        }
    }

    private void OnSelectBackground(Texture2D selectedTexture)
    {
        SelectedBackgroundTexture = selectedTexture;
        SceneManager.LoadScene(battleSceneName);
    }
}

public class BattleManager : MonoBehaviour
{
    public GameObject fallingItemPrefab;
    public Transform spawnArea;
    public Text collectedText;
    public Text missedText;

    private int collected = 0;
    private int missed = 0;
    private int maxMissed = 5;
    private string saveUrl = "http://localhost:3000/saveResults";

    void Start()
    {
        if (ImageGridManager.SelectedBackgroundTexture != null)
        {
            StartCoroutine(SpawnFallingItems());
        }
    }

    IEnumerator SpawnFallingItems()
    {
        while (missed < maxMissed)
        {
            SpawnItem();
            yield return new WaitForSeconds(1f);
        }
        SaveResults();
    }

    void SpawnItem()
    {
        GameObject newItem = Instantiate(fallingItemPrefab, spawnArea.position, Quaternion.identity);
        newItem.GetComponent<Rigidbody2D>().gravityScale = 1;
        newItem.GetComponent<SpriteRenderer>().sprite = TextureToSprite(ImageGridManager.SelectedBackgroundTexture);
        newItem.AddComponent<ItemController>().Setup(this);
    }

    public void ItemCollected()
    {
        collected++;
        collectedText.text = "Recolectados: " + collected;
    }

    public void ItemMissed()
    {
        missed++;
        missedText.text = "Caídos: " + missed;
    }

    private void SaveResults()
    {
        StartCoroutine(SendResults());
    }

    IEnumerator SendResults()
    {
        WWWForm form = new WWWForm();
        form.AddField("collected", collected);
        form.AddField("missed", missed);

        UnityWebRequest request = UnityWebRequest.Post(saveUrl, form);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error al guardar resultados: " + request.error);
        }
    }

    Sprite TextureToSprite(Texture2D texture)
    {
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        return Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
    }
}

public class ItemController : MonoBehaviour
{
    private BattleManager battleManager;

    public void Setup(BattleManager manager)
    {
        battleManager = manager;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            battleManager.ItemCollected();
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            battleManager.ItemMissed();
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
public class ImageArray
{
    public ImageData[] imagenes;
}
*/
using UnityEngine;

public class ItemProperties : MonoBehaviour
{
    public int recogido = 0;
    public int caido = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        /*if (other.CompareTag("Player"))
        {
            Debug.Log("Capturado!!");
            recogido++;
            Debug.Log("Recodigo: "+ recogido);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Bordes"))
        {
            Debug.Log("Caido!!");
            caido++;
            Debug.Log("Caido: "+ caido);
            Destroy(gameObject);
        }*/
        if (other.CompareTag("Pared"))
        {
            Debug.Log("Fuera del juego!!");
            Destroy(gameObject);
        }
    }
    private void OnTriggerExit2D()
    {
        Debug.Log("Recodigo: "+ recogido);
        Debug.Log("Caido: "+ caido);
    }
}

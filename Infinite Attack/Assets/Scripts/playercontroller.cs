using UnityEngine;

public class playercontroller : MonoBehaviour
{
    public float speed = 10f;
    public float fuerzaSalto = 20f;
    public Collider2D cantanteCollider;
    public float saltosMaximos;
    private Rigidbody2D rigidBody;
    private bool grounded = true;
    private float saltosRestantes;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movimiento();
        
    }

    void Movimiento()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, 0) * speed * Time.deltaTime;
        Salto();
    }

    void Salto()
    {
        if((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && saltosRestantes > 0)
        {
            rigidBody.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            saltosRestantes--;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bordes"))
        {
            saltosRestantes = saltosMaximos;
            grounded = true;
            Debug.Log(grounded);
        }
    }
    private void OnCollisionExit2D(Collision2D collison)
    {
        if(collison.gameObject.CompareTag("Bordes"))
        {
            grounded = false;
            Debug.Log(grounded);
        }
    }
}


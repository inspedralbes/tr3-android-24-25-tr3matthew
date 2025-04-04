using UnityEngine;
using System.Collections;

public class playercontroller : MonoBehaviour
{
    public Animator animator;
    public float speed = 10f;
    public float fuerzaSalto = 20f;
    public Collider2D cantanteCollider;
    public float saltosMaximos = 1;
    private Rigidbody2D rigidBody;
    private bool grounded = true;
    private float saltosRestantes;
    public int recogidos = 0;

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
        float VelocidadX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.position += new Vector3(VelocidadX, 0, 0);
        animator.SetFloat("movement", VelocidadX * speed);

        if (VelocidadX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (VelocidadX > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        Salto();
    }

    void Salto()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && saltosRestantes > 0)
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, 0);
            rigidBody.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            saltosRestantes--;
            /*impulso = fuerzaSalto;
            if (impulso>=0)
            {
                impulso -= 2f * Time.deltaTime;
                animator.SetFloat("impulso", impulso);
            }*/
        }
        if (rigidBody.linearVelocity.y > 0)
        {
            animator.SetBool("impulso", true);
        }
        else if (rigidBody.linearVelocity.y < 0)
        {
            animator.SetBool("impulso", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bordes"))
        {
            saltosRestantes = saltosMaximos;
            grounded = true;
            animator.SetBool("grounded", grounded);
            Debug.Log(grounded);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            recogidos++;
            Debug.Log("Recogido: " + recogidos);
            Destroy(other.gameObject);

        }
    }
    private void OnCollisionExit2D(Collision2D collison)
    {
        if (collison.gameObject.CompareTag("Bordes"))
        {
            grounded = false;
            animator.SetBool("grounded", grounded);
            Debug.Log(grounded);
        }
    }
}


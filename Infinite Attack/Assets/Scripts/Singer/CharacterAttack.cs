using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    public Animator animator;
    public int combo;
    public bool atacando;
    public AudioSource audio_S;
    public AudioClip[] sonido;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        audio_S = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Combos();
    }
    public void Combos()
    {
        if(Input.GetKeyDown(KeyCode.J) && !atacando)
        {
            atacando = true;
            animator.SetTrigger(""+combo);
            //audio_S.clip = sonido[combo];
            //audio_S.Play();
        }
    }
    public void Start_Combo()
    {
        atacando = false;
        if(combo < 3)
        {
            combo++;
            Debug.Log(combo);
        }
    }
    public void Finish_Combo()
    {
        atacando = false;
        combo = 0;
        Debug.Log(combo);
    }
}

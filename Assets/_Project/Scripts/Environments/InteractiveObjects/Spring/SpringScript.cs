using System.Reflection;
using UnityEngine;

public class SpringScript : MonoBehaviour
{
    public Animator anim;
    public float jumpforce=20;
    void Start()
    {
        anim=GetComponent<Animator>();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            anim.SetBool("OnSpring",true);
        }
        Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
    if (collision.contacts[0].normal.y<-0.5f&&rb.linearVelocity.y <= 0f)
    {
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
        }
    }
    }

        private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            anim.SetBool("OnSpring",false);
        }

    }

    void Update()
    {
        
    }
}

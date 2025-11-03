using UnityEngine;

public class Trampolin : MonoBehaviour
{
    public float fuerzaSalto = 15f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayTrampolin();
            }
        }
    }
}

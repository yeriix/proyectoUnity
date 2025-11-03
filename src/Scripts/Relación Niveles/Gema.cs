using UnityEngine;

public class Gema : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.SumarPuntos(1);
            AudioManager.instance.PlayMoneda();
            Destroy(gameObject);
        }
    }
}

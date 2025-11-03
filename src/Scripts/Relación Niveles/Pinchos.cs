using UnityEngine;

public class Pinchos : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MovimientoJugador jugador = collision.GetComponent<MovimientoJugador>();
            if (jugador != null)
            {
                jugador.Die();
            }
        }
    }
}

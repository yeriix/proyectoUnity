using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] private float speed = 2f; // Velocidad a la que la lava sube (se edita en el inspector)
    [SerializeField] private float maxHeight = 10f; // Altura máxima que puede alcanzar (se edita en el inspector)

    private bool detenida = false; //esto para el boton para q la lava se detenga

    void Update()
    {
        if (transform.position.y < maxHeight)
            transform.position += Vector3.up * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MovimientoJugador jugador = other.GetComponent<MovimientoJugador>();
            if (jugador != null)
            {
                jugador.Die(); 
            }
        }
    }
    public void DetenerLava()
    {
        speed = 0f;
        detenida = true;
    }
}

using UnityEngine;

public class MovimientoCamara : MonoBehaviour
{
    // A qué objeto queremos que siga la cámara
    public Transform jugador;

    // Qué tan rápido reacciona la cámara al movimiento del jugador
    [Range(0f, 1f)]
    public float suavizado = 0.1f;

    // Offset: distancia fija entre la cámara y el jugador
    public Vector3 desplazamiento;

    // Si el jugador muere, la cámara deja de seguirlo
    [HideInInspector]
    public bool seguirJugador = true;

    private void LateUpdate()
    {
        // LateUpdate asegura que la cámara se mueva DESPUÉS de que el jugador ya se ha movido

        if (!seguirJugador) return; // no seguimos más (por ejemplo, si la palma)

        // calculamos dónde debería estar la cámara ahora mismo
        Vector3 posicionObjetivo = jugador.position + desplazamiento;

        // movimiento mas suave en lugar de brusco, que no se vea raro
        transform.position = Vector3.Lerp(transform.position, posicionObjetivo, suavizado);
    }
}

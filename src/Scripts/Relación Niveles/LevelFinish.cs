using UnityEngine;

public class LevelFinish : MonoBehaviour
{
    private bool completado = false; // evita activar la victoria 2 veces

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (completado) return; // si ya se compeltó, no hacer nada
        if (!other.CompareTag("Player")) return; // funciona con el player solo
        completado = true;
        UIManager.instance.MostrarVictoria(); //mostramos la victoria
    }
}

using UnityEngine;
using TMPro;

public class PlacaPresion : MonoBehaviour
{
    public Lava lava; // referencia al script de la lava
    public TextMeshProUGUI mensajeUI; // texto que aparece cuando activas la placa
    public string mensaje = "¡La lava ha sido detenida!";
    private bool activada = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activada) return; // solo se activa 1 vez
        if (!collision.CompareTag("Player")) return;
        activada = true;

        // detener la lava
        lava.DetenerLava();

        // mostrar texto temporal
        if (mensajeUI != null)
        {
            mensajeUI.gameObject.SetActive(true);
            mensajeUI.text = mensaje;
        }
        Destroy(gameObject);
    }
}

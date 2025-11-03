using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    public TextMeshProUGUI gemasText; // texto que muestra las gemas recogidas

    private void Start()
    {
        // evento que hace que se actualice al cambiar puntos
        if (GameManager.instance != null)
        {
            GameManager.instance.OnPuntosCambiados += ActualizarGemas;
            // mostramos las gemas actuales 
            ActualizarGemas(GameManager.instance.PuntosTotales);
        }
    }
    private void OnDestroy()
    {
        if (GameManager.instance != null)
            GameManager.instance.OnPuntosCambiados -= ActualizarGemas;
    }

    // Cambia el texto del HUD cuando cambian las gemas
    void ActualizarGemas(int puntos)
    {
        gemasText.text = "Gemas: " + puntos;
    }
}

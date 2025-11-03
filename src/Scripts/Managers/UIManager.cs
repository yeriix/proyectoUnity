using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;


public class UIManager : MonoBehaviour
{
    // Singleton (sirve para que desde cualquier script podamos llamar a esta clase)
    public static UIManager instance;

    [Header("Paneles UI")]
    public GameObject panelPausa;
    public GameObject panelVictoria;
    public GameObject panelMuerte;

    [Header("Textos de Victoria")]
    public TextMeshProUGUI gemasText;
    public TextMeshProUGUI tiempoText;

    [Header("Variables internas")]
    private bool juegoPausado = false;
    private bool nivelFinalizado = false;

    [Header("Botón Reanudar (solo pausa)")]
    public GameObject botonReanudar;

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1f; // Aseguramos que la escena no empiece pausada
    }

    private void Update()
    {
        // Si el nivel no ha acabado, podemos abrir/cerrar menú con ESC
        if (!nivelFinalizado && Input.GetKeyDown(KeyCode.Escape))
        {
            if (!juegoPausado) ActivarPausa();
            else ReanudarJuego();
        }
    }

    // Activa el panel de pausa y detiene el juego
    public void ActivarPausa()
    {
        panelPausa.SetActive(true);

        if (botonReanudar != null)
            botonReanudar.SetActive(true); // solo en pausa

        Time.timeScale = 0f;
        juegoPausado = true;
    }


    // Cierra el menú y vuelve al juego
    public void ReanudarJuego()
    {
        panelPausa.SetActive(false);
        Time.timeScale = 1f;
        juegoPausado = false;
    }

    // Se llama cuando el jugador toca la meta
    public void MostrarVictoria()
    {
        nivelFinalizado = true;
        panelVictoria.SetActive(true);

        // Actualizar texto de gemas
        gemasText.text = "Gemas: " + GameManager.instance.PuntosTotales;

        // Mostrar tiempo del Timer
        float tiempo = Timer.instance.ObtenerTiempo();
        int min = Mathf.FloorToInt(tiempo / 60f);
        int seg = Mathf.FloorToInt(tiempo % 60f);
        tiempoText.text = $"Tiempo: {min}:{seg:00}";

        Time.timeScale = 0f;
        AudioManager.instance.PlayVictory();
    }

    // Se llamará si el jugador muere
    public void MostrarMuerte()
    {
        nivelFinalizado = true;
        panelMuerte.SetActive(true);

        if (botonReanudar != null)
            botonReanudar.SetActive(false); // lo ocultamos

        Time.timeScale = 0f;
    }

    // Reiniciar el nivel actual
    public void ReiniciarNivel()
    {
        ResetEstado();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Después de recargar la escena, asegurarse que la música se reproduzca
        if (AudioManager.instance != null)
            AudioManager.instance.ReiniciarMusica();
    }

    // Volver al menú principal
    public void VolverMenu()
    {
        ResetEstado();
        SceneManager.LoadScene("Menu Principal");
    }

    // Pasar al siguiente nivel
    public void SiguienteNivel()
    {
        ResetEstado();
        int siguiente = SceneManager.GetActiveScene().buildIndex + 1;

        if (siguiente < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(siguiente);
        else
            SceneManager.LoadScene("Menu Principal"); // Si no hay más niveles, volver al menú,evitamos errores 
    }

    // Limpieza general de estado, para evitar problemas al cambiar de nivel o reiniciar
    private void ResetEstado()
    {
        GameManager.instance.ReiniciarPuntos();
        Timer.instance.ReiniciarTimer();
        Time.timeScale = 1f;

        juegoPausado = false;
        nivelFinalizado = false;
    }
    public void ElegirNivel()
    {
        ResetEstado();
        SceneManager.LoadScene("Niveles"); // Escena donde se elige el nivel
    }
}

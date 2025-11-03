using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Este script controla la puntuación general del juego
    public static GameManager instance;  // Singleton (sirve para que desde cualquier script podamos llamar a esta clase)

    // Lleva la suma total de gemas/puntos obtenidos
    public int PuntosTotales;

    // Evento que avisa a la UI cuando cambian los puntos
    public delegate void PuntosCambiados(int puntos);
    public event PuntosCambiados OnPuntosCambiados;

    private void Awake()
    {
        // Activamos Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // No se destruye al cargar otra escena
        }
        else
        {
            Destroy(gameObject); // Si ya existe uno, este sobra, destruimos duplicado, para evitar errores
        }
    }

    private void Start()
    {
        // Avisamos a la UI del valor inicial
        OnPuntosCambiados?.Invoke(PuntosTotales);
    }

    // sumar puntos desde cualquier parte del juego
    public void SumarPuntos(int cantidad)
    {
        PuntosTotales += cantidad;
        OnPuntosCambiados?.Invoke(PuntosTotales); // avisamos a la UI del cambio
    }

    // Dejar los puntos a cero (por ejemplo al reiniciar un nivel)
    public void ReiniciarPuntos()
    {
        PuntosTotales = 0;
        OnPuntosCambiados?.Invoke(PuntosTotales);
    }
}

using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public static Timer instance;

    // texto en pantalla donde se mostrará el tiempo
    public TMP_Text timerText;

    // tiempo que llevamos
    private float tiempo;

    // si esta contando o no
    private bool contando = true;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); return; }

        // Inicializar valores
        tiempo = 0f;
        contando = true;
        Time.timeScale = 1f; 
    }

    private void Update()
    {
        // Si el timer está detenido, no se actualiza
        if (!contando) return;

        // Sumar el tiempo que ha pasado desde el último frame
        tiempo += Time.deltaTime;

        // Actualizar el texto en pantalla
        ActualizarTexto();
    }

    // Actualiza el ui del cronómetro según el tiempo
    public void ActualizarTexto()
    {
        if (timerText == null) return; 
        int segundosTotales = Mathf.FloorToInt(tiempo);
        // Si es menos de un minuto, mostrar en segundos
        if (segundosTotales < 60)
            timerText.text = "Tiempo: " + segundosTotales + "s";
        else
            // Mostrar formato Minutos: Segundos con dos dígitos en segundos
            timerText.text = "Tiempo: " + (segundosTotales / 60) + ":" + (segundosTotales % 60).ToString("00");
    }

    // detiene el temporizador
    public void PararTimer() => contando = false;
    // reinicia el tiempo y vuelve a contar
    public void ReiniciarTimer()
    {
        tiempo = 0f;
        contando = true;
        Time.timeScale = 1f;
        ActualizarTexto();
    }

    // Permite asignar otro TMP_Text 
    public void SetTextTMP(TMP_Text nuevoTexto)
    {
        timerText = nuevoTexto;
        ActualizarTexto();
    }

    // permite a otros scripts obtener el tiempo actual
    public float ObtenerTiempo()
    {
        return tiempo;
    }
}

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Música")]
    public AudioSource background;

    [Header("Efectos")]
    public AudioSource caida, moneda, paso1, paso2, salto, death, victory, trampolin;

    //variables para guardar y cargar ajustes de volumen
    private const string MUSIC_KEY = "MusicVolume";
    private const string FX_KEY = "FXVolume";
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
            return;
        }

        // recupera los volúmenes guardados o pone valor por defecto (1)
        float musicVol = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float fxVol = PlayerPrefs.GetFloat(FX_KEY, 1f);

        if (background != null)
        {
            background.volume = musicVol;
            if (!background.isPlaying && background.clip != null)
            {
                background.loop = true;
                background.Play();
            }
        }
        // aplica el volumen de efectos a cada efecto de sonido
        AudioSource[] efectos = { caida, moneda, paso1, paso2, salto, death, victory, trampolin };
        foreach (AudioSource e in efectos)
            if (e != null) e.volume = fxVol;
    }


    // Método para reproducir un sonido 
    private void Reproducir(AudioSource audio)
    {
        if (audio != null && audio.clip != null)
            audio.PlayOneShot(audio.clip, audio.volume);
    }

    // método para que otros scripts reproduzcan efectos 
    public void PlayCaida() => Reproducir(caida);
    public void PlayMoneda() => Reproducir(moneda);
    public void PlayPaso1() => Reproducir(paso1);
    public void PlayPaso2() => Reproducir(paso2);
    public void PlaySalto() => Reproducir(salto);
    public void PlayTrampolin() => Reproducir(trampolin);

    // cuando jugador muere, paramos la música y reproducimos el sonido de muerte
    public void PlayDeath()
    {
        if (background != null) background.Stop();
        Reproducir(death);
    }

    // Al ganar lo mismo pero ponemos sonido de victoria
    public void PlayVictory()
    {
        if (background != null) background.Stop();
        Reproducir(victory);
    }

    // método para volver a activar la música si no está sonando
    public void ReiniciarMusica()
    {
        if (background != null && background.clip != null && !background.isPlaying)
        {
            background.loop = true;
            background.Play();
        }
    }
}

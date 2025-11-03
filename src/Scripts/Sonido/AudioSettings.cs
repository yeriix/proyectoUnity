using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    // Sliders que pondremos para ajustar los volúmenes
    public Slider sliderMusic;
    public Slider sliderFX;

    // claves para guardar los valores 
    private const string MUSIC_KEY = "MusicVolume";
    private const string FX_KEY = "FXVolume";

    private void OnEnable()
    {
        // cargar los valores guardados previamente o valor por defecto (1)
        float savedMusic = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float savedFX = PlayerPrefs.GetFloat(FX_KEY, 1f);

        // asignar valores a los sliders 
        sliderMusic.SetValueWithoutNotify(savedMusic);
        sliderFX.SetValueWithoutNotify(savedFX);

        // aplicar volúmenes
        ApplyMusicVolume(savedMusic);
        ApplyFXVolume(savedFX);

        // añadir listeners para detectar cambios cuando el jugador mueva los sliders
        sliderMusic.onValueChanged.AddListener(OnMusicChanged);
        sliderFX.onValueChanged.AddListener(OnFXChanged);
    }

    private void OnMusicChanged(float value)
    {
        // cada vez que cambia el slider de música, se aplica y se guarda
        ApplyMusicVolume(value);
        PlayerPrefs.SetFloat(MUSIC_KEY, value);
        PlayerPrefs.Save();
    }

    private void OnFXChanged(float value)
    {
        // cada vez que cambia el slider de efectos, se aplica y se guarda
        ApplyFXVolume(value);
        PlayerPrefs.SetFloat(FX_KEY, value);
        PlayerPrefs.Save();
    }

    private void ApplyMusicVolume(float value)
    {
        // aplicar volumen al sonido de fondo, background
        if (AudioManager.instance != null && AudioManager.instance.background != null)
            AudioManager.instance.background.volume = value;
    }

    private void ApplyFXVolume(float value)
    {
        if (AudioManager.instance == null) return; 
        // array para actualizar todos los volúmenes
        AudioSource[] efectos = {
            AudioManager.instance.caida,
            AudioManager.instance.moneda,
            AudioManager.instance.paso1,
            AudioManager.instance.paso2,
            AudioManager.instance.salto,
            AudioManager.instance.death,
            AudioManager.instance.victory,
            AudioManager.instance.trampolin
        };

        // recorrer cada efecto y aplicar volumen 
        foreach (AudioSource efecto in efectos)
            if (efecto != null)
                efecto.volume = value;
    }
}

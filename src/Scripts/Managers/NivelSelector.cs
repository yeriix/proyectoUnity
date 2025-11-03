using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

public class NivelesSystem : MonoBehaviour
{
    // Prefab del botón de nivel 
    public GameObject nivelButtonPrefab;

    // Contenedor donde aparecerán los botones 
    public Transform buttonContainer;

    // Número total de niveles que se van a generar automáticamente (que luego saldran)
    public int totalNiveles = 2;

    void Start()
    {
        GenerateNivelButtons(); 
    }

    // Método que crea los botones
    void GenerateNivelButtons()
    {
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }
        // Bucle que generará un botón por cada nivel
        for (int i = 1; i <= totalNiveles; i++)
        {
            // instancia un nuevo botón en el contenedor de UI
            GameObject button = Instantiate(nivelButtonPrefab, buttonContainer);

            // cambia el texto del botón para mostrar el número del nivel
            button.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();

            int nivelIndex = i; // Se guarda i en una variable temporal para evitar errores 

            // Se añade un evento al botón: Cuando el usuario haga clic se carga la escena correspondiente
            button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                SceneManager.LoadScene("Nivel_" + nivelIndex);
                 if (AudioManager.instance != null)
                    AudioManager.instance.ReiniciarMusica();
            });
        }
    }
}

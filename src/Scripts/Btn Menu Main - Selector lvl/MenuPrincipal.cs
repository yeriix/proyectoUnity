using UnityEngine;
using UnityEngine.SceneManagement; 

public class MenuPrincipal : MonoBehaviour
{
    // Método que se ejecuta cuando mi jugador hace clic en "Seleccionar Niveles"
    public void IrSelectorNiveles()
    {
        SceneManager.LoadScene("Niveles"); // vamos a la escena "Niveles"
    }

    // Método para salir del juego
    public void SalirJuego()
    {
        Application.Quit(); // Cierra el juego
    }

    // Método para volver al menú principal desde otras escenas
    public void VolverMenuPrincipal()
    {
        SceneManager.LoadScene("Menu Principal"); // vamos al menú principal (por si hay que volver a él)
    }
}

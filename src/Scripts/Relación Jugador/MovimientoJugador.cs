using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovimientoJugador : MonoBehaviour
{
    // Componentes del jugador
    public Rigidbody2D rb;                  // Física del jugador
    public Animator animator;               // Controla animaciones
    public MovimientoCamara camara;         // Para detener seguimiento si muere
    public Collider2D colliderPlayer;       // Para desactivar colisiones al morir

    public UIManager uiManager;          // UI Manager para mostrar los diferentes menus

    private bool paso = true;         // Para alternar paso1 y paso2 (sonido)
    private float pasoTimer = 0f;     // Temporizador para reproducir pasos (sonido)
    public float pasoIntervalo = 0.4f; // Cada cuánto tiempo suena un paso (sonido)

    private bool estabaEnAire = false; // Para detectar si estaba cayendo


    // Movimiento general
    public float velocidad = 7f;
    public float fuerzaSalto = 8f;

    // upgrade cuando se salta
    public float gravedadCaida = 2.5f;      // caída más rápida
    public float gravedadBajoSalto = 4f;    // si suelta el botón, cae antes

    // Detección del suelo
    public Transform detectorSuelo;
    public Vector2 tamanoSuelo = new Vector2(0.28f, 0.05f);
    public LayerMask capaSuelo;

    // Detección de paredes para arreglar bug "wall slide"
    public float distanciaPared = 0.35f;

    // Control interno
    private float movimientoX;
    private bool mirandoDerecha = true;
    private bool botonSaltoPresionado;
    private bool muerto = false;

    private void Update()
    {
        if (muerto) return; // Si está muerto, no se mueve ni anima

        // animaciones en función de velocidad
        animator.SetFloat("magnitude", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("yVelocity", rb.velocity.y);

        // volteo automático según dirección, si se ve derecha pues el personaje mira a la derecha
        if (movimientoX > 0 && !mirandoDerecha) Girar();
        else if (movimientoX < 0 && mirandoDerecha) Girar();
    }

    private void FixedUpdate()
    {
        // si el personaje está muerto, no procesamos nada más
        if (muerto) return;

        // comprobaciones del estado del personaje
        bool enSuelo = EstaEnSuelo();
        bool tocandoDerecha = IsTouchingWall(1);  // toca una pared a la derecha
        bool tocandoIzquierda = IsTouchingWall(-1); // toca una pared a la izquierda

        // Movimiento horizontal aplicando velocidad suave
        rb.velocity = new Vector2(movimientoX * velocidad, rb.velocity.y);

        // si no estamos en suelo y estamos contra una pared, limitamos la velocidad de caída para simular un "deslizamiento por la pared" y no quedarnos pegados
        if (!enSuelo && ((tocandoDerecha && movimientoX > 0) || (tocandoIzquierda && movimientoX < 0)))
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -2f));
        }

        // ajuste de gravedad para mejorar el control del salto, si cae va mas rapido mientras mas cae y si salta y suelta pues el personaje baja antes
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (gravedadCaida - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !botonSaltoPresionado)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (gravedadBajoSalto - 1) * Time.fixedDeltaTime;
        }

        // reproduce sonido de pasos solo cuando está en el suelo y moviéndose
        if (enSuelo && Mathf.Abs(movimientoX) > 0.1f)
        {
            pasoTimer += Time.fixedDeltaTime;
            if (pasoTimer >= pasoIntervalo)
            {
                ReproducirPaso();
                pasoTimer = 0f;
            }
        }

        // detección de transición de aire a suelo: nos sirve para saber cuando empieza a caer y cuando vuelve a tocar el suelo para reproducir sonido de caída
        if (!estabaEnAire && !enSuelo)
        {
            estabaEnAire = true; // Ha dejado el suelo
        }
        else if (estabaEnAire && enSuelo)
        {
            AudioManager.instance.PlayCaida(); // aterriza
            estabaEnAire = false;
        }
    }


    // Input Movimiento (Input System, para moverse) lee el valor del eje horizontal y lo guarda en movimientoX.
    public void movimiento(InputAction.CallbackContext context)
    {
        movimientoX = context.ReadValue<Vector2>().x;
    }

    // Salto con salto corto
    public void salto(InputAction.CallbackContext context)
    {
        if (context.started && EstaEnSuelo())
        {
            botonSaltoPresionado = true;
            rb.velocity = new Vector2(rb.velocity.x, 0f); 
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            animator.SetTrigger("jump");
            AudioManager.instance.PlaySalto();
        }

        if (context.canceled)
            botonSaltoPresionado = false;
    }

    // detectar suelo 
    private bool EstaEnSuelo()
    {
        return Physics2D.OverlapBox(detectorSuelo.position, tamanoSuelo, 0f, capaSuelo);
    }

    // detectar pared 
    private bool IsTouchingWall(int dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * dir, distanciaPared, capaSuelo);
        return hit.collider != null;
    }

    // dar la vuelta al personaje a los lados (no afecta al movimiento, solo a lo visual)
    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1; // Invertimos horizontalmente
        transform.localScale = escala;
    }

    // muerte del jugador
    public void Die()
    {
        if (muerto) return;
        muerto = true;

        // reseteamos movimiento y colisiones
        rb.velocity = Vector2.zero;
        colliderPlayer.enabled = false;
        animator.SetTrigger("die");

        // saltito cuando muere
        rb.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);

        // la cámara deja de seguirlo
        if (camara != null) camara.seguirJugador = false;

        AudioManager.instance.PlayDeath();
        Timer.instance.PararTimer();

        // mostrar menu muerte
        StartCoroutine(MostrarMenuTrasMuerte());
    }

    private IEnumerator MostrarMenuTrasMuerte()
    {
        yield return new WaitForSecondsRealtime(2f);
        UIManager.instance.MostrarMuerte(); 
    }
    private void ReproducirPaso()
    {
        if(paso) AudioManager.instance.PlayPaso1();
        else AudioManager.instance.PlayPaso2();

        paso = !paso;
    }
}

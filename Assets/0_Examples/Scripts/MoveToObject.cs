using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class MoveToObject : MonoBehaviour
{
    /// <summary>
    /// Lista de objetos a seguir
    /// </summary>
    public Transform[] objectToFollowList;
    /// <summary>
    /// Velocidad de movimiento por segundo
    /// </summary>
    public float moveSpeed = 3f;
    /// <summary>
    /// Distancia minima para detenerse
    /// </summary>
    public float threshold = 0.5f;
    /// <summary>
    /// Bool que identifica que el objeto esta o debe moverse
    /// </summary>
    public bool isMoving;
    /// <summary>
    /// Bool para ver si esta caminando o corriendo
    /// </summary>
    public bool isWalking;

    private Transform m_MyTransform;
    private Animator m_MyAnimator;
    /// <summary>
    /// Indice del actual objeto a seguir
    /// </summary>
    private int m_CurrentObjectToFollowIndex;

	void Start ()
    {
        m_MyTransform = GetComponent<Transform>();
        m_MyAnimator = GetComponent<Animator>();

        if (objectToFollowList.Length == 0)
        {
            Debug.LogError("I need least one objectToFollow in my inspector.");
        }
	}
	
	void Update ()
    {
        // Verificamos que tengamos objetos en la lista y podemos movernos
        if (objectToFollowList.Length > 0 && isMoving)
        {
            // Calculamos el Vector3 direccion con modulo 1 (normalizado) hacia el objetivo, visto desde myTransform
            var directionToObject = (objectToFollowList[m_CurrentObjectToFollowIndex].position - m_MyTransform.position).normalized;
            // Hacemos que el Vector3 blue z local sea el Vector3 direction pero con el eje 'y' 0 para que mire al objetivo ignorando su posicion em 'y'
            m_MyTransform.forward = new Vector3(directionToObject.x, 0, directionToObject.z);

            // Calculamos la distancia entre el objetivo y myTransform, si esa distancia aun no es menor que el threshold (distancia minima para detenernos), tenemos que seguir acercandonos al objetivo
            if (Vector3.Distance(objectToFollowList[m_CurrentObjectToFollowIndex].position, m_MyTransform.position) > threshold)
            {
                // Movemos myTransform con Translate (perpectiva local), enviandole el Vector3 forward global como direccion, ya que ya tenemos el forward de myTransform apuntando hacia el objetivo
                // Ademas lo multiplicamos por moveSpeed (velocidad de movimiento) y Time.deltaTime para que el movimiento sea a metros/segundo, la velocidad es la mitad si esta caminando
                m_MyTransform.Translate(Vector3.forward * Time.deltaTime * (isWalking ? (moveSpeed * 0.5f) : moveSpeed));

                // Ponemos el valor del parametro 'MoveSpeed' del Animator a 0.5f o 1f depende de si esta caminando o corriendo (isWalking == false)
                m_MyAnimator.SetFloat("MoveSpeed", isWalking ? 0.5f : 1f);
            }
            else
            {
                // Si ya estamos muy cerca y podemos detenernos, ponemos el parametro del Animator a 0f para actualizar la animacion a Idle
                m_MyAnimator.SetFloat("MoveSpeed", 0f);

                // Aumentamos el indice para seguir al siguiente objeto
                m_CurrentObjectToFollowIndex++;

                // TO DO: Verificar el tamano de la lista y reiniciar a el indice actual:m_CurrentObjectToFollowIndex a 0
            }
        }
        else
        {
            // Si ya estamos muy cerca y podemos detenernos, ponemos el parametro del Animator a 0f para actualizar la animacion a Idle
            m_MyAnimator.SetFloat("MoveSpeed", 0f);
        }
	}
}

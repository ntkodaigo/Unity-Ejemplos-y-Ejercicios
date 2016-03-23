using UnityEngine;
using System.Collections;

public class ThirdPersonCharacter_Test : MonoBehaviour
{
    private Transform m_MyTransform;
    private Vector3 m_FromPivotToMouse;
    private Camera m_MainCam;

    void Start()
    {
        m_MyTransform = GetComponent<Transform>();
        m_MainCam = Camera.main;
    }

    void Update()
    {
        // Obtenemos el eje z del Vector3 calculado de myTransform (en el Mundo) a perpectiva de la Pantalla, como referencia para la posicion del mouse
        var distanceToScreen = m_MainCam.WorldToScreenPoint(m_MyTransform.position).z;
        // Calculamos la posicion del mouse de la pantalla, a perspectiva del mundo, usando el z en la pantalla de myTransfom
        var mouseInWorld = m_MainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen));

        // Calculamos el Vector3 direccion con modulo 1 (normalizado) hacia el mouse, visto desde myTranform
        var directionToMouse = (mouseInWorld - m_MyTransform.position).normalized;
        // Multiplicamos cada eje del Vector3 direccion hacia el mouse, con en nuevo Vector3 (1, 0, 1) para anular su eje 'y'
        directionToMouse = Vector3.Scale(directionToMouse, new Vector3(1,0,1));
        // Hacemos que el Vector3 blue z local sea el Vector3 direction pero con el eje 'y' 0 para que mire al objetivo ignorando su posicion en 'y'
        m_MyTransform.forward = directionToMouse;

        // Obtenemos los ejes Horizontal y Vertical y creamos un nuevo vector con modulo 1 (normalizado) como Vector3 de direccion
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        var dir = new Vector3(h, 0, v).normalized;

        // Transladamos myTransform, enviandolo la direccion creado con los ejes Horizontal y Vectical, a una velocidad constante de 5 metros / segundo
        // Translate, enviandole un solo parametro usa una perspectiva local para trasladar, por eso esta bien enviar el Vector3 dir (global) 
        m_MyTransform.Translate(dir * Time.deltaTime * 5f);
    }

    
    void OnMouseDown()
    {
        // Obtenemos el eje z del Vector3 calculado de myTransform (en el Mundo) a perpectiva de la Pantalla, como referencia para la posicion del mouse
        var distanceToScreen = m_MainCam.WorldToScreenPoint(m_MyTransform.position).z;

        // Obtenemos el Vector3 desde el Pivot (origen de la posicion) hacia el punto de toque del mouse y lo guardamos
        m_FromPivotToMouse = m_MainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen)) - m_MyTransform.position;
    }

    void OnMouseDrag()
    {
        // Obtenemos el eje z del Vector3 calculado de myTransform (en el Mundo) a perpectiva de la Pantalla, como referencia para la posicion del mouse
        var distanceToScreen = m_MainCam.WorldToScreenPoint(m_MyTransform.position).z;
        // Obtenemos el Vector3 calculado desde la posicion del mouse con referencia al eje z de myTransform en perspectiva en pantalla, restado por Vector3 calculado anteriormente para obtener la posicion deseada
        // fromPivotToMouse = mouseInWorld - myTransform.position -> myTransform.position = mouseInWorld - fromPivotToMouse
        var mouseInWorld = m_MainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen)) - m_FromPivotToMouse;

        m_MyTransform.position = mouseInWorld;
    }
}

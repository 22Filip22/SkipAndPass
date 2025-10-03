using UnityEngine;
using UnityEngine.Events;

public class ColiderEntered : MonoBehaviour
{
    [Header("Event triggered when something enters the collider")]
    public UnityEvent onTriggerEnterEvent;

    [Header("Event triggered when something leaves the collider")]
    public UnityEvent onTriggerLeaveEvent;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        onTriggerEnterEvent?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onTriggerLeaveEvent?.Invoke();
    }

}

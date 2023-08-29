using Event;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class TutorialMoveTarget : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            EventManager.TriggerEvent(EventKeyword.NextTutorial);
        }
    }
}

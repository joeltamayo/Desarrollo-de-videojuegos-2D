using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Llama al método del script del jugador para recolectar la moneda
            NewBehaviourScript playerScript = other.GetComponent<NewBehaviourScript>();
            if (playerScript != null)
            {
                playerScript.OnCoinCollected(1); // Supone que cada moneda vale 1
            }

            // Destruye la moneda
            Destroy(gameObject);
        }
    }
}
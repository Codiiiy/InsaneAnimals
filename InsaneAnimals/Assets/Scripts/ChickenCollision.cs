using UnityEngine;

public class ChickenCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Seed"))
        {
            Game_Manager.instance.AddPoints();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Obstacle"))
        {
            HandleObstacleCollision();
        }
        else if (other.CompareTag("PowerUp_DoublePoints"))
        {
            Game_Manager.instance.ActivateDoublePoints();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("PowerUp_SlowTime"))
        {
            Game_Manager.instance.ActivateSlowTime();
            Destroy(other.gameObject);
        }
    }

    private void HandleObstacleCollision()
    {
        Game_Manager.instance.DeductPoints();
    }
}

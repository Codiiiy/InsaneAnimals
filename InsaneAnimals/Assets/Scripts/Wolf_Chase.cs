using UnityEngine;

public class Wolf_Chase : MonoBehaviour
{
    public Chicken_movement target;

    public float followDistance = 2f;
    public float followSpeed = 5f;
    public float catchUpDistance = 0.5f;
    private bool catchingUp = false;


    void Update()
    {
        if (target == null) return;

        if (target.isPlaying == true)
        {
            Vector2 targetPosition = (Vector2)target.transform.position - (Vector2)target.transform.right * (catchingUp ? catchUpDistance : followDistance);
            transform.position = new Vector3(target.transform.position.x,
                                                    Mathf.MoveTowards(transform.position.y, target.transform.position.y, followSpeed * Time.deltaTime),
                                                    transform.position.z);
        }

        
    }

    public void OnHit()
    {
        catchingUp = true;
    }


}

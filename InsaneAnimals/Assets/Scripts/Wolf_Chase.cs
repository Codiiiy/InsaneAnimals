using UnityEngine;


public class Wolf_Chase : MonoBehaviour
{
    public Chicken_movement target;

    public float followDistance = 2f;
    public float followSpeed = 5f;
    public float catchUpDistance = 0.5f;
    private bool catchingUp = false;
    private bool noLives = false;
    [SerializeField] private AudioClip growlClip;
    [SerializeField] private AudioClip biteClip;
    [SerializeField] private AudioClip howlClip;



    void Update()
    {
        if (target == null) return;

        if (noLives)
        {
            transform.position = new Vector3(
                target.transform.position.x,
                Mathf.MoveTowards(transform.position.y, target.transform.position.y, followSpeed * Time.deltaTime),
                transform.position.z
            );
            SoundFXManager.instance.PlaySoundFXClip(biteClip, transform, 1f);
            gameObject.layer = 10;
            
        }
        else if (catchingUp)
        {

            float targetX = target.transform.position.y - catchUpDistance;
            transform.position = new Vector3(
                target.transform.position.x,
                Mathf.MoveTowards(transform.position.y, targetX, followSpeed * Time.deltaTime),
                transform.position.z
            );
            SoundFXManager.instance.PlaySoundFXClip(growlClip, transform, 1f);

            if (transform.position.y != targetX)
            {
                followSpeed += 1;
            }
        }
        else if (target.isPlaying)
        {
            float targetX = target.transform.position.y - followDistance;
            transform.position = new Vector3(
                Mathf.MoveTowards(transform.position.x, target.transform.position.x, followSpeed * Time.deltaTime),
                Mathf.MoveTowards(transform.position.y, targetX, followSpeed * Time.deltaTime),
                transform.position.z
            );
            SoundFXManager.instance.PlaySoundFXClip(howlClip, transform, 1f);
        }
    }

    public void OnHit()
    {
        if(catchingUp == false)
        {
            catchingUp = true;
        }
        else
        {
            noLives = true;
        }
        
    }

    public void TriggerNoLives()
    {
        noLives = true;
    }

    public void LifeUp()
    {
        if (catchingUp)
        {
            catchingUp = false;
        }
    }
}

using UnityEngine;

public class FollowScript : MonoBehaviour
{
    public float speed = 1f;
    private Transform target;
    public float followDistance = 1.5f;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        // Calculate the distance to the target
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // Only move towards the target if the distance is greater than followDistance
        if (distanceToTarget > followDistance)
        {
            // Calculate the direction towards the target
            Vector3 direction = (target.position - transform.position).normalized;

            // Rotate the zombie to face the target
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            // Move towards the target
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
}

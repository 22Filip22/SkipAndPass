using UnityEngine;

public class CarMover : MonoBehaviour
{
    [SerializeField] float speed = 2f;         
    [SerializeField] Transform carsStart;   
    [SerializeField] Transform carsEnd ;   
    [SerializeField] float bobAmplitude = 0.2f; 
    [SerializeField] float bobFrequency = 2f;   

    private Vector3[] startPositions;

    private void Start()
    {
        // cache the initial positions of all children
        int count = transform.childCount;
        startPositions = new Vector3[count];

        for (int i = 0; i < count; i++)
        {
            startPositions[i] = transform.GetChild(i).position;
        }
    }

    private void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            // move right
            child.position += Vector3.right * speed * Time.deltaTime;

            // sine-based up & down movement relative to start position
            float offsetY = Mathf.Sin(Time.time * bobFrequency + child.GetInstanceID()) * bobAmplitude;
            child.position = new Vector3(child.position.x, startPositions[i].y + offsetY, child.position.z);

            // wrap around when passing right limit
            if (child.position.x > carsEnd.position.x)
            {
                child.position = new Vector3(carsStart.position.x, startPositions[i].y + offsetY, child.position.z);
            }
        }
    }
}

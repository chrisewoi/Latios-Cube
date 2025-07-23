using UnityEngine;

public class TrailRNG : MonoBehaviour
{
    public TrailRenderer trail;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        trail.startColor += new Color(Random.value, Random.value, Random.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

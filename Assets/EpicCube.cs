using System;
using UnityEngine;

public class EpicCube : MonoBehaviour
{

    public float battlefrontVersion;
    private Vector3 input;
    private Rigidbody rb;
    [SerializeField] private float speed;

    public Vector3 rotation;
    // Hi, I'm start. Everything that you want done first, well I'm your guy. In good time I think we'll get to know each other really well :)
    void Start()
    {
        battlefrontVersion = 3.0f;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        /*transform.position += transform.forward * Time.deltaTime * 5f;
        transform.rotation = Quaternion.Euler(rotation);
        rotation += Vector3.one * (50f * Time.deltaTime);*/

        input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
    }

    private void FixedUpdate()
    {
        rb.AddForce(input * speed);
    }
}

using UnityEngine;
using System.Collections;

public class Locomotion : MonoBehaviour {

    public float MoveSpeed;
    public float TurnSpeed;

    Rigidbody rb;
    Vector3 GroundNormal = Vector3.up;
    bool Grounded;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")).normalized;
        float TurnInput = Input.GetAxis("Mouse X");
        GroundedTest();

        float moveSpeed = MoveSpeed;
        if (Grounded)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed *= 2f;
            }
        }
        else
        {
            moveSpeed *= 0.5f;
        }

        transform.Rotate(transform.up, TurnInput * Time.fixedDeltaTime * TurnSpeed);
        transform.Translate(Vector3.ProjectOnPlane(input, GroundNormal) * moveSpeed * Time.fixedDeltaTime);
	}

    void GroundedTest()
    {
        Ray ray = new Ray(transform.position + (Vector3.up * 0.1f), -transform.up);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 0.2f))
        {
            GroundNormal = hit.normal;
            Grounded = true;
        }
        else
        {
            GroundNormal = Vector3.up;
            Grounded = false;
        }
    }
}

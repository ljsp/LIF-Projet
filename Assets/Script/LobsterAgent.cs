using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgentsExamples;
using Unity.MLAgents.Sensors;

public class LobsterAgent : Agent
{

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float raycastDistance;
    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);

    }

    public override void OnEpisodeBegin()
    {
        
    }

    private void Move()
    {
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");
        Vector3 movement = new UnityEngine.Vector3(hAxis, 0, vAxis) * speed;
        Vector3 newPosition = rb.position + rb.transform.TransformDirection(movement);
        rb.MovePosition(newPosition);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded())
            {
                rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, UnityEngine.Vector3.down, raycastDistance);
    }
}

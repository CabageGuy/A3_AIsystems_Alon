using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Basic movement (nothing fancy, just gets the job done)
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0, v);
        controller.Move(move * speed * Time.deltaTime);

        // Make noise when moving
        if (move.magnitude > 0.1f)
        {
            MakeNoise();
        }
    }

    void MakeNoise()
    {
        // Every step you take... the bat is watching (or at least listening)
        Collider[] hits = Physics.OverlapSphere(transform.position, 8f);

        foreach (Collider hit in hits)
        {
            BatAI bat = hit.GetComponent<BatAI>();
            if (bat != null)
            {
                bat.OnPlayerNoise(transform.position);
            }
        }
    }
}
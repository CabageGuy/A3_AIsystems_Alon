using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private bool isHeld = false;
    public Transform holdPoint;
    private Vector3 startPosition;
    private Quaternion startRotation;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isHeld)
            {
                PickUp();
            }
            else
            {
                Drop();
            }
        }
    }

    void PickUp()
    {
        isHeld = true;
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;

        MakeNoise();
    }

    void Drop()
    {
        isHeld = false;
        transform.SetParent(null);

        MakeNoise();
    }

    void MakeNoise()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 10f);

        foreach (Collider hit in hits)
        {
            BatAI bat = hit.GetComponent<BatAI>();
            if (bat != null)
            {
                bat.OnPlayerNoise(transform.position);
            }
        }
    }
    public void ForceDrop()
    {
        if (!isHeld) return;

        isHeld = false;

        transform.SetParent(null);

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(Vector3.forward * 3f, ForceMode.Impulse); // small toss effect
        }

        MakeNoise();
    }
    public void ResetItem()
    {
        isHeld = false;

        transform.SetParent(null);
        transform.position = startPosition;
        transform.rotation = startRotation;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }
    public bool IsHeld()
    {
        return isHeld;
    }
}
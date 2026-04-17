using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private bool isHeld = false;
    public Transform holdPoint;

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
}
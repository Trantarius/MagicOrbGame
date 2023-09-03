using System.Collections;
using UnityEngine;

public class ThiefBehavior : MonoBehaviour
{
    private bool isHoldingOrb = false;
    private GameObject heldOrb;
    private MeshRenderer mr;
    private bool dead;

    public float movementSpeed = 2f;
    public float movementSpeedWhenHoldingOrb = 3f;

    private void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        var tag = isHoldingOrb && heldOrb != null ? "Lava" : "WaterOrb";
        var target = FindNearest(tag);

        if (target != null)
        {
            var targetWidth = target.GetComponent<Renderer>().bounds.size.x;
            var targetPosition = target.transform.position - transform.position + new Vector3(targetWidth / 2, 0, 0);
            Vector3 moveDirection = (targetPosition).normalized;
            var speed = isHoldingOrb ? movementSpeedWhenHoldingOrb : movementSpeed;
            transform.Translate(moveDirection * speed * Time.deltaTime);

            if (Vector3.Distance(targetPosition, transform.position) < 1)
            {
                dropWaterOrb();
            }
        }
    }


    private GameObject FindNearest(string tag)
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);
        GameObject nearestTaggedObject = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject orb in taggedObjects)
        {
            float distance = Vector3.Distance(transform.position, orb.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTaggedObject = orb;
            }
        }

        return nearestTaggedObject;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (dead)
        {
            return;
        }
        if (!isHoldingOrb && collision.gameObject.CompareTag("WaterOrb"))
        {
            // Steal the water orb
            isHoldingOrb = true;
            heldOrb = collision.gameObject;

            // Make the water orb a child of the thief
            heldOrb.transform.parent = transform;

            // Disable the water orb's Rigidbody
            Rigidbody orbRb = heldOrb.GetComponent<Rigidbody>();
            if (orbRb != null)
            {
                orbRb.isKinematic = true;
            }
        }
    }

    private void toggleVisable()
    {
        mr.enabled = !mr.enabled;
    }

    private IEnumerator DestroyInSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    public void HitByMagicBall()
    {
        if (this.heldOrb)
        {
            dropWaterOrb();
        }

        dead = true;
        // Destroy the thief
        var mat = new UnityEngine.Material(mr.material);
        mat.SetColor("_BaseColor", Color.red);
        mr.material = mat;
        InvokeRepeating("toggleVisable", 0, 0.05f);

        StartCoroutine(DestroyInSeconds(0.5f));
    }

    private void dropWaterOrb()
    {
        this.heldOrb.transform.parent = null;
        Rigidbody orbRb = heldOrb.GetComponent<Rigidbody>();
        orbRb.isKinematic = false;
    }
}
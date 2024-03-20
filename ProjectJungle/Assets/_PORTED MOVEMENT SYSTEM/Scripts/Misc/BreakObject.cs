using System;
using Unity.Mathematics;
using UnityEngine;

public class BreakObject : MonoBehaviour
{
    [SerializeField] private GameObject brokenPrefab;

    private MeshRenderer meshRenderer;
    private BoxCollider collider;
    private Rigidbody rb;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        collider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.transform.transform.CompareTag("NonDesctuctionInteraction"))
        {
            var fracture = Instantiate(brokenPrefab, transform.position, quaternion.identity);
            Destroy(fracture, 15f);

            meshRenderer.enabled = false;
            collider.enabled = false;
            Destroy(rb);
        }
    }
}
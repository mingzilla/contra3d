using Types;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    private Color originalColor;
    private Material itemMaterial;

    private void Awake()
    {
        itemMaterial = GetComponent<MeshRenderer>().material;
        originalColor = itemMaterial.color;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag(GameTag.PLAYER.name)) itemMaterial.color = Color.red;
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag(GameTag.PLAYER.name)) itemMaterial.color = originalColor;
    }
}
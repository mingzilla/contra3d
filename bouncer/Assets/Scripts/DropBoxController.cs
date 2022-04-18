using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBoxController : MonoBehaviour
{
    public float dropInterval = 1f;
    public float dropCounter = 1f;

    public GameObject dropBox;

    private List<GameObject> previousDropBoxes = new List<GameObject>();

    void Update()
    {
        if (dropCounter < 0)
        {
            previousDropBoxes.ForEach(Destroy);
            previousDropBoxes.Add(SpawnBox());
            previousDropBoxes.Add(SpawnBox());
            previousDropBoxes.Add(SpawnBox());
            dropCounter = dropInterval;
        }
        dropCounter = dropCounter - Time.deltaTime;
    }

    GameObject SpawnBox()
    {
        int x = Random.Range(0, 37) - 17;
        int z = Random.Range(0, 36) - 21;
        return Instantiate(dropBox, new Vector3(x, 8, z), Quaternion.identity);
    }
}
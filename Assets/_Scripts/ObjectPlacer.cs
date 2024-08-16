using System;
using UnityEngine;
using System.Collections.Generic;
public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameObject = new List<GameObject>();

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;
        placedGameObject.Add(newObject);
        return placedGameObject.Count - 1;
    }
}

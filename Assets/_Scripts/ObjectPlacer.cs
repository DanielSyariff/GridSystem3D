using System;
using UnityEngine;
using UnityEngine.AI;
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

        NavMeshAgent agent = newObject.GetComponentInChildren<NavMeshAgent>();
        Debug.Log("Agent : " + agent);
        if (agent != null)
        {
            agent.enabled = true;
        }
        CharMovement c = newObject.GetComponentInChildren<CharMovement>();
        Debug.Log("Char : " + c);
        if (c != null)
        {
            c.enabled = true;
        }

        return placedGameObject.Count - 1;
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObject.Count <= gameObjectIndex || placedGameObject[gameObjectIndex] == null)
        {
            return;
        }
        else
        {
            Destroy(placedGameObject[gameObjectIndex]);
            placedGameObject[gameObjectIndex] = null;
        }
    }
}

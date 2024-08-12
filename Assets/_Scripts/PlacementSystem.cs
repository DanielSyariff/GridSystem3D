using System;
using UnityEngine;
using System.Collections.Generic;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    GameObject mouseIndicator;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO database;
    private int selectedObjectIndex = -1; //Default

    [SerializeField]
    private GameObject gridVisualization;

    //Pisahin Data antara Floor dan Furniture, jadi dia bikin 2 Grid Data
    private GridData floorData, furnitureData;

    private List<GameObject> placedGameObject = new List<GameObject>();

    [SerializeField] private PreviewSystem previewSystem;

    private Vector3Int lastdetectedPosition = Vector3Int.zero;

    private void Start()
    {
        StopPlacement();

        floorData = new GridData();
        furnitureData = new GridData();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();

        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);

        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID Found{ID}");
            return;
        }
        //cellIndicator.SetActive(true);
        previewSystem.StartShowingPlacementPreview(database.objectsData[selectedObjectIndex].Prefab, database.objectsData[selectedObjectIndex].Size);
        gridVisualization.SetActive(true);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        if (!placementValidity)
        {
            return;
        }

        GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);

        placedGameObject.Add(newObject);

        //Yang Misahin Grid Data ketika Place Object, Floor or Furniture
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;

        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID, placedGameObject.Count - 1);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        //Yang Memvalidasi GridData pake Dictionary yg mana Floor/Furniture
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;

        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        //cellIndicator.SetActive(false);
        previewSystem.StopShowingPreview();
        gridVisualization.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;

        lastdetectedPosition = Vector3Int.zero;
    }

    private void Update()
    {
        if (selectedObjectIndex < 0)
        {
            return;
        }

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if (lastdetectedPosition != gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

            mouseIndicator.transform.position = mousePosition;

            previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
            lastdetectedPosition = gridPosition;
        }
    }
}

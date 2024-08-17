using System;
using UnityEngine;
using System.Collections.Generic;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO database;

    [SerializeField]
    private GameObject gridVisualization;

    //Pisahin Data antara Floor dan Furniture, jadi dia bikin 2 Grid Data
    private GridData floorData, furnitureData;

    [SerializeField] private PreviewSystem previewSystem;

    private Vector3Int lastdetectedPosition = Vector3Int.zero;

    [SerializeField]
    private ObjectPlacer objectPlacer;

    IBuildingState buildingState;

    [SerializeField]
    private SoundFeedback soundFeedback;

    private void Start()
    {
        StopPlacement();

        floorData = new GridData();
        furnitureData = new GridData();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridVisualization.SetActive(true);

        buildingState = new PlacementState(ID,
                                           grid,
                                           previewSystem,
                                           database,
                                           floorData,
                                           furnitureData,
                                           objectPlacer, 
                                           soundFeedback);
        
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }
    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new RemovingState(grid, previewSystem, floorData, furnitureData, objectPlacer, soundFeedback);

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

        buildingState.OnAction(gridPosition);
    }

    //private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    //{
    //    //Yang Memvalidasi GridData pake Dictionary yg mana Floor/Furniture
    //    GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;

    //    return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    //}

    private void StopPlacement()
    {
        if (buildingState == null)
        {
            return;
        }
        gridVisualization.SetActive(false);
        buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;

        lastdetectedPosition = Vector3Int.zero;
    }

    private void Update()
    {
        if (buildingState == null)
        {
            return;
        }

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if (lastdetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastdetectedPosition = gridPosition;
        }
    }
}

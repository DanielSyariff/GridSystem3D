using System;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    Grid grid;
    PreviewSystem previewSystem;
    GridData floorData;
    GridData furnitureData;
    ObjectPlacer objectPlacer;
    SoundFeedback soundFeedback;

    public RemovingState(Grid grid,
                         PreviewSystem previewSystem,
                         GridData floorData,
                         GridData furnitureData,
                         ObjectPlacer objectPlacer,
                         SoundFeedback soundFeedback)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;
        this.soundFeedback = soundFeedback;

        previewSystem.StartShowingRemovePreview();
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;
        if (furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
        {
            selectedData = furnitureData;
        }
        else if(floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
        {
            selectedData = floorData;
        }

        if (selectedData == null)
        {
            //Nothing to remove
            soundFeedback.PlaySFX(SoundType.wrongPlacement);
        }
        else
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
            if (gameObjectIndex == -1)
            {
                soundFeedback.PlaySFX(SoundType.wrongPlacement);
                return;
            }
            else
            {
                soundFeedback.PlaySFX(SoundType.remove);
                selectedData.RemoveObjectAt(gridPosition);
                objectPlacer.RemoveObjectAt(gameObjectIndex);
            }
            Vector3 cellPosition = grid.CellToWorld(gridPosition);
            previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
        }
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return !(furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) && floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one));
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = CheckIfSelectionIsValid(gridPosition);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
    }
}

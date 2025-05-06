using UnityEngine;

namespace CubeBuilder.GridSystem
{
    public class GridFace : MonoBehaviour
    {
        private Cell[,] _grid;
        private float _cellSizeX;
        private float _cellSizeZ;
        private int _gridWidth;
        private int _gridHeight;

        public void GenerateGrid(int width, int height)
        {
            _gridWidth = width;
            _gridHeight = height;
            _grid = new Cell[width, height];

            // Масштаб грани в мировых координатах
            var lossyScale = transform.lossyScale;
            var faceSizeX = lossyScale.x;
            var faceSizeZ = lossyScale.z;
            _cellSizeX = faceSizeX / width;
            _cellSizeZ = faceSizeZ / height;

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    Vector3 localPos = new Vector3(
                        (x + 0.5f) * _cellSizeX - faceSizeX / 2,
                        0,
                        (z + 0.5f) * _cellSizeZ - faceSizeZ / 2
                    );
                    _grid[x, z] = new Cell(localPos);
                }
            }
        }

        public void PlaceBuildingAtPoint(Vector3 localPoint, GameObject buildingPrefab)
        {
            int gridX = Mathf.FloorToInt((localPoint.x + (transform.lossyScale.x / 2)) / _cellSizeX);
            int gridZ = Mathf.FloorToInt((localPoint.z + (transform.lossyScale.z / 2)) / _cellSizeZ);

            if (gridX >= 0 && gridX < _gridWidth && gridZ >= 0 && gridZ < _gridHeight)
            {
                PlaceBuilding(buildingPrefab, gridX, gridZ);
            }
        }

        private void PlaceBuilding(GameObject buildingPrefab, int gridX, int gridZ)
        {
            if (_grid[gridX, gridZ].IsOccupied) return;

            Vector3 localPos = _grid[gridX, gridZ].Position;
            Vector3 worldPos = transform.TransformPoint(localPos) + transform.up * 0.01f;
            Quaternion rotation = Quaternion.LookRotation(transform.forward, transform.up);

            GameObject building = Object.Instantiate(buildingPrefab, worldPos, rotation, transform);
            _grid[gridX, gridZ].IsOccupied = true;
            _grid[gridX, gridZ].Building = building;
        }

        private void OnDrawGizmos() {
            if (_grid == null) return;

            // Рисуем сетку
            Gizmos.color = Color.yellow;
            Gizmos.matrix = transform.localToWorldMatrix;

            // Рисуем ячейки
            for (int x = 0; x < _gridWidth; x++) {
                for (int z = 0; z < _gridHeight; z++) {
                    Vector3 cellPos = _grid[x, z].Position;
                    Gizmos.DrawWireCube(cellPos, new Vector3(_cellSizeX, 0.01f, _cellSizeZ));
                }
            }

            // Рисуем границы грани для отладки
            Gizmos.color = Color.red;
            float faceSizeX = _cellSizeX * _gridWidth;
            float faceSizeZ = _cellSizeZ * _gridHeight;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(faceSizeX, 0.01f, faceSizeZ));

            // Рисуем нормаль грани для проверки ориентации
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(Vector3.zero, Vector3.up * 0.2f);
        }
    }
}
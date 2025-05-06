using UnityEngine;


namespace CubeBuilder.GridSystem
{
    [System.Serializable]
    public class Cell {
        public Vector3 Position { get; private set; }
        public bool IsOccupied { get; set; }
        public GameObject Building { get; set; }

        public Cell(Vector3 position) {
            Position = position;
            IsOccupied = false;
            Building = null;
        }
    }
}
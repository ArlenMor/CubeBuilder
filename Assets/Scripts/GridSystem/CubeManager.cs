using UnityEngine;

namespace CubeBuilder.GridSystem
{
    public class CubeManager : MonoBehaviour
    {
        [SerializeField] private float _cubeSize = 1f;
        [SerializeField] private int _gridWidth = 3;
        [SerializeField] private int _gridHeight = 3;
        [SerializeField] private GridFace _facePrefab;
        [SerializeField] private GameObject _buildingPrefab;

        private GridFace[] _faces;

        private void Start()
        {
            SetupCube();
        }

        [ContextMenu("Setup Cube")]
        public void SetupCube()
        {
            foreach (Transform child in transform)
            {
                if (Application.isPlaying)
                {
                    Destroy(child.gameObject);
                }
                else
                {
                    DestroyImmediate(child.gameObject);
                }
            }

            //var faceScale = _cubeSize / 10f;

            // Создаём новые грани
            _faces = new GridFace[6];
            _faces[0] = CreateFace("Top", new Vector3(0, _cubeSize / 2, 0), Quaternion.Euler(0, 0, 0));
            _faces[1] = CreateFace("Bottom", new Vector3(0, -_cubeSize / 2, 0), Quaternion.Euler(0, 0, 180));
            _faces[2] = CreateFace("FrontLeft", new Vector3(0, 0, -_cubeSize / 2), Quaternion.Euler(-90, 0, 0));
            _faces[3] = CreateFace("FrontRight", new Vector3(_cubeSize / 2, 0, 0), Quaternion.Euler(-90, -90, 0));
            _faces[4] = CreateFace("BackLeft", new Vector3(-_cubeSize / 2, 0, 0), Quaternion.Euler(-90, 90, 0));
            _faces[5] = CreateFace("BackRight", new Vector3(0, 0, _cubeSize / 2), Quaternion.Euler(90, 0, 0));

            // Настраиваем сетку на каждой грани
            foreach (var face in _faces)
            {
                face.GenerateGrid(_gridWidth, _gridHeight);
                Debug.Log(
                    $"Face {face.name}: Position={face.transform.localPosition}, Rotation={face.transform.localRotation.eulerAngles}, Scale={face.transform.localScale}");
            }
        }

        private GridFace CreateFace(string name, Vector3 position, Quaternion rotation, float scale = 1)
        {
            var face = Instantiate(_facePrefab, transform);
            face.name = name;
            face.transform.localPosition = position;
            face.transform.localRotation = rotation;
            
            return face;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    GridFace face = hit.transform.GetComponent<GridFace>();
                    if (face != null)
                    {
                        Vector3 localHit = hit.transform.InverseTransformPoint(hit.point);
                        face.PlaceBuildingAtPoint(localHit, _buildingPrefab);
                    }
                }
            }
        }
    }
}
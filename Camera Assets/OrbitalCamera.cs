using UnityEngine;

public class OrbitalCamera : MonoBehaviour{

    #region Inspector Variables

        [Header("Target Settings")]
        [SerializeField, Tooltip("If a transform is not assigned, the camera will use this position as target, otherwise it will use the fixed target")] 
        private Transform target;
        [SerializeField] private Vector3 fixedTarget = new(0, 0, 0);

        [Header("Movement Settings")]
        [SerializeField] private float distance = 10f; 
        [SerializeField] private float minDistance = 5f;
        [SerializeField] private float maxDistance = 20f;
        [SerializeField] private float zoomSpeed = 2f;
        
        [Header("Rotation Settings")]
        [SerializeField] private float rotationSpeed = 100f;
        [SerializeField] private float yaw = 0f;
        [SerializeField] private float pitch = 0f;
        [SerializeField] private float minPitch = -30f;
        [SerializeField] private float maxPitch = 60f;
    
    #endregion

    #region Private variables

        private Vector3 targetPosition => target == null ? fixedTarget : target.position;

    #endregion

    #region Unity methods

        void Start(){
            Vector3 direction = transform.position - targetPosition;
            distance = direction.magnitude;
            yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            pitch = Mathf.Asin(direction.y / distance) * Mathf.Rad2Deg;

            UpdateCameraPosition();
        }

        void Update(){
            HandleRotation();
            HandleZoom();
            UpdateCameraPosition();
        }

    #endregion

    void HandleRotation(){
        if (Input.GetMouseButton(1)){
            yaw += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            pitch -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }
    }

    void HandleZoom(){
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }

    void UpdateCameraPosition(){
        float x = distance * Mathf.Sin(Mathf.Deg2Rad * yaw) * Mathf.Cos(Mathf.Deg2Rad * pitch);
        float y = distance * Mathf.Sin(Mathf.Deg2Rad * pitch);
        float z = distance * Mathf.Cos(Mathf.Deg2Rad * yaw) * Mathf.Cos(Mathf.Deg2Rad * pitch);

        Vector3 targetPosition = this.targetPosition;

        transform.position = targetPosition + new Vector3(x, y, z);
        transform.LookAt(targetPosition);
    }
}

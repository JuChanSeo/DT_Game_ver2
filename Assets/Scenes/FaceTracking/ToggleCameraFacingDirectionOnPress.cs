namespace UnityEngine.XR.ARFoundation.Samples
{
    public class ToggleCameraFacingDirectionOnPress : PressInputBase
    {
        ARTrackedImageManager a;
        [SerializeField]
        ARCameraManager m_CameraManager;

        

        public ARCameraManager cameraManager
        {
            get => m_CameraDirection.cameraManager;
            set => m_CameraDirection.cameraManager = value;
        }

        CameraDirection m_CameraDirection;

        protected override void Awake()
        {
            base.Awake();
            m_CameraManager.requestedFacingDirection = CameraFacingDirection.User;
            m_CameraDirection = new CameraDirection(m_CameraManager);
        }

        public void toggleCam()
        {
            Debug.Log("toggleCam Pressed");
            m_CameraDirection.Toggle();
        }

        protected override void OnPressBegan(Vector3 position)
        {
            m_CameraDirection.Toggle();
        }
    }
}

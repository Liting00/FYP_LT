using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private Transform cam;

        public static CameraManager Instance;

        private void Awake()
        {
            MoveCamera();
        }
        //move camera (offset)
        public void MoveCamera()
        {
            float camX = 9f;
            float camY = 12.5f + (1f - GameSettings.TILE_SIZE);
            cam.transform.rotation = Quaternion.Euler(90, -90, 90);
            cam.transform.position = new Vector3(camX, camY, 4.5f);
        }
    }
}
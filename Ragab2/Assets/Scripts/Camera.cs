using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ragab
{

    public class Camera : MonoBehaviour
    {
        [Header("Settings")]

        [SerializeField]
        Transform focusTarget;
        [SerializeField]
        float smoothCamera = 2;
        [SerializeField]
        float AimCameraMultiplier = 2;


        [Header("Radius")]

        [Header("Clamp")]

        [SerializeField]
        float clampLeft = -6;
        [SerializeField]
        float clampRight = 6;
        [SerializeField]
        float clampDown = 6;
        [SerializeField]
        float clampUp = 6;


        Vector3 velocity = Vector3.zero;

        Vector3 actualFocusPosition;


        private void Update()
        {
            FocusOnTarget(actualFocusPosition);
        }

        public void FocusOnTarget(Vector3 targetPos)
        {
            /*actualViewX = focusTarget.transform.position.x;
            actualViewY = focusTarget.transform.position.y;
            //this.transform.position = new Vector3(focusTarget.position.x, focusTarget.position.y, this.transform.position.z);
            transform.position -= new Vector3(((transform.position.x - actualViewX) * smoothCamera * 3) * Time.deltaTime, 
                                              ((transform.position.y - actualViewY) * smoothCamera * 3) * Time.deltaTime,
                                               0);*/

            transform.position = Vector3.SmoothDamp(transform.position, targetPos + new Vector3(0, 0, this.transform.position.z), ref velocity, smoothCamera);
            ClampCamera();
        }

        private void ClampCamera()
        {
            this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, clampLeft, clampRight),
                                                Mathf.Clamp(this.transform.position.y, clampLeft, clampRight),
                                                this.transform.position.z);
        }





        public void FocusDefault()
        {
            actualFocusPosition = focusTarget.position;
        }

        public void FocusOnAim(Vector2 pos)
        {
            actualFocusPosition = focusTarget.position + new Vector3(pos.x * AimCameraMultiplier, pos.y * AimCameraMultiplier, 0);
        }



    }
}

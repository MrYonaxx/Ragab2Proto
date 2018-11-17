using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ragab
{

    public class CameraScript : MonoBehaviour
    {
        [Header("Settings")]

        [SerializeField]
        UnityEngine.Camera camera;
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

        [Header("Orthographic Transition")]
        [SerializeField]
        int timeTransition = 10;


        Vector3 velocity = Vector3.zero;

        Vector3 actualFocusPosition;

        private IEnumerator orthographicCoroutine = null;


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
                                                Mathf.Clamp(this.transform.position.y, clampDown, clampUp),
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




        public void ChangeOrthographicSize(float newValue)
        {
            if(orthographicCoroutine != null)
            {
                StopCoroutine(orthographicCoroutine);
            }
            orthographicCoroutine = OrthographicSizeTransition(newValue);
            StartCoroutine(orthographicCoroutine);
        }

        private IEnumerator OrthographicSizeTransition(float newValue)
        {
            float time = timeTransition;
            float rate = (camera.orthographicSize - newValue) / timeTransition;
            while (time != 0)
            {
                camera.orthographicSize -= rate;
                time -= 1;
                yield return null;
            }

            camera.orthographicSize = newValue;
            orthographicCoroutine = null;
        }


    }
}

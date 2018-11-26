/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using UnityEngine;
using System.Collections;

namespace Ragab
{
    /// <summary>
    /// Definition of the ChangeRotation class
    /// </summary>
    public class ChangeRotation : MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        float speed = 0.5f;

        float rotation = 0;

        bool active = false;
        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */


        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */

        private void Update()
        {
            if (active == true)
            {
                transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime * SlowMotionManager.Instance.playerTime));

                if (rotation - Mathf.Abs(speed) * Time.deltaTime * SlowMotionManager.Instance.playerTime < transform.eulerAngles.z &&
                    transform.eulerAngles.z < rotation + Mathf.Abs(speed) * Time.deltaTime * SlowMotionManager.Instance.playerTime)
                {
                    transform.eulerAngles = new Vector3(0, 0, rotation);
                    active = false;
                }
            }
        }

        public void NewRotation(float newValue)
        {
            rotation = newValue;
            active = true;
        }

        #endregion

    } // ChangeRotation class

} // #PROJECTNAME# namespace
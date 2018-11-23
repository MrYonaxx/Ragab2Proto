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
    /// Definition of the Elevator class
    /// </summary>
    public class Elevator : MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        private float scrollSpeed;
        [SerializeField]
        private float tileSizeZ;
        [SerializeField]
        private bool loop = true;

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

        private Vector3 startPosition;

        void Start()
        {
            startPosition = transform.position;
        }

        void Update()
        {
            //float newPosition = Mathf.Repeat(Time.time * scrollSpeed * SlowMotionManager.Instance.playerTime, tileSizeZ);
            transform.position += new Vector3(0, scrollSpeed * Time.deltaTime * SlowMotionManager.Instance.playerTime, 0);
            if (transform.position.y < tileSizeZ - (scrollSpeed * Time.deltaTime * SlowMotionManager.Instance.playerTime))
            {
                if(loop == true)
                    transform.position = startPosition;
            }
        }


        public void Stop()
        {
            scrollSpeed = 0;
        }

        #endregion

    } // Elevator class

} // #PROJECTNAME# namespace
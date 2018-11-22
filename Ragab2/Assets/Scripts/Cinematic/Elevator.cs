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
            float newPosition = Mathf.Repeat(Time.time * scrollSpeed * SlowMotionManager.Instance.playerTime, tileSizeZ);
            transform.position = startPosition + Vector3.up * newPosition;
        }

        #endregion

    } // Elevator class

} // #PROJECTNAME# namespace
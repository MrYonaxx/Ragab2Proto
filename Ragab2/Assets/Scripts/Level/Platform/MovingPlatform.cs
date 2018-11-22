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
    /// Definition of the MovingPlatform class
    /// </summary>
    public class MovingPlatform : MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        float speed = 0.5f;

        [SerializeField]
        Transform[] waymarkers;

        int i;

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
            transform.position = Vector3.MoveTowards(transform.position, waymarkers[i].position, speed * Time.deltaTime * SlowMotionManager.Instance.playerTime);
            if (transform.position == waymarkers[i].position)
            {
                i += 1;
                if (i == waymarkers.Length)
                    i = 0;
            }
        }

        #endregion

    } // MovingPlatform class

} // #PROJECTNAME# namespace
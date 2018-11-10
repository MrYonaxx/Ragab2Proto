/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Ragab
{

    /// <summary>
    /// Definition of the RagabRoofDetection class
    /// </summary>
    public class RagabRoofDetection : MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [SerializeField]
        UnityEvent eventOnRoofEnter;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */


        #endregion

        #region Functions 

        private void OnTriggerEnter2D(Collider2D theCollision)
        {
            if (theCollision.gameObject.tag == "Ground")
            {
                eventOnRoofEnter.Invoke();
            }

        }

        #endregion

    } // RagabRoofDetection class

} // #Ragab# namespace
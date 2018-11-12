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
    /// Definition of the Enemy class
    /// </summary>
    public class Enemy : CharacterRaycast
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [Header("Event")]
        [SerializeField]
        UnityEvent eventHit;

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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "ProjectilePlayer")
            {
                eventHit.Invoke();
            }
        }

        #endregion

    } // Enemy class

} // #PROJECTNAME# namespace
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
    /// Definition of the TriggerZone class
    /// </summary>
    public class TriggerZone : MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        UnityEvent eventTrigger;
        
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

        void OnTriggerEnter2D(Collider2D other) 
        {
            if(other.gameObject.tag == "Player")
            {
                eventTrigger.Invoke();
            }
        }
        
        #endregion

    } // TriggerZone class

} // Ragab namespace
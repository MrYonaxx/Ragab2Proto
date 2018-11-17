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
    /// Definition of the EventFeedback class
    /// </summary>
    public class EventFeedback : Feedback
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Header("Event")]
        [SerializeField]
        UnityEvent eventFeedback;

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

        public override void StartFeedback(string param = null)
        {
            eventFeedback.Invoke();
        }

        #endregion

    } // EventFeedback class

} // #PROJECTNAME# namespace
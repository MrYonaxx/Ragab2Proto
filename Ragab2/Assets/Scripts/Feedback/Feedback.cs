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
    /// Definition of the Feedback class 
    /// </summary>
    public class Feedback : MonoBehaviour
    {
        // éventuellement changer ça en interface si je met le plugin pour serializer des interfaces

        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Header("Note pour savoir ce que c'est")]
        [SerializeField]
        string name;
        
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

        public virtual void StartFeedback()
        {

        }

        public virtual void StopFeedback()
        {

        }

        #endregion

    } // Feedback class

} // #PROJECTNAME# namespace
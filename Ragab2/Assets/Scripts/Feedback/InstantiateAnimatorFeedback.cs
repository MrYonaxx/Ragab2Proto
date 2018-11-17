/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ragab
{
    /// <summary>
    /// Definition of the InstantiateFeedback class
    /// </summary>
    public class InstantiateAnimatorFeedback : Feedback
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        Animator prefab;
        [SerializeField]
        Transform pos;

        List<Animator> gameObjectPool = new List<Animator>();
        
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

        public override void StartFeedback(string animName)
        {
            Animator newObject = Instantiate(prefab, pos.position, Quaternion.identity);
            gameObjectPool.Add(newObject);
        }

        public override void StopFeedback()
        {
            for(int i = 0; i < gameObjectPool.Count; i++)
            {
                Destroy(gameObjectPool[i].gameObject);
            }
            gameObjectPool.Clear();
        }
        
        #endregion

    } // InstantiateFeedback class

} // #PROJECTNAME# namespace
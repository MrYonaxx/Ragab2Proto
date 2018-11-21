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

        [SerializeField]
        bool poolOneObject = false;

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
        //feedback.speed = SlowMotionManager.Instance.playerTime;

        public override void StartFeedback(string animName)
        {
            if (poolOneObject == false)
            {
                Animator newObject = Instantiate(prefab, pos.position, Quaternion.identity);
                gameObjectPool.Add(newObject);
            }
            else
            {
                if(gameObjectPool.Count == 0)
                {
                    Animator newObject = Instantiate(prefab, pos.position, Quaternion.identity);
                    gameObjectPool.Add(newObject);
                }
                else
                {
                    gameObjectPool[0].gameObject.SetActive(true);
                    gameObjectPool[0].transform.position = pos.position;
                }
            }
        }

        public override void StopFeedback()
        {
            for(int i = 0; i < gameObjectPool.Count; i++)
            {
                Destroy(gameObjectPool[i].gameObject);
            }
            gameObjectPool.Clear();
        }











        public void SetSpeedAnimation()
        {
            prefab.speed = SlowMotionManager.Instance.playerTime;
        }

        public void AnimationEnd()
        {
            prefab.gameObject.SetActive(false);
            //prefab.enabled = false;
        }

        public void AnimationDestroy()
        {
            Destroy(this.gameObject);
            //prefab.enabled = false;
        }


        #endregion

    } // InstantiateFeedback class

} // #PROJECTNAME# namespace
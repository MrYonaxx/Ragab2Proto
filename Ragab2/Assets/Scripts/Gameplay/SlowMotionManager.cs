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
    /// Definition of the SlowMotionManager class
    /// </summary>
    public class SlowMotionManager : MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        public static SlowMotionManager Instance = null;

        public float playerTime = 1f;
        public float enemyTime = 1f;

        [SerializeField]
        private float transitionTime = 10;

        private IEnumerator slowMotionCoroutine = null;


        [Header("Event")]
        [SerializeField]
        UnityEvent eventSlowMotionActive;
        [SerializeField]
        UnityEvent eventSlowMotionUnactive;

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

        ////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void CheckIfSlowMotion(float value)
        {
            if (value >= 1)
            {
                eventSlowMotionUnactive.Invoke();
            }
            else
            {
                eventSlowMotionActive.Invoke();
            }
        }

        public void SetSlowMotion(float newValue)
        {
            CheckIfSlowMotion(newValue);

            if (slowMotionCoroutine != null)
            {
                StopCoroutine(slowMotionCoroutine);
            }
            playerTime = newValue;
            enemyTime = playerTime;
        }

        public void SetSlowMotionGradually(float newValue)
        {
            CheckIfSlowMotion(newValue);

            if (slowMotionCoroutine != null)
            {
                StopCoroutine(slowMotionCoroutine);
            }
            slowMotionCoroutine = SlowMoTransition(newValue);
            StartCoroutine(slowMotionCoroutine);
        }

        private IEnumerator SlowMoTransition(float newValue)
        {
            float time = transitionTime;
            float rate = (playerTime - newValue) / transitionTime;
            while (time != 0)
            {
                playerTime -= rate;
                enemyTime = playerTime;
                time -= 1;
                yield return null;
            }
            playerTime = newValue;
            enemyTime = playerTime;
            slowMotionCoroutine = null;
        }
        
        #endregion

    } // SlowMotionManager class

} // #PROJECTNAME# namespace
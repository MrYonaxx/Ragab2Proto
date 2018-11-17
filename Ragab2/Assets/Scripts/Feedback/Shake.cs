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
    /// Definition of the ScreenShake class
    /// </summary>
    public class Shake : Feedback
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [Header("Shake Object")]
        [SerializeField]
        Transform objectToShake;

        [Header("Shake Parameter")]
        [SerializeField]
        float shakeForceX = 1;
        [SerializeField]
        float shakeForceY = 1;
        [SerializeField]
        int shakeTime = 20;

        Vector3 initialPosition;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */


        #endregion

        #region Functions 

        public override void StartFeedback(string param = null)
        {
            ShakeScreen();
        }

        public void ShakeScreen()
        {
            StartCoroutine(ShakeCoroutine(objectToShake, shakeForceX, shakeForceY, shakeTime));
        }

        private IEnumerator ShakeCoroutine(Transform transformToShake, float forceX, float forceY, int time)
        {
            initialPosition = transformToShake.position;
            while(time != 0)
            {
                time -= 1;
                transformToShake.position += new Vector3(Random.Range(-forceX, forceX) * Time.deltaTime,
                                                         Random.Range(-forceY, forceY) * Time.deltaTime, 0);
                yield return null;
            }
            //transformToShake.position = initialPosition;
        }


        #endregion

    } // ScreenShake class

} // #PROJECTNAME# namespace
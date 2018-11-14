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
    /// Definition of the SoundManager class
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [SerializeField]
        AudioSource audiosource;
        [SerializeField]
        int volumeTransitionTime = 20;

        private IEnumerator changeVolumeCoroutine = null;

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

        public void ChangeVolumeGradually(float newValue)
        {
            if (changeVolumeCoroutine != null)
            {
                StopCoroutine(changeVolumeCoroutine);
            }
            changeVolumeCoroutine = VolumeCoroutine(newValue);
            StartCoroutine(changeVolumeCoroutine);
        }

        private IEnumerator VolumeCoroutine(float newValue)
        {
            int time = volumeTransitionTime;
            float rate = (audiosource.volume - newValue) / volumeTransitionTime;
            while (time != 0)
            {
                time -= 1;
                audiosource.volume -= rate;
                yield return null;
            }
            audiosource.volume = newValue;
            changeVolumeCoroutine = null;
        }


        public void ChangePitchGradually(float newValue)
        {
            if (changeVolumeCoroutine != null)
            {
                StopCoroutine(changeVolumeCoroutine);
            }
            changeVolumeCoroutine = PitchCoroutine(newValue);
            StartCoroutine(changeVolumeCoroutine);
        }

        private IEnumerator PitchCoroutine(float newValue)
        {
            int time = volumeTransitionTime;
            float rate = (audiosource.pitch - newValue) / volumeTransitionTime;
            while (time != 0)
            {
                time -= 1;
                audiosource.pitch -= rate;
                yield return null;
            }
            audiosource.volume = newValue;
            changeVolumeCoroutine = null;
        }

        #endregion

    } // SoundManager class

} // #PROJECTNAME# namespace
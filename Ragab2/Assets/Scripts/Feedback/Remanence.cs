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
    /// Definition of the Remanence class
    /// </summary>
    public class Remanence : Feedback
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        SpriteRenderer spriteToCopy;

        [SerializeField]
        SpriteRenderer[] remanence;

        [SerializeField]
        int interval = 10;

        [SerializeField]
        float alphaValue = 0.8f;

        private IEnumerator remanenceCoroutine = null;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */


        #endregion

        #region Functions 

        public override void StartFeedback(string param = null)
        {
            RemanenceStart();
        }

        public override void StopFeedback()
        {
            if (remanenceCoroutine == null)
                return;
            StopCoroutine(remanenceCoroutine);
            remanenceCoroutine = null;
            for (int i = 0; i < remanence.Length; i++)
            {
                remanence[i].color = new Color(1, 1, 1, 0);
            }
        }

        public void RemanenceStart()
        {
            if (remanenceCoroutine != null)
                return;
            remanenceCoroutine = RemanenceCoroutine();
            StartCoroutine(remanenceCoroutine);
        }

        private IEnumerator RemanenceCoroutine()
        {
            int time;
            int index = 0;
            float ratio = alphaValue / (interval * remanence.Length);
            Debug.Log(ratio);
            Color alphaColor = new Color(1, 1, 1, alphaValue);
            Color rate = new Color(0, 0, 0, ratio);
            while (true)
            {
                time = interval;
                remanence[index].sprite = spriteToCopy.sprite;
                remanence[index].color = alphaColor;
                remanence[index].transform.position = spriteToCopy.transform.position;
                //remanence[index].transform. = spriteToCopy.transform.localScale;
                remanence[index].transform.rotation = spriteToCopy.transform.rotation;
                remanence[index].flipX = spriteToCopy.flipX;
                while (time != 0)
                {
                    for(int i = 0; i < remanence.Length; i++)
                    {
                        remanence[i].color -= rate;
                    }
                    time -= 1;
                    yield return null;
                }
                index += 1;
                if(index == remanence.Length)
                {
                    index = 0;
                }
            }


        }

        #endregion

    } // Remanence class

} // #PROJECTNAME# namespace
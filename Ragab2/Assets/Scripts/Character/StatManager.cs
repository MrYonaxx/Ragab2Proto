/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Ragab
{
    /// <summary>
    /// Definition of the StatManager class
    /// </summary>
    public class StatManager : MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [Header("")]
        [SerializeField]
        private float hp;

        public float Hp
        {
            get { return hp; }
            set { hp = value; }
        }

        [SerializeField]
        private float hpMax;

        [Header("HUD")]
        [SerializeField]
        private RectTransform image;
        [SerializeField]
        private float imageSize = 300;



        private IEnumerator shakeCoroutine = null;

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


        public void LoseHP(float damage)
        {
            hp -= damage;
            DrawLifeBar();
        }

        public void DrawLifeBar()
        {
            image.sizeDelta = new Vector2((hp / hpMax) * imageSize, image.sizeDelta.y);
        }


        public void ShakeHUD(float force, float time)
        {
            if (shakeCoroutine != null)
            {
                return;
            }
            shakeCoroutine = ShakeHUDCoroutine(force, time);
            StartCoroutine(shakeCoroutine);
        }

        private IEnumerator ShakeHUDCoroutine(float force, float time)
        {
            Vector3 origin = image.parent.transform.position;
            while (time > 0)
            {
                image.parent.transform.position = origin + new Vector3(Random.Range(-force, force), Random.Range(-force, force), 0);
                time -= 1;
                yield return null;
            }
            image.parent.transform.position = origin;
            shakeCoroutine = null;
        }

        #endregion

    } // StatManager class

} // #PROJECTNAME# namespace
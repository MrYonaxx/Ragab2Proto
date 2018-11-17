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
    /// Definition of the SpriteFlash class
    /// </summary>
    public class SpriteFlash : Feedback
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        private MaterialPropertyBlock propBlock;
        SpriteRenderer spriteRenderer;

        [SerializeField]
        Color flashColor;
        [SerializeField]
        int flashTime = 10;

        private IEnumerator flashCoroutine = null;

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

        ////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
        /// </summary>
        protected void Start()
        {
            propBlock = new MaterialPropertyBlock();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public override void StartFeedback(string param = null)
        {
            Flash();
        }

        // Potentiellement beaucoup de lag
        public void Flash()
        {
            spriteRenderer.GetPropertyBlock(propBlock);
            propBlock.SetFloat("_FlashAmount", 1);
            spriteRenderer.SetPropertyBlock(propBlock);
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
            }
            flashCoroutine = FlashCoroutine();
            StartCoroutine(flashCoroutine);
        }

        private IEnumerator FlashCoroutine()
        {
            int time = flashTime;
            float rate = 1f / flashTime;
            float currentRate = 1f;
            while (time != 0)
            {
                time -= 1;
                currentRate -= rate;
                spriteRenderer.GetPropertyBlock(propBlock);
                propBlock.SetFloat("_FlashAmount", currentRate);
                spriteRenderer.SetPropertyBlock(propBlock);
                yield return null;
            }
            spriteRenderer.GetPropertyBlock(propBlock);
            propBlock.SetFloat("_FlashAmount", 0);
            spriteRenderer.SetPropertyBlock(propBlock);
            flashCoroutine = null;
        }

        
        #endregion

    } // SpriteFlash class

} // #PROJECTNAME# namespace
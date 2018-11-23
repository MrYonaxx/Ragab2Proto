/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using UnityEngine;

using UnityEngine.UI;

using UnityEngine.Events;
using TMPro;
using System.Collections;

namespace Ragab
{
    /// <summary>
    /// Definition of the CrystalManager class
    /// </summary>
    public class CrystalManager : MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [Header("Paramètre")]
        [SerializeField]
        int crystalNumber = 3;
        [SerializeField]
        float crystalAmount = 200;
        [SerializeField]
        float recoveryAmount = 1;
        [SerializeField]
        float timeBeforeRecovery = 1;

        [SerializeField]
        float slideConsumption = 1;
        [SerializeField]
        float traceDashConsumption = 1;


        [Header("Draw")]
        [SerializeField]
        TextMeshProUGUI textDebug;
        [SerializeField]
        RectTransform[] crystals;
        [SerializeField]
        RectTransform lowerTimerDash;
        [SerializeField]
        RectTransform upperTimerDash;

        [SerializeField]
        UnityEvent eventCrystalBreak;


        private IEnumerator recoveryCoroutine = null;
        private IEnumerator consumptionCoroutine = null;

        float actualCrystalNumber = 3;
        float actualCrystalAmount = 100;

        bool traceDashing = false;


        private IEnumerator shakeCoroutine = null;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        public float getCrystalNumber()
        {
            return actualCrystalNumber;
        }

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */

        private void Start()
        {
            actualCrystalNumber = crystalNumber+1;
            actualCrystalAmount = 0;
            DrawCrystals();
        }

        public void DrawCrystals()
        {
            if (traceDashing == true)
            {
                DrawTraceDashTimer();
            }
            else
            {
                HideTraceDashTimer();
                //upperTimerDash.anchoredPosition = new Vector2(0, 0);
                //lowerTimerDash.anchoredPosition = new Vector2(0, lowerTimerDash.rect.width);
            }
            textDebug.text = actualCrystalNumber.ToString() + " - " + actualCrystalAmount.ToString();
            if (actualCrystalNumber == crystalNumber + 1)
            {
                for (int i = 0; i < crystalNumber; i++)
                {
                    crystals[i].anchoredPosition = new Vector2(0, 0);
                }
            } else {
                for (int i = 0; i < crystalNumber; i++)
                {
                    if(i < actualCrystalNumber-1)
                    {
                        crystals[i].anchoredPosition = new Vector2(0, 0);
                    }
                    else if (actualCrystalNumber-1 < i)
                    {
                        crystals[i].anchoredPosition = new Vector2(0, -crystals[i].rect.height);
                    }
                    else
                    {
                        crystals[i].anchoredPosition = new Vector2(0, -crystals[i].rect.height + (crystals[i].rect.height * (actualCrystalAmount / crystalAmount)));
                    }
                }
            }
        }


        private void DrawTraceDashTimer()
        {
            upperTimerDash.anchoredPosition = new Vector2(upperTimerDash.rect.width + (upperTimerDash.rect.width * (actualCrystalAmount / crystalAmount)), 0);
            lowerTimerDash.anchoredPosition = new Vector2(-lowerTimerDash.rect.width * (actualCrystalAmount / crystalAmount), 0);
        }

        private void HideTraceDashTimer()
        {
            if(upperTimerDash.anchoredPosition.x < upperTimerDash.rect.width * 2)
                upperTimerDash.anchoredPosition += new Vector2(50, 0);
            if(lowerTimerDash.anchoredPosition.x > -lowerTimerDash.rect.width)
                lowerTimerDash.anchoredPosition += new Vector2(-50, 0);
        }

        public void StartRecovery()
        {

            if (recoveryCoroutine != null)
            {
                return;
            }

            if (consumptionCoroutine != null)
            {
                StopCoroutine(consumptionCoroutine);
                consumptionCoroutine = null;
            }


            traceDashing = false;
            recoveryCoroutine = Recovery(timeBeforeRecovery, recoveryAmount);
            StartCoroutine(recoveryCoroutine);
        }



        public void StartConsumptionSlide()
        {
            if(consumptionCoroutine != null)
            {
                return;
            }

            if (recoveryCoroutine != null)
            {
                StopCoroutine(recoveryCoroutine);
                recoveryCoroutine = null;
            }

            traceDashing = false;
            consumptionCoroutine = Consumption(slideConsumption);
            StartCoroutine(consumptionCoroutine);
        }


        public void StartConsumptionTraceDashing()
        {

            if (consumptionCoroutine != null)
            {
                return;
            }

            if (recoveryCoroutine != null)
            {
                StopCoroutine(recoveryCoroutine);
                recoveryCoroutine = null;
            }


            traceDashing = true;
            consumptionCoroutine = Consumption(traceDashConsumption);
            StartCoroutine(consumptionCoroutine);


        }

        public void ConsumeCrystal()
        {
            actualCrystalAmount = crystalAmount;
            actualCrystalNumber -= 1;
        }


        private IEnumerator Recovery(float initialTime, float amount)
        {
            yield return new WaitForSeconds(initialTime);
            while (actualCrystalNumber <= crystalNumber)
            {
                while (actualCrystalAmount < crystalAmount)
                {
                    actualCrystalAmount += amount;
                    DrawCrystals();
                    yield return null;
                }
                actualCrystalNumber += 1;
                actualCrystalAmount = 0;
            }
            DrawCrystals();
            recoveryCoroutine = null;
        }

        private IEnumerator Consumption(float amount)
        {
            while (actualCrystalNumber > 0)
            {
                while (actualCrystalAmount != 0)
                {
                    actualCrystalAmount -= amount;
                    DrawCrystals();
                    yield return null;
                }
                eventCrystalBreak.Invoke();
                actualCrystalNumber -= 1;
                actualCrystalAmount = crystalAmount;
            }
            DrawCrystals();
            consumptionCoroutine = null;
        }



        public void ShakeHUD(float force, float time)
        {
            if(shakeCoroutine != null)
            {
                return;
            }
            shakeCoroutine = ShakeHUDCoroutine(force, time);
            StartCoroutine(shakeCoroutine);
        }

        private IEnumerator ShakeHUDCoroutine(float force, float time)
        {
            Vector3[] origin = new Vector3[crystals.Length];
            for (int i = 0; i < crystals.Length; i++)
            {
                origin[i] = crystals[i].parent.transform.position;
            }
            while (time > 0)
            {
                for (int i = 0; i < crystals.Length; i++)
                {
                    crystals[i].parent.transform.position = origin[i] + new Vector3(Random.Range(-force, force), Random.Range(-force, force), 0);
                }
                time -= 1;
                yield return null;
            }
            for (int i = 0; i < crystals.Length; i++)
            {
                crystals[i].parent.transform.position = origin[i];
            }
            shakeCoroutine = null;
        }


        #endregion

    } // CrystalManager class

} // #PROJECTNAME# namespace
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
        UnityEvent eventCrystalBreak;


        private IEnumerator recoveryCoroutine = null;
        private IEnumerator consumptionCoroutine = null;

        float actualCrystalNumber = 3;
        float actualCrystalAmount = 100;

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

        private void Start()
        {
            actualCrystalNumber = crystalNumber+1;
            actualCrystalAmount = 0;
            DrawCrystals();
        }

        public void DrawCrystals()
        {
            textDebug.text = actualCrystalNumber.ToString() + " - " + actualCrystalAmount.ToString();
        }

        public void StartRecovery()
        {
            if(consumptionCoroutine != null)
            {
                StopCoroutine(consumptionCoroutine);
                consumptionCoroutine = null;
            }



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

            actualCrystalAmount = crystalAmount;
            actualCrystalNumber -= 1;

            consumptionCoroutine = Consumption(traceDashConsumption);
            StartCoroutine(consumptionCoroutine);


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



        #endregion

    } // CrystalManager class

} // #PROJECTNAME# namespace
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

    [System.Serializable]
    public class ArenaWave
    {
        [SerializeField]
        public int numberEnemies = 0;
        [SerializeField]
        public UnityEvent eventWave;
    }

    /// <summary>
    /// Definition of the Arena class
    /// </summary>
    public class Arena : MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        ArenaWave[] waves;

        int step = 0;
        int killNumber = 0;
        
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

        public void AddKill()
        {
            killNumber += 1;
            if(killNumber >= waves[step].numberEnemies)
            {
                waves[step].eventWave.Invoke();
                step += 1;
            }
        }
        
        #endregion

    } // Arena class

} // #PROJECTNAME# namespace
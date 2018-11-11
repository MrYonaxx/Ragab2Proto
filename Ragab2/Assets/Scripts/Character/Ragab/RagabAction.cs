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
    /// Definition of the RagabAction class
    /// </summary>
    public class RagabAction : MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [SerializeField]
        BaseProjectile projectilePrefab;
        [SerializeField]
        int maxNumberProjectile = 20;


        List<BaseProjectile> listObject;

        [SerializeField]
        Transform viseur;
        [SerializeField]
        Transform curseur;


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
        protected virtual void Start()
        {
            listObject = new List<BaseProjectile>(maxNumberProjectile);
        }


        public void Shoot()
        {

        }

        public void Aim(Vector2 angle)
        {
            /*float length = Mathf.Sqrt(angle.x * angle.x + angle.y * angle.y);
            if(length > 0.7f)
            {
                angle = new Vector2(angle.x / length, angle.y / length);
                Debug.Log(angle);
            }*/
            //angle.Normalize();
            //angle = Vector2.ClampMagnitude(angle, 0.2f);
            float angleAim = Mathf.Atan2(angle.x, -angle.y) * Mathf.Rad2Deg;
            viseur.localRotation = Quaternion.Euler(new Vector3(0,0,angleAim));

            //curseur.position = new Vector2(this.transform.position.x, this.transform.position.y) + angle;
            //viseur.localRotation.SetLookRotation(curseur.position);
        }

        
        #endregion

    } // RagabAction class

} // #PROJECTNAME# namespace
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
    /// Definition of the MissileHelicopterProjectile class
    /// </summary>
    public class MissileHelicopterProjectile : MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [SerializeField]
        protected float speedYMin = 10;
        [SerializeField]
        protected float speedYMax = 20;
        [SerializeField]
        protected float accelerationY = 0.5f;
        [SerializeField]
        protected float accelerationX = 0.5f;
        [SerializeField]
        protected float speedX = 20;
        [SerializeField]
        protected float longevity = 5;

        float time = 0;
        int direction = -1;
        float actualSpeedX = 0;
        float actualSpeedY = 0;

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
        protected void Start()
        {
            actualSpeedY = Random.Range(speedYMin, speedYMax);
            actualSpeedX = 0;
        }

        protected void Update()
        {
            if (transform.eulerAngles.z == 180)
                direction = 1;
            if(actualSpeedY < 0)
                actualSpeedY += accelerationY;
            if(Mathf.Abs(actualSpeedX) < speedX)
                actualSpeedX += accelerationX * direction;

            time += Time.deltaTime * SlowMotionManager.Instance.enemyTime;

            transform.position += new Vector3(actualSpeedX * SlowMotionManager.Instance.playerTime * Time.deltaTime,
                                              actualSpeedY * SlowMotionManager.Instance.playerTime * Time.deltaTime, 0);
            if (time >= longevity)
            {
                Destroy(this.gameObject);
            }
        }

        /*void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Ground")
                Destroy(this.gameObject);
        }*/

        #endregion

    } // MissileHelicopterProjectile class

} // #PROJECTNAME# namespace
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
    /// Definition of the BossHelicopter class
    /// </summary>
    public class BossHelicopter : Enemy
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [Header("Helicopter")]
        [SerializeField]
        float shootStartup = 1f;
        [SerializeField]
        float shootInterval = 0.5f;

        [SerializeField]
        CrapoProjectile projectile;

        [SerializeField]
        int missileNumber = 5;
        [SerializeField]
        float missileInterval = 0.5f;
        [SerializeField]
        float missileStartup = 5;
        [SerializeField]
        MissileHelicopterProjectile missile;




        [SerializeField]
        int danmakuTime = 10;
        [SerializeField]
        float danmakuInterval = 0.5f;
        [SerializeField]
        CrapoProjectile projectileDanmaku;

        [SerializeField]
        Transform gatlingPositionLeft;
        [SerializeField]
        Transform gatlingPositionRight;

        [Header("Deplacement")]
        [SerializeField]
        float rotateSpeed = 1;
        [SerializeField]
        Transform positionLeft;
        [SerializeField]
        Transform positionRight;

        [Header("Camera")]
        [SerializeField]
        CameraScript cameraObject;

        float posX;

        float actualShootInterval = 0;
        float actualShootNumber = 0;
        int etape = 0;

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

        protected override void Start()
        {
            base.Start();
            direction = -1;
        }

        protected override void InitializePattern(int valMin = 0, int valMax = 0)
        {
            actualSpeedX = 0;
            actualSpeedY = 0;

            if (currentPattern == 0) // Initialize
            {
                currentPattern = Random.Range(1, 5);
            }
            else
            {
                currentPattern = Random.Range(valMin, valMax);
            }

            switch (currentPattern)
            {
                case 1: // Helicopter tire en avancant
                    timePatternStartup = shootStartup;
                    timePattern = 1000f;
                    actualShootInterval = 0;
                    break;
                case 2: // Helicopter charge
                    timePatternStartup = 1;
                    timePattern = 1000f;
                    break;
                case 3: // Helicopter envoie 3 salve de 5 missiles
                    actualShootNumber = missileNumber;
                    timePatternStartup = missileStartup;
                    timePattern = 1000f;
                    break;
                case 4: // Helicopter go Touhou
                    timePattern = danmakuTime;
                    actualShootInterval = danmakuInterval;
                    etape = 0;
                    break;
            }

            // Toujours mettre le select pattern ici juste après
            SelectPattern();
        }




        protected override void SelectPattern()
        {
            switch (currentPattern)
            {
                case 1: // Helicopter Tire
                    if (timePatternStartup > 0)
                        EnemyShootStartup();
                    else
                        EnemyShoot();
                    break;
                case 2: // Helicopter Charge
                    if (timePatternStartup > 0)
                        EnemyDescend();
                    else
                        EnemyCharge();
                    break;
                case 3:
                    if (timePatternStartup > 0)
                        EnemyFireMissileStartup();
                    else
                        EnemyFireMissile();
                    break;
                case 4:
                    EnemyDanmaku();
                    break;



                // Animation reposition
                case 30: //
                    Repositionning();
                    break;
            }
        }





        private void EnemyShootStartup()
        {
            if(timePatternStartup == shootStartup)
            {
                if (direction == -1)
                    posX = positionLeft.position.x;
                else
                    posX = positionRight.position.x;
            }
            if (-speedMax < actualSpeedX && actualSpeedX < speedMax)
                actualSpeedX -= speedAcceleration * direction;

            if(Mathf.Abs(this.transform.eulerAngles.z) < 39 || Mathf.Abs(this.transform.eulerAngles.z) > 321)
                this.transform.eulerAngles += new Vector3(0, 0, rotateSpeed * -direction * Time.deltaTime * SlowMotionManager.Instance.enemyTime);

        }



        private void EnemyShoot()
        {

            if (Mathf.Abs(this.transform.eulerAngles.z) < 39 || Mathf.Abs(this.transform.eulerAngles.z) > 321)
                this.transform.eulerAngles += new Vector3(0, 0, rotateSpeed * -direction * Time.deltaTime * SlowMotionManager.Instance.enemyTime);

            if(Mathf.Abs(actualSpeedX) < 20)
                actualSpeedX += speedAcceleration * 1.25f * direction;

            if (transform.position.x < posX + 18 && direction == -1)
            {
                etape = 3;
                timePattern = 100;
                currentPattern = 30;
                actualSpeedX = speedMax * direction;
            }
            else if (transform.position.x > posX - 18 && direction == 1)
            {
                etape = 3;
                timePattern = 100;
                currentPattern = 30;
                actualSpeedX = speedMax * direction;
            }
            else
            {
                cameraObject.ChangeFocusBetween();
                actualShootInterval -= Time.deltaTime * SlowMotionManager.Instance.playerTime;
                if(actualShootInterval <= 0)
                {
                    CrapoProjectile proj;
                    if (direction == -1)
                    {
                        proj = Instantiate(projectile, gatlingPositionLeft.position, this.transform.rotation);
                        proj.transform.Rotate(new Vector3(0, 0, 180));
                    }
                    else
                    {
                        proj = Instantiate(projectile, gatlingPositionRight.position, this.transform.rotation);
                    }
                    proj.transform.Rotate(new Vector3(0, 0, Random.Range(-10, 10)));
                    proj.transform.position += new Vector3(actualSpeedX * Time.deltaTime * SlowMotionManager.Instance.playerTime, 0, 0);
                    actualShootInterval = shootInterval;
                }
            }
        }





        private void EnemyDescend()
        {
            if (direction == -1)
                posX = positionLeft.position.x;
            else
                posX = positionRight.position.x;

            actualSpeedY -= speedAcceleration * 20;
            actualSpeedX += speedAcceleration * 1 * direction;


            if (Mathf.Abs(this.transform.eulerAngles.z) > 2)
                this.transform.eulerAngles += new Vector3(0, 0, rotateSpeed * direction * Time.deltaTime * SlowMotionManager.Instance.enemyTime);

        }


        private void EnemyCharge()
        {

            if (Mathf.Abs(this.transform.eulerAngles.z) > 2)
                this.transform.eulerAngles += new Vector3(0, 0, rotateSpeed * direction * Time.deltaTime * SlowMotionManager.Instance.enemyTime);

            actualSpeedY *= 0.75f;
            actualSpeedX += speedAcceleration * 2 * direction;

            if (transform.position.x < posX + 18 && direction == -1)
            {
                etape = 3;
                timePattern = 100;
                currentPattern = 30;
                //actualSpeedX = speedMax * direction;
            }
            else if (transform.position.x > posX - 18 && direction == 1)
            {
                etape = 3;
                timePattern = 100;
                currentPattern = 30;
                //actualSpeedX = speedMax * direction;
            }
        }



        private void EnemyFireMissileStartup()
        {

            if (Mathf.Abs(this.transform.eulerAngles.z) > 2)
                this.transform.eulerAngles += new Vector3(0, 0, rotateSpeed * 2 * direction * Time.deltaTime * SlowMotionManager.Instance.enemyTime);
            else
            {
                if (direction == -1)
                {
                    CrapoProjectile proj = Instantiate(projectile, gatlingPositionLeft.position, this.transform.rotation);
                    proj.transform.Rotate(new Vector3(0, 0, 180));
                }
                if (direction == 1)
                    Instantiate(projectile, gatlingPositionRight.position, this.transform.rotation);
            }

        }


        private void EnemyFireMissile()
        {

            if(actualShootNumber == 0)
            {
                timePattern = 2;
                actualShootNumber -= 1;
            } 
            else if (actualShootNumber == -1)
            {
                if (Mathf.Abs(this.transform.eulerAngles.z) < 19 || Mathf.Abs(this.transform.eulerAngles.z) > 341)
                    this.transform.eulerAngles += new Vector3(0, 0, rotateSpeed * -direction * Time.deltaTime * SlowMotionManager.Instance.enemyTime);
            }
            else
            {
                if (Mathf.Abs(this.transform.eulerAngles.z) > 2)
                    this.transform.eulerAngles += new Vector3(0, 0, rotateSpeed * direction * Time.deltaTime * SlowMotionManager.Instance.enemyTime);

                MissileHelicopterProjectile proj = Instantiate(missile, transform.position, Quaternion.identity);
                if (direction == 1)
                    proj.transform.Rotate(0, 0, 180);
                timePatternStartup = missileInterval;

                actualShootNumber -= 1;
            }
        }


        private void EnemyDanmaku()
        {
            if (etape == 0)
            {
                if (Mathf.Abs(this.transform.eulerAngles.z) < 39 || Mathf.Abs(this.transform.eulerAngles.z) > 321)
                    this.transform.eulerAngles += new Vector3(0, 0, rotateSpeed * 2 * -direction * Time.deltaTime * SlowMotionManager.Instance.enemyTime);
                else
                    etape = 1;
            }
            else
            {
                if (Mathf.Abs(this.transform.eulerAngles.z) > 1)
                    this.transform.eulerAngles += new Vector3(0, 0, rotateSpeed * 2 * direction * Time.deltaTime * SlowMotionManager.Instance.enemyTime);
                else
                    etape = 0;
            }


         
            actualShootInterval -= Time.deltaTime * SlowMotionManager.Instance.playerTime;
            if (actualShootInterval <= 0)
            {
                float rotationfactor = -30;
                for (int i = 0; i < 3; i++)
                {

                    CrapoProjectile proj;
                    if (direction == -1)
                    {
                        proj = Instantiate(projectileDanmaku, gatlingPositionLeft.position, this.transform.rotation);
                        proj.transform.Rotate(new Vector3(0, 0, 180 + rotationfactor));
                    }
                    else
                    {
                        proj = Instantiate(projectileDanmaku, gatlingPositionRight.position, this.transform.rotation);
                        proj.transform.Rotate(new Vector3(0, 0, rotationfactor));
                    }
                    proj.transform.position += new Vector3(actualSpeedX * Time.deltaTime * SlowMotionManager.Instance.playerTime, 0, 0);
                    actualShootInterval = danmakuInterval;
                    rotationfactor += 30;
                }
            }
         
        }




        private void Repositionning()
        {
            switch(etape)
            {
                case 3:
                    if (transform.position.x < posX + -5 && direction == -1)
                    {
                        direction *= -1;
                        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z * -1);
                        if (direction == 1)
                            posX = positionLeft.position.x;
                        else
                            posX = positionRight.position.x;
                        etape -= 1;
                        actualSpeedX *= -0.5f;
                        this.transform.eulerAngles = new Vector3(0, 0, 20 * -direction);
                        transform.position = new Vector3(positionLeft.position.x - 5, positionLeft.position.y, positionLeft.position.z);
                    }
                    else if (transform.position.x > posX + 5 && direction == 1)
                    {
                        direction *= -1;
                        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z * -1);
                        if (direction == 1)
                            posX = positionLeft.position.x;
                        else
                            posX = positionRight.position.x;
                        etape -= 1;
                        actualSpeedX *= -0.5f;
                        this.transform.eulerAngles = new Vector3(0, 0, 20 * -direction);
                        transform.position = new Vector3(positionRight.position.x + 5, positionRight.position.y, positionRight.position.z);
                    }
                    break;
                case 2:
                    cameraObject.ChangeFocusBetween(this.transform);
                    if (transform.position.x >= posX && direction == 1)
                    {
                        actualSpeedX = 0;
                        etape = 0;
                        currentPattern = 0;
                        timePattern = 0;
                    }
                    else if (transform.position.x <= posX && direction == -1)
                    {
                        actualSpeedX = 0;
                        etape = 0;
                        currentPattern = 0;
                        timePattern = 0;
                    }
                    break;
            }


        }


        protected override void Hit(float angle, float forceMultiplier = 1, int damage = 1)
        {
            base.Hit(angle, forceMultiplier);
            statManager.DrawLifeBar();
        }

        #endregion

    } // BossHelicopter class

} // #PROJECTNAME# namespace
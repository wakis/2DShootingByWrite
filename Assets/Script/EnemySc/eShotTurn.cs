using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace enemyMoveScript
{
    public class eShotTurn : objStatusDefault
    {
        bool Shot;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void Renewal(GameObject obj)//切り替え時
        {

        }

        public override int Setting(GameObject obj)//ゲーム開始時
        {
            Shot = false;
            objRule = Camera.main.GetComponent<gameObj>();
            speed = obj.GetComponent<objStatusRenewal>().eStatus.speed;
            coolTime = -1f;
            setCoolTime = obj.GetComponent<objStatusRenewal>().eStatus.setCoolTime;
            return 1;
        }

        public override int Stay(GameObject obj)//待機状態
        {
            obj.transform.position = transPos_Front(obj.transform, objRule.scrollSpeed);
            if (obj.transform.position.x < objRule.ScreenSize[1].x) return 1;
            return 0;
        }

        public override int Play(GameObject obj)//画面内行動
        {
            if (!Shot) {
                obj.transform.position = transPos_Front(obj.transform, objRule.scrollSpeed + speed);
                if((obj.transform.position-objRule.player.transform.position).magnitude< (objRule.ScreenSize[1] - objRule.ScreenSize[0]).magnitude / 2)
                {
                    Instantiate(obj.GetComponent<objStatusRenewal>().bullet, obj.transform.position,
                        Quaternion.Euler(new Vector3(0f, 0f, Vector3.Angle(obj.transform.position, objRule.player.transform.position))));
                    Shot = true;
                }
            }
            else
            {
                obj.transform.position += Vector3.right * (objRule.scrollSpeed + speed)*Time.deltaTime/2f;
                if (coolTime < 0f)
                {
                    Instantiate(obj.GetComponent<objStatusRenewal>().bullet, obj.transform.position,
                        Quaternion.Euler(new Vector3(0f, 0f, Vector3.Angle(obj.transform.position, objRule.player.transform.position))));
                    coolTime = setCoolTime;
                }
                else
                {
                    coolTime -= Time.deltaTime;
                }
            }
            if (objRule.ScreenSize[0].x * 1.1f > obj.transform.position.x
                || objRule.ScreenSize[1].x * 1.1f < obj.transform.position.x) return 1;
            return 0;
        }
        public override void Loss(GameObject obj)//消失時
        {

        }
    }
}

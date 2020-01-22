using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace enemyMoveScript
{
    public class eSinMove : objStatusDefault
    {
        float standTime;
        float wait;
        float rand;
        float transY;
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
            rand = Random.Range(-1f,1f);
            objRule = Camera.main.GetComponent<gameObj>();
            speed = obj.GetComponent<objStatusRenewal>().eStatus.speed;
            coolTime = -1f;
            setCoolTime = obj.GetComponent<objStatusRenewal>().eStatus.setCoolTime;
            wait = (Mathf.Abs(objRule.ScreenSize[0].y-transform.position.y)< Mathf.Abs(objRule.ScreenSize[1].y - transform.position.y)?
                Mathf.Abs(objRule.ScreenSize[0].y - transform.position.y)*0.9f: Mathf.Abs(objRule.ScreenSize[1].y - transform.position.y) * 0.9f);
            transY = transform.position.y;
            standTime = 0f;
            coolTime=Random.Range(0f, 1f);
            return 1;
        }

        public override int Stay(GameObject obj)//待機状態
        {

            obj.transform.position = transPos_Front(obj.transform, objRule.scrollSpeed);
            if (obj.transform.position.x < objRule.ScreenSize[1].x*1.1f) return 1;
            return 0;
        }

        public override int Play(GameObject obj)//画面内行動
        {
            standTime += objRule.gameDeltaTime;
            obj.transform.position = transPos_Front(obj.transform, objRule.scrollSpeed);
            var trans = transform.position;
            trans.y = transY +Mathf.Sin(rand + standTime) *wait;
            transform.position = trans;
            
            if (coolTime < 0f)
            {
                Instantiate(obj.GetComponent<objStatusRenewal>().bullet, obj.transform.position,
                    Quaternion.Euler(new Vector3(0f, 0f, Vector3.Angle(obj.transform.position, objRule.player.transform.position))));
                coolTime = setCoolTime;
            }
            else
            {
                coolTime -= objRule.gameDeltaTime;
            }
            if (objRule.ScreenSize[0].x * 1.1f > obj.transform.position.x) return 1;
                return 0;
        }
        public override void Loss(GameObject obj)//消失時
        {

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace enemyMoveScript
{
    public class eSinMove : objStatusDefault
    {
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
            return 1;
        }

        public override int Stay(GameObject obj)//待機状態
        {
            Debug.Log(objRule.scrollSpeed);
            obj.transform.position = transPos_Front(obj.transform, objRule.scrollSpeed);
            if (obj.transform.position.x < objRule.ScreenSize[1].x*1.1f) return 1;
            return 0;
        }

        public override int Play(GameObject obj)//画面内行動
        {
            obj.transform.position = transPos_Front(obj.transform, objRule.scrollSpeed);
            var trans = transform.position;
            trans.y = transY +Mathf.Sin(rand + trans.x)*wait;
            transform.position = trans;
            return 0;
        }
        public override void Loss(GameObject obj)//消失時
        {

        }
    }
}

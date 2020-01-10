using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace enemyMoveScript
{
    public class eWarpMove : objStatusDefault
    {
        bool warp;
        LineRenderer line;
        Vector3[] linePos = new Vector3[2];
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
            warp = false;
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
            if (coolTime < 0f)
            {
                warp = true;
                Instantiate(obj.GetComponent<objStatusRenewal>().bullet, obj.transform.position,
                    Quaternion.Euler(new Vector3(0f, 0f, Vector3.Angle(obj.transform.position, objRule.player.transform.position))));
                coolTime = setCoolTime;
            }
            else
            {
                if (warp)
                {
                    WarpMove();
                    warp = !warp;
                }
                else
                {
                    if (10f * (1f - coolTime / setCoolTime) < 1f)
                    {

                        line.SetPosition(1, linePos[1] - (linePos[1] - linePos[0]) * 10f*(1f - coolTime / setCoolTime));
                    }else if(coolTime>0f)
                    {
                        Destroy(line);
                    }
                    coolTime -= Time.deltaTime;
                }
            }
            if (objRule.ScreenSize[0].x * 1.1f > obj.transform.position.x) return 1;
            return 0;
        }
        public override void Loss(GameObject obj)//消失時
        {

        }

        void WarpMove()
        {
            linePos[0] = transform.position;
            linePos[1] = linePos[0];
            linePos[0].x -= (objRule.ScreenSize[1].x - objRule.ScreenSize[0].x) / 10f;
            linePos[0].y = Random.Range(objRule.ScreenSize[0].y * 0.9f, objRule.ScreenSize[1].y*0.9f);
            transform.position = linePos[0];
            line = Instantiate(new GameObject()).AddComponent<LineRenderer>();
            line.useWorldSpace = true;
            //line.materials[0] = Resources.GetBuiltinResource<Material>("Sprites-Default.mat");
            //Materialどうにかしなさい
            line.startColor = line.endColor = new Color(1f, 0.3f, 0.3f);
            line.startWidth = line.endWidth = 0.1f;
            line.positionCount = 2;
            line.SetPositions(new Vector3[] { linePos[0], linePos[1] });
        }
        private void OnDestroy()
        {
            Destroy(line);
        }
    }
}

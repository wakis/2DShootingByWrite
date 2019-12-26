using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Debuger
{
    public class DGetPos : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            List<Vector2> dx = new List<Vector2>();
            var debuger= GetComponent<SpriteRenderer>().sprite.vertices;
            foreach (var d in debuger)
            {
                Debug.Log(d);
                
            }


        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                Start();
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                Sprite any = GetComponent<SpriteRenderer>().sprite;
                var ans = any.vertices;
                int n = 0;
                IList<Vector2[]> Ivect2 = new List<Vector2[]>();
                Ivect2.Add(ans);
                foreach (var a in ans)
                {
                    Debug.Log(Ivect2[0][n]);
                    //Debug.Log(a);
                    n++;
                }
                Debug.Log("laster"+n);
                Debug.Log(any.GetPhysicsShapeCount());

                Debug.Log(any.GetPhysicsShapePointCount(0));

                Ivect2[0][1] = new Vector2(1,1);
                any.OverridePhysicsShape(Ivect2);

                //any.vertices = ans;
            }
        }
    }
}

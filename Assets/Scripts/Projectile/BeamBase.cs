using UnityEngine;
using System.Collections;

public class BeamBase : MonoBehaviour {
    public float Lifespan = 1;
    public float Str = 1;
    public float BaseDamage = 1;
    public bool isFriendly = true;
    public bool LoopActionStack = true;
    public bool Go = false;    
    Transform origin;
    ActionBase[] actions;

    // Use this for initialization
    void Start() {
        actions = GetComponents<ActionBase>();
    }

    // Update is called once per frame
    void Update() {
        if (Go)
        {
            DoActionStack();
            LineRenderer lr = GetComponent<LineRenderer>();
            //MyBeam.GetComponent<BeamBase>().Str = 1;
            lr.SetPosition(0,origin.position);

            Vector3 end = new Vector3(0,0,0);
            RaycastHit2D hit = Physics2D.Raycast(origin.position, origin.right);
            if (hit.collider != null)
            {

                Collider2D col = hit.collider;
                GetComponent<BeamCollideBase>().OnBeamHit(col);
                end = new Vector3(hit.point.x, hit.point.y, 0);
            }
            transform.Find("HitEffect").position = end;
            lr.SetPosition(1, end);
            // MyBeam.transform.position = this.transform.Find("BulletSpawner").position;
        }
        else
        {
            LineRenderer lr = GetComponent<LineRenderer>();
            lr.SetPosition(0, new Vector3(-1000, 0, 0));            
            lr.SetPosition(1, new Vector3(-1000,0,0));
            transform.Find("HitEffect").position = new Vector3(-1000, 0, 0);
            foreach (Transform child in transform)
            {
                if(child.name != "HitEffect")Destroy(child.gameObject);
            }
        }

        
    }

    public void SetOrigin(Transform or)
    {
        origin = or;
    }

    void DoActionStack()
    {
        int inactiveCount = 0;
        for (int x = 0; x < actions.Length; x++)
        {
            if (actions[x].DoAction)
            {
                if (x == 0)
                    actions[x].CheckAction();

                else if (actions[x].WaitForPrevious && actions[x - 1].DoAction)
                    x = actions.Length;
                else
                {
                    actions[x].CheckAction();
                }


            }
            else
                inactiveCount++;

        }


        if (inactiveCount == actions.Length && LoopActionStack)
        {

            for (int x = 0; x < actions.Length; x++)
            {
                actions[x].Reset();
            }
        }
    }
}

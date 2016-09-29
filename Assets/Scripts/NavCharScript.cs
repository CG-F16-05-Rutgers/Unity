using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavCharScript : MonoBehaviour
{

    //NavMeshAgent agent;
    List<GameObject> active = new List<GameObject>();
    List<GameObject> moving = new List<GameObject>();
    List<GameObject> stop = new List<GameObject>();
    GameObject activeObs = null;
    public float speed;

    private float timer;

    // Use this for initialization
    void Start()
    {
        timer = 0.0f;
        speed = 100.0f;
        //agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.tag == "Player")
                {
                    activeObs = null;
                    bool check = true;
                    if (active.Count == 0)
                    {
                        active.Add(hit.transform.gameObject);
                    }
                    else
                    {
                        // foreach (GameObject obj in active)
                        for (int i = 0; i < active.Count; i++)
                        {
                            if (active[i] == hit.transform.gameObject)
                            {
                                active.Remove(active[i]);
                                check = false;
                            }
                        }
                        if (check)
                        {
                            active.Add(hit.transform.gameObject);
                        }
                    }
                }
                else if (hit.transform.tag == "Obstacle")
                {
                    active.Clear();
                    if (activeObs == null)
                        activeObs = hit.transform.gameObject;
                    else
                    {
                        if (activeObs == hit.transform.gameObject)
                        {
                            activeObs = null;
                        }
                        else
                        {
                            activeObs = hit.transform.gameObject;
                        }
                    }
                }
                else
                {
                    if (active.Count != 0)
                    {
                        //foreach (GameObject obj in active)
                        for (int i = 0; i < active.Count; i++)
                        {
                            Debug.Log("setdestination ---------------");
                            active[i].GetComponent<NavMeshAgent>().SetDestination(hit.point);
                            for (int k = 0; k < stop.Count; k++)
                            {
                                if (active[i].gameObject == stop[k].gameObject)
                                {
                                    stop.Remove(stop[k].gameObject);
                                    break;
                                }
                            }
                            if (moving.Count > 0)
                            {
                                bool c = true;
                                for (int k = 0; k < moving.Count; k++ )
                                {
                                    if (moving[k].gameObject == active[i].gameObject)
                                    {
                                        c = false;
                                        break;
                                    }
                                }
                                if (c)
                                {
                                    active[i].GetComponent<NavMeshAgent>().Resume();
                                    Debug.Log("add into moving list");
                                    moving.Add(active[i].gameObject);
                                }
                            }
                            else
                            {
                                active[i].GetComponent<NavMeshAgent>().Resume();
                                Debug.Log("add into moving list");
                                moving.Add(active[i].gameObject);
                            }
                            
                        }                         
                    }
                }
            }
        }

        if (timer <= 0.0f)
        {
            timer = 2.0f;
            
            if(stop.Count>0){
                for (int k = 0; k < stop.Count; k++)
                {
                    Debug.Log("doing stop thing ++++++++++++");
                    stop[k].GetComponent<NavMeshAgent>().Stop();
                    for (int n = 0; n < moving.Count; n++)
                    {
                        if (moving[n].gameObject == stop[k].gameObject)
                        {
                            moving.Remove(moving[n].gameObject);
                            break;
                        }
                    }
                }
                stop.Clear();
            }

            if (moving.Count > 0)
            {
                int size = moving.Count;
                Debug.Log("moving.count = " + size);
                int count = 0;
                for (int i = 0; i < size; i++)
                {
                    GameObject temp = moving[i].gameObject;
                    float d1x = temp.transform.position.x;
                    float d1z = temp.transform.position.z;
                    float d2x = temp.GetComponent<NavMeshAgent>().destination.x;
                    float d2z = temp.GetComponent<NavMeshAgent>().destination.z;
                    float distance = (d1x - d2x) * (d1x - d2x) + (d1z - d2z) * (d1z - d2z);

                    if (distance <= 9)
                    {
                        
                            Debug.Log("adding into the stop list");
                            count++;
                            stop.Add(moving[i].gameObject);
                        
                    }
                }

                /*
                Debug.Log("count = "+count);
                Debug.Log("size = "+size);
                if (count == size)
                {
                    Debug.Log("clear the moving list");
                    moving.Clear();
                }
                */
            }
        }

        timer -= Time.deltaTime;

        if (activeObs != null)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                Vector3 fwd = activeObs.transform.forward;
                fwd.Normalize();
                activeObs.GetComponent<Rigidbody>().AddForce(fwd * speed);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                Vector3 fwd = -activeObs.transform.forward;
                fwd.Normalize();
                activeObs.GetComponent<Rigidbody>().AddForce(fwd * speed);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                Vector3 fwd = activeObs.transform.right;
                fwd.Normalize();
                activeObs.GetComponent<Rigidbody>().AddForce(fwd * speed);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Vector3 fwd = -activeObs.transform.right;
                fwd.Normalize();
                activeObs.GetComponent<Rigidbody>().AddForce(fwd * speed);
            }
        }

    }

    public void assignRandomValues()
    {
        System.Random rand = new System.Random();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("CrazyBall")){
            float rand1 = rand.Next(-100,100);
            float rand2 = rand.Next(-100,100);
            obj.GetComponent<Rigidbody>().AddForce(5 * new Vector3(rand1, 0, rand2));
        }
    }
    /*
    bool checkStuck(GameObject obj, GameObject obj2) //different between 1 second
    {
        Vector3 oldDest, newDest;
        oldDest = obj.GetComponent<NavMeshAgent>().destination;
        newDest = obj2.GetComponent<NavMeshAgent>().destination;
        if(System.Math.Abs(oldDest.x - newDest.x)+System.Math.Abs(oldDest.z - newDest.z) <= 1){
            return true;
        }
        return false;
    }
     */
}
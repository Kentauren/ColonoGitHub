using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public float harvestTime;
    public float availableResource;
    public Transform treeLog;
    Animator m_Animator;
    public bool gatherers = false;
    public bool occupied = false;
    public GameObject colonist;
    Movement mv;

    // Start is called before the first frame updatess
    void Start()
    {
        StartCoroutine(ResourceTick());
        m_Animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (availableResource <= 10) //executes animation when value hits below 10
        {
            //If current health is 50 play animation ("TreeFalls") 
            Debug.Log("Keep Clear! THE TREE IS FALLING!!");
            m_Animator.SetTrigger("TreeFallsTrigger"); //Animation for the falling tree
        }

        if (availableResource <= 0)
        {
            Debug.Log("Tree is cutted into logs");
            Instantiate(treeLog, new Vector3(transform.position.x, Terrain.activeTerrain.SampleHeight(transform.position), transform.position.z), Quaternion.Euler(90, 0, 0)); //Deploys a wooden log at the sameposition as the tree
            Instantiate(treeLog, new Vector3(transform.position.x, Terrain.activeTerrain.SampleHeight(transform.position), transform.position.z + 3.5f), Quaternion.Euler(90, 0, 0));
            Instantiate(treeLog, new Vector3(transform.position.x, Terrain.activeTerrain.SampleHeight(transform.position), transform.position.z + 7), Quaternion.Euler(90, 0, 0));
            ChangeTask();           
        }
    }
    public void OnTriggerExit(Collider Collision)
    {
        if (Collision.gameObject.tag == "Selectable")
        {
            gatherers = false;
            occupied = false;
            gameObject.tag = "Resource"; //Changes tree tag to "Resource"
        }
    }

    public void OnTriggerStay(Collider Collision)
    {
        if (Collision.gameObject.tag == "Selectable")
        {
            gameObject.tag = "Occupied";
            gatherers = true;
        }
    }

    public void ResourceGather()
    {
        if (gatherers == true)
        {
            availableResource -= 1;
        }
    }

    IEnumerator ResourceTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            ResourceGather();
        }
    }

    void ChangeTask()
    {
        GameObject.FindGameObjectWithTag("Selectable").GetComponent<Movement>().NewTask();  
        Destroy(gameObject); //Destroys the object when availableResource = 0
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenownedGames.Apex;

public class Crowd : MonoBehaviour
{
    //[Group("Crowd Members")]
    [SerializeField]
    private List<CrowdMember> crowdMembers = new List<CrowdMember>();

    private Transform _target;

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            crowdMembers.Add(transform.GetChild(i).GetComponent<CrowdMember>());
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<CrowdMember> GetMembers()
    {
        return crowdMembers;
    }

  //  public void SetAlertLocation(Transform target)
  //  {
  //      _target = target;
  //  }
}

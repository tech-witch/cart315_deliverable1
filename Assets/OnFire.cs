using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFire : MonoBehaviour
{
  //Basic fire variables
  public GameObject FirePrefab;
  GameObject myFire;
  public bool onFire = true;
  //Does the object have a limited burn time?
  public bool DeleteAfterBurn=false;
  public float BurnTime=1;
  //Is the object replaced once its deleted?
  public bool replaceable=false;
  public GameObject ReplacementObject;
  float elapsedTime = 0f;

  public GameObject neighbor1;
  public GameObject neighbor2;
  public GameObject neighbor3;

  void Start()
  {
    if (onFire){
      StartNewFire();
    }
  }

  void FixedUpdate()
  {
    if(onFire){
      elapsedTime += Time.deltaTime;
    }
    if (DeleteAfterBurn){
      if (elapsedTime >= 1f) {
       elapsedTime = elapsedTime % 1f;
       if (BurnTime>0){
         BurnTime-=1;
       }
       if (BurnTime==0){
         burnedUp();
       }
       }
     }


  }

  void OnTriggerEnter (Collider other) {
    if (other.gameObject.CompareTag("flammable")&&!other.gameObject.GetComponent<OnFire>().onFire && this.onFire)
    {
      other.gameObject.GetComponent<OnFire>().onFire = true;
      other.gameObject.GetComponent<OnFire>().StartNewFire();
    }
  }

  void StopFire(){
    this.onFire=false;
    Destroy(myFire);
  }

  void StartNewFire(){
    GameObject myFire= Instantiate(FirePrefab);
    this.onFire=true;
    myFire.transform.parent = this.transform;
    myFire.transform.localPosition=Vector3.zero;
    StartCoroutine(spreadFire());

  }

  //Called if the object has limited burn time
  void burnedUp(){
    if (replaceable){
      var newOjbect = Instantiate(ReplacementObject);
      Vector3 temp =this.transform.position;
      newOjbect.transform.position = temp;
    }
    Destroy(gameObject);
  }

  IEnumerator spreadFire(){
      if (neighbor1!=null && neighbor1.CompareTag("flammable") && !neighbor1.GetComponent<OnFire>().onFire) {
        yield return new WaitForSeconds(2);
        neighbor1.GetComponent<OnFire>().StartNewFire();
      }
      if (neighbor2!=null && neighbor2.CompareTag("flammable") && !neighbor2.GetComponent<OnFire>().onFire) {
        yield return new WaitForSeconds(2);
        neighbor2.GetComponent<OnFire>().StartNewFire();
      }
      if (neighbor3!=null && neighbor3.CompareTag("flammable") && !neighbor3.GetComponent<OnFire>().onFire) {
        yield return new WaitForSeconds(2);
        neighbor3.GetComponent<OnFire>().StartNewFire();
      }
  }
}

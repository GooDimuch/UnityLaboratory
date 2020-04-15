using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTLBController : MonoBehaviour
{
  const float speed = 100;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKey(KeyCode.A))
    {
      transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - Time.deltaTime * speed, transform.eulerAngles.z);
    }
    if (Input.GetKey(KeyCode.D))
    {
      transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + Time.deltaTime * speed, transform.eulerAngles.z);
    }
    if (Input.GetKey(KeyCode.W))
    {
      transform.eulerAngles = new Vector3(transform.eulerAngles.x + Time.deltaTime * speed, transform.eulerAngles.y, transform.eulerAngles.z);
    }
    if (Input.GetKey(KeyCode.S))
    {
      transform.eulerAngles = new Vector3(transform.eulerAngles.x - Time.deltaTime * speed, transform.eulerAngles.y, transform.eulerAngles.z);
    }
    if (Input.GetKey(KeyCode.Q))
    {
      transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + Time.deltaTime * speed);
    }
    if (Input.GetKey(KeyCode.E))
    {
      transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - Time.deltaTime * speed);
    }
  }
}

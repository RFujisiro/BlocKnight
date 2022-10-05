using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMain : MonoBehaviour
{
    PlayerInput input;
    Rigidbody rb;
    int speed = 5;
    int maxspeed = 5;
    public int jumpPower = 5;
    bool buildingMode=true;

    public GameObject hedObject;
    GameObject cameraObject;
    public Vector3 cameraRotate;
    public Vector3 transformFor;
    public GameObject testBuildingObject;
    public Vector3 key;

    public GameObject cursorObject;
    public float cursorRange=0.5f;

    #region BuildingMode

    BuildingManager buildingManagerSctipt;
    public bool isBuildingMode=false;
    #endregion
    public Vector3 debugDrawPos;
    #region Debug
    public Vector3 flont, buck, right, left;
    public Vector3 moveVelocity;

    #endregion
    int layerMask;
    // Start is called before the first frame update
    void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Player");
        layerMask = ~layerMask;

        input = GetComponent<PlayerInput>();
        rb=GetComponent<Rigidbody>();
        cameraObject= hedObject.transform.GetChild(0).gameObject;
        buildingManagerSctipt =GameObject.FindWithTag("GameManager").gameObject.GetComponent<BuildingManager>();
        Debug.Log(hedObject.transform.position+":"+cameraObject.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        Camera();
        transformFor = transform.forward;
        if (buildingMode) Building();

        #region
        
        flont= Vector3.forward;
        buck= Vector3.back;
        right= Vector3.right;
        left= Vector3.left;
        #endregion

    }

    private void PlayerMove()
    {
        key.x = input.actions["Move"].ReadValue<Vector2>().x;
        key.z = input.actions["Move"].ReadValue<Vector2>().y;
        rb.AddRelativeForce(key * speed);

        float a = rb.velocity.x > 0 ? rb.velocity.x - maxspeed : 0;

        Vector2 nowSpeed = new Vector2(rb.velocity.x > 0 ? rb.velocity.x : rb.velocity.x * -1,
                                      rb.velocity.z > 0 ? rb.velocity.z : rb.velocity.z * -1);

        rb.AddForce(nowSpeed.x > maxspeed ? (rb.velocity.x - maxspeed) * -1 : 0, 0,
                    nowSpeed.y > maxspeed ? (rb.velocity.z - maxspeed) * -1 : 0);





    }
 
    private void NewPlayerMove()
    {
        key.x = input.actions["Move"].ReadValue<Vector2>().x;
        key.z = input.actions["Move"].ReadValue<Vector2>().y;

        if (transformFor.x > 0)
        {

        }
        

        rb.velocity = moveVelocity;
    }

    
    private void Camera()
    {
        cameraRotate.x -= input.actions["CameraMove"].ReadValue<Vector2>().y;
        cameraRotate.y += input.actions["CameraMove"].ReadValue<Vector2>().x;
        if (cameraRotate.x > 90) cameraRotate.x = 90;
        if (cameraRotate.x < -45) cameraRotate.x = -45;
        transform.rotation = Quaternion.Euler(0, cameraRotate.y, 0);
        hedObject.transform.rotation = Quaternion.Euler(cameraRotate.x, cameraRotate.y, 0);

    }


   public Vector3 putPos,buildingKey;
   public bool isKey=true;

    public Vector3 cursolPos;
    private void Building()
    {

        Debug.Log(LayerMask.NameToLayer("Player"));
        cameraObject.transform.position = (hedObject.transform.position) + hedObject.transform.forward * cursorRange;
        if (Physics.Linecast(hedObject.transform.position, cursorObject.transform.position, layerMask))
        {
            Debug.Log("HIt");
            cursorRange -= 0.1f;
        } else if (Physics.Linecast(cursorObject.transform.position, cursorObject.transform.position + (-cursorObject.transform.forward)))
        {
            Debug.Log("NoHit");
            if (cursorRange < 8) cursorRange += 0.1f;
        }
        Debug.DrawLine(hedObject.transform.position, cursorObject.transform.position, Color.red);
        Debug.DrawLine(cursorObject.transform.position, cursorObject.transform.position + (cursorObject.transform.forward*0.1f), Color.blue);
        cursolPos=cursorObject.transform.position;
        //if (Physics.Linecast(hedObject.transform.position, hedObject.transform.position + (hedObject.transform.forward * 5)))
        //{
        //    Debug.Log("blocked");
        //}


        //Debug.DrawLine(hedObject.transform.position,hedObject.transform.position+(hedObject.transform.forward*5),Color.red);

        //buildingKey = input.actions["BuildingPos"].ReadValue<Vector2>();
        //if (isKey)
        //{
        //    putPos.x+=buildingKey.x;
        //    putPos.z+=buildingKey.y;
        //    testBuildingObject.transform.position = putPos;
        //    isKey = false;
        //}
        //if (buildingKey == new Vector3(0, 0, 0))isKey = true;
    }

    #region InputSystem
    public void MainAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            cursolPos.y+=1;
            if (isBuildingMode) buildingManagerSctipt.PutBlock(new Vector3(0,0,0), cursolPos, 0);
        }

    }
    public void SubAction(InputAction.CallbackContext context)
    {

    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
            rb.AddForce(0, jumpPower, 0, ForceMode.Impulse);
    }

    public void BuildSwitch(InputAction.CallbackContext context)
    {
        if (context.started) isBuildingMode=isBuildingMode == true? false:true;
    }

    #endregion
}

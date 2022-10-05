using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GameObject[] block;

    Vector3[] putBlockPos=new Vector3[6];
    float blockSize=1.0f;

    public bool PutBlock(Vector3 blockRotate,Vector3 putPos,int blockN)
    {
        putPos = new Vector3((int)putPos.x,(int)putPos.y,(int)putPos.z);
        for (int i =0; i < putBlockPos.Length; i++)
        {
           // if (putPos == putBlockPos[i]) return false;
        }
        GameObject brock = Instantiate(block[blockN], putPos,Quaternion.Euler(blockRotate));

        return true;
    }

}

using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Rewindable : MonoBehaviourPun
{
    List<Vector3> myPos = new List<Vector3>();
    List<Quaternion> myRot = new List<Quaternion>();
    List<Vector3> myVel = new List<Vector3>();
    protected Rigidbody rb;
    

    [SerializeField] int maxAmount = 100;
    protected bool shouldBeCapturingPosition = true;
    protected bool backingTime = false;

    public virtual IEnumerator RewindTime()
    {
        if (backingTime) yield break;
        backingTime = true;
        /*Stack<Vector3> aux = new Stack<Vector3>();
        for (int i = 0; i < myPosList.Count; i++)
        {
            aux.Push(myPosList.Dequeue());
        }
        Stack<Quaternion> aux2 = new Stack<Quaternion>();
        for (int i = 0; i < myRotList.Count; i++)
        {
            aux2.Push(myRotList.Dequeue());
        

        for (int i = 0; i < aux.Count; i++)
        {
            transform.position = aux.Pop();
            transform.rotation = aux2.Pop();
            Debug.Log("Backing");
            yield return new WaitForSeconds(0.1f);
        }*/
        float aux = 2 / myPos.Count;
        for (int i = 0; i < myPos.Count; i++)
        {
            transform.position = myPos[myPos.Count - i - 1];
            transform.rotation = myRot[myRot.Count - i - 1];
            rb.velocity = Vector3.zero;

            Debug.Log("rewinding");
            yield return new WaitForSeconds(aux);
        }
        rb.velocity = myVel[0];
        /*for (int i = 0; i < mypos.Count; i++)
        {
            transform.position = mypos[i];

            yield return new WaitForSeconds(aux);
        }*/
        myPos.Clear();
        myRot.Clear();
        myVel.Clear();
        backingTime = false;
    }
    public virtual void addNewPos(Vector3 pos, Quaternion rot, Vector3 velocity)
    {
        if (myPos.Count > maxAmount)
        {
            //myPosList.Dequeue();
            //myRotList.Dequeue();
            myPos.RemoveAt(0);
            myRot.RemoveAt(0);
            myVel.RemoveAt(0);
        }
        // myPosList.Enqueue(transform.position);
        //myRotList.Enqueue(transform.rotation);
        myPos.Add(pos);
        myRot.Add(rot);
        myVel.Add(rb.velocity);

    }
}

using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Rewindable : MonoBehaviourPun
{
    protected List<Vector3> myPos = new List<Vector3>();
    protected List<Quaternion> myRot = new List<Quaternion>();
    protected List<Vector3> myVel = new List<Vector3>();
    protected Rigidbody rb;

    [SerializeField] float speed = 2;

    [SerializeField] protected float timeToRewind = 2;
    [SerializeField] protected float rewindTime = 2;
    public float fps = 20;
    [HideInInspector] public float timeBetweenSaves = 0f;
    public bool shouldBeCapturingPosition = true;
    protected bool backingTime = false;

    public IEnumerator capturePosition;
    [HideInInspector] public CharacterHead owner;


    public virtual void Start()
    {
        timeBetweenSaves = 1 / fps;

        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

        shouldBeCapturingPosition = true;
        capturePosition = addPositionAlways();
        StartCoroutine(capturePosition);
    }

    /*public void Rewind()
    {
        photonView.RPC("RequestRewind", RpcTarget.AllBuffered);
    }
    [PunRPC]
    public void RequestRewind()
    {
        //PlayerInstantiator.Instance.RequestRewindable()
        StartCoroutine(RewindTime());
    }
    */

    public void OnRewind()
    {
        photonView.RPC("Rewind", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void Rewind()
    {
        StartCoroutine(RewindTime());
    }

    public virtual IEnumerator RewindTime()
    {
        if (backingTime) yield break;
        shouldBeCapturingPosition = false;
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
        float aux = timeToRewind / myPos.Count;
        for (int i = 0; i < myPos.Count; i++)
        {
            transform.position = myPos[myPos.Count - i - 1];
            transform.rotation = myRot[myRot.Count - i - 1];
            rb.velocity = Vector3.zero;
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
        shouldBeCapturingPosition = true;
    }
    public virtual void addNewPos(Vector3 pos, Quaternion rot, Vector3 velocity)
    {
        float aux = rewindTime * fps;
        if (myPos.Count >= aux)
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

    public IEnumerator addPositionAlways()
    {
        while (true)
        {
            if (shouldBeCapturingPosition)
            {
                addNewPos(transform.position, transform.rotation, rb.velocity);
                yield return new WaitForSeconds(timeBetweenSaves);
            }
            else yield return new WaitForSeconds(timeBetweenSaves);
        }
    }

    public void ClearRewindable()
    {
    }
}

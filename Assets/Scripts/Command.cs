using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command 
{
    public virtual void Do() { }
    public virtual void Undo() { }
}

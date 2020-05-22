using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IController 
{
    float Horizontal();
    float Vertical();
    bool Jump();

    float HorizontalRotation();
    float VerticalRotation();

    bool Shoot();
    bool ThrowGranade();
    bool Rewind();
}

using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetSelector : NetworkBehaviour
{
    public abstract bool TryGetTarget(out Transform[] targets);
}

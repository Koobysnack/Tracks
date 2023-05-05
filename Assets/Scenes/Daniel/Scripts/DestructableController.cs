using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DestructableController : EntityController
{
    // Start is called before the first frame update
    protected abstract IEnumerator DeathRattle();
 

}

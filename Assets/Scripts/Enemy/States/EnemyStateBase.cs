using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyStateBase<Key>
{
    Key key
    {
        get;
    }

    EnemyController controller
    {
        get;
        set;
    }

    abstract void Init();
    abstract Key OnEnter();
    abstract Key OnUpdate();
    abstract void OnExit();
}
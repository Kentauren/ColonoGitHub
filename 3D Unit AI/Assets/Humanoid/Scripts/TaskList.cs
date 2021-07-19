using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskList {
    Gathering,
    Moving,
    Idle,
    Building,
    Delivering,
    PickingUp,
    FindObject,

    //Woodcuter tasks
    HeadingToTree,
    CuttingTree,
    HeadingToLog,
    MovingLogToStorage,
    LookingForLog,
    LookingForStorage,

    //MillWorker tasks
    HeadingToLumberMill,
    CheckStorage,
    PickingUpWoodenLogs,
    EmptyingTimberStorage,
    MovingWoodenLogToStorage,
    HeadingTowardsStorage,
    MovingLogToLumberMill,
    MovingWoodenLogToWorkbench,
    CheckWorkbench,
    MovingTowardsMillsWoodenLogStorage,
    WaitingForResources,
    HeadingTowardsWorkbench,
    MovingTimberToStorage,
    HeadingTowardsWoodenLogStorage,
    ProducingTimber,
    TakeTimberWorkbench,

    //Soldier tasks
    Attacking,
    PreparingForattack,
    ReadyForAttack,
    Blocking
}

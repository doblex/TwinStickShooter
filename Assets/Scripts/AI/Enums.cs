public enum ZombieType
{
    BASIC,
    FAST,
    RANGER
}

public enum STATE
{
    IDLE, 
    PATROL, 
    CHASE, 
    ATTACK,
    DEATH,
    REPOSITION,
    SPECIAL_ATTACK,
    ATTACHED,
}

public enum EVENT
{
    ENTER, UPDATE, EXIT
}

public enum COMBATRANGE
{
    CLOSE,
    RANGE,
    FAR
}

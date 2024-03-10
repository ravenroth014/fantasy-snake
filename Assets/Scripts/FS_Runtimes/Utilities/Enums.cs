namespace FS_Runtimes.Utilities
{
    public enum EGridState
    {
        Empty
        , Obstacle
        , Occupied
        , Walled
    }

    public enum ECharacterType
    {
        Hero
        , Enlist
        , Enemy
        , None
    }

    public enum EDirection
    {
        Up
        , Down
        , Left
        , Right
        , None
    }

    public enum EPlayerAction
    {
        Up
        , Right
        , Down
        , Left
        , RotateLeft
        , RotateRight
        , None
    }

    public enum EPoolingType
    {
        Stack
        , LinkedList
    }

    public enum EGameState
    {
        GameMenu
        , GamePrepare
        , GamePlay
        , GameOver
        , GameError
    }

    public enum ECharacterSwitch
    {
        Left
        , Right
    }

    public enum EDecorateType
    {
        Decorate
        , Obstacle
    }
}

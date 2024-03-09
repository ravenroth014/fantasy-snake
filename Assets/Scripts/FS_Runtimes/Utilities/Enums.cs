namespace FS_Runtimes.Utilities
{
    public enum EGridState
    {
        Empty
        , Obstacle
        , Occupied
    }

    public enum ECharacterType
    {
        Player
        , Enlist
        , Enemy
    }

    public enum EDirection
    {
        Up
        , Down
        , Left
        , Right
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
    }
}

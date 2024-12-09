namespace Player
{
    public enum PlayerDirection
    {
        Up,
        Down,
        Left,
        Right
    }


    public static class PlayerDirectionExtension
    {
        public static PlayerDirection GetOpposite(this PlayerDirection direction)
        {
            return direction switch 
            {
                PlayerDirection.Up => PlayerDirection.Down,
                PlayerDirection.Down => PlayerDirection.Up,
                PlayerDirection.Right => PlayerDirection.Left,
                PlayerDirection.Left => PlayerDirection.Right,
                _ => PlayerDirection.Up
            };
        }
    }
}
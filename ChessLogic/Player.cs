namespace ChessLogic
{
    // Enum to represent the player in the game.
    // "None" represents no player, "White" represents the white player, and "Black" represents the black player.
    public enum Player
    {
        None,  // No player (used for empty spots or when no player is active)
        White, // Represents the white player
        Black  // Represents the black player
    }

    // Static class to extend functionality for the Player enum.
    public static class PlayerExtensions
    {
        // Extension method to return the opponent of a given player.
        // It switches between White and Black players, and returns Player.None if the player is None.
        public static Player Opponent(this Player player)
        {
            // The 'switch' expression is used to determine the opponent:
            return player switch
            {
                // If the player is White, the opponent is Black
                Player.White => Player.Black,
                // If the player is Black, the opponent is White
                Player.Black => Player.White,
                // If the player is None, there is no opponent (return None)
                _ => Player.None,
            };
        }
    }
}

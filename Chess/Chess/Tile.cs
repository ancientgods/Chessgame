
namespace Chess
{
    public class Tile
    {
        public bool White;
        public ChessPiece ChessPiece;
      
        public Tile(bool white, ChessPiece chesspiece = ChessPiece.None)
        {
            White = white;
            ChessPiece = chesspiece;
        }
    }
}

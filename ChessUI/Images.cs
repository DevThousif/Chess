using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChessLogic;


namespace ChessUI
{
    public static class Images
    {

        private static readonly Dictionary<PieceType, ImageSource> whiteSources = new()
        {
            { PieceType.Pawn, LoadImage("Assets/wp.png") },
            { PieceType.Bishop, LoadImage("Assets/wB.png") },
            { PieceType.Knight, LoadImage("Assets/wN.png") },
            { PieceType.Rook, LoadImage("Assets/wR.png") },
            { PieceType.Queen, LoadImage("Assets/wQ.png") },
            { PieceType.King, LoadImage("Assets/wK.png") },
        };
        private static readonly Dictionary<PieceType, ImageSource> blackSources = new()
        {
            { PieceType.Pawn, LoadImage("Assets/bp.png") },
            { PieceType.Bishop, LoadImage("Assets/bB.png") },
            { PieceType.Knight, LoadImage("Assets/bN.png") },
            { PieceType.Rook, LoadImage("Assets/bR.png") },
            { PieceType.Queen, LoadImage("Assets/bQ.png") },
            { PieceType.King, LoadImage("Assets/bK.png") },
        };


        private static ImageSource LoadImage(string filePath)
        {
            return new BitmapImage(new Uri(filePath, UriKind.Relative));
        }

        public static ImageSource GetImage(Player color, PieceType type)
        {

            return color switch
            {
                Player.White => whiteSources[type],
                Player.Black => blackSources[type],
                _ => null,
            };


        }
        public static ImageSource GetImage (Piece piece)
        {
            if (piece == null)
            {
                return null;
            }
            return  GetImage(piece.Color, piece.Type);
        }


    }


}

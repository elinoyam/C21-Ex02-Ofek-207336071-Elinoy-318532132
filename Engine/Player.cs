using System;


namespace Engine
{
    public class Player
    {
        public const char k_Player1Sign = 'X';
        public const char k_Player2Sign = 'O';
        private readonly bool r_IsComputer;
        private readonly char r_UserSign;
        private static int s_CountPlayers = 0;
        private readonly string r_PlayerName;
        private int m_CountWins = 0;
        private AIComputer m_AIComputer = null;

        public bool IsComputer
        {
            get
            {
                return r_IsComputer;
            }
        }

        public char PlayerSign
        {
            get
            {
                return r_UserSign;
            }
        }

        public string PlayerName
        {
            get
            {
                return r_PlayerName;
            }
        }

        public int PlayerWinsCounter
        {
            get
            {
                return m_CountWins;
            }
            set
            {
                m_CountWins = value;
            }  
        }

        public Player(bool i_IsComputer)
        {
            r_UserSign = s_CountPlayers == 0  ? k_Player1Sign : k_Player2Sign;
            r_IsComputer = i_IsComputer;
            r_PlayerName = !r_IsComputer  ? $"Player {s_CountPlayers + 1}" : "Computer";
            if (i_IsComputer) 
            {
                m_AIComputer = new AIComputer();
            }

            s_CountPlayers++;
            if(s_CountPlayers >= 2)
            {
                s_CountPlayers = 0;
            }
        }

        public void IncreaseWinsCounter()
        {
            ++PlayerWinsCounter;
        }

        public void GetComputerChoice(ref Board i_Board, out int o_Row, out int o_Column)
        {
            double winningPercentage;
            const bool v_IsMaxPlayer = true;
           
            o_Column = m_AIComputer.MinimaxAlgorithm(ref i_Board, AIComputer.k_UndefinedRowOrColumn, AIComputer.k_UndefinedRowOrColumn, k_Player2Sign,  Board.k_AmountToWin,  double.NegativeInfinity,  double.PositiveInfinity, v_IsMaxPlayer,  out winningPercentage);
            o_Row = i_Board.GetRowByPlayerColumnChoice(o_Column);
            i_Board.MakeMove(o_Column, k_Player2Sign);
        }
    }
}

using System;


namespace Engine
{
    public class Player
    {
        private readonly bool m_IsComputer;
        private readonly char m_UserSign;
        private static int m_CountPlayers = 0;
        private readonly string m_PlayerName;
        private int m_CountWins = 0;
        private AIComputer m_AIComputer = null;

        public bool IsComputer
        {
            get
            {
                return m_IsComputer;
            }
        }

        public char PlayerSign
        {
            get
            {
                return m_UserSign;
            }
        }

        public string PlayerName
        {
            get
            {
                return m_PlayerName;
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
            m_UserSign = m_CountPlayers == 0  ? 'X' : 'O';
            m_IsComputer = i_IsComputer;
            m_PlayerName = !m_IsComputer  ? $"Player {m_CountPlayers + 1}" : "Computer";
            if (i_IsComputer) 
            {
                m_AIComputer = new AIComputer();
            }
            //m_CountPlayers = (m_CountPlayers + 1) % 2;

            m_CountPlayers++;
            if(m_CountPlayers >= 2)
            {
                m_CountPlayers = 0;
            }
        }

        public void IncreaseWinsCounter()
        {
            ++PlayerWinsCounter;
        }

        private bool checkPlayerChoice(int i_Col)
        {

            return true;
        }

        public void GetComputerChoice(ref Board i_Board, out int o_Row, out int o_Column)
        {
            double winningPercentage;
           
            o_Column = m_AIComputer.minimax(i_Board, -1, -1,  'O',  4,  double.NegativeInfinity,  double.PositiveInfinity,  true,  out winningPercentage);
            o_Row = i_Board.GetRowByPlayerColumnChoice(o_Column);
            i_Board.MakeMove(o_Column, 'O');
        }



    }
}

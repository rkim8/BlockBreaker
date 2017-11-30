using System;
using System.Windows.Forms;

namespace EECEBlockBreaker
{
#if WINDOWS || XBOX
    static class Program
    {
        /// 
        /// The main entry point for the application.
        /// 
        static void Main(string[] args)
        {
            try
            {
                using (Game game = new Game())
                {
                    game.Run();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n" + e.StackTrace );
            }
        }
    }
#endif
}


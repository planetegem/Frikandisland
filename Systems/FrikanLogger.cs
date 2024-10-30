namespace Frikandisland.Systems
{
    // GameLogger manages error logging: whether in .txt file or VS Debug screen 
    internal sealed class FrikanLogger
    {
        // SINGLETON DESIGN PATTERN
        // Get instance, with padlock for thread safety
        private static FrikanLogger instance;
        private static readonly object instanceLock = new object();
        public static FrikanLogger Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (instanceLock)
                    {
                        if (instance == null)
                        {
                            instance = new FrikanLogger ();
                        }
                    }
                }
                return instance;
            }
        }
        // Private constructor to avoid creating new instances
        private FrikanLogger() { }

        // ERROR LOGGING
        // Extremely simple for now
        public static void Write(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}

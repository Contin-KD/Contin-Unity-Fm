namespace TGame
{
    public static class IDGenerator
    {
        private static long currentInstanceID;
        public static long CurrentInstanceID() { return currentInstanceID; }
        public static void ResetInstanceID()
        {
            currentInstanceID = 0;
        }
        public static long NewInstanceID()
        {
            return ++currentInstanceID;
        }

        private static long currentID;
        public static long CurrentID() { return currentID; }
        public static void ResetID()
        {
            currentID = 0;
        }
        public static void SetID(long currentID)
        {
            IDGenerator.currentID = currentID;
        }
        public static long NewID()
        {
            return ++currentID;
        }
    }
}
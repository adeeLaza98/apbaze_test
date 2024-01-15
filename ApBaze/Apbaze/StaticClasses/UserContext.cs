namespace Apbaze.StaticClasses
{
    public static class UserContext
    {
        private static int loggedInUserId;

        public static int LoggedInUserId
        {
            get { return loggedInUserId; }
            set { loggedInUserId = value; }
        }

        private static string loggedInUsername;

        public static string LoggedInUsername
        {
            get { return loggedInUsername; }
            set { loggedInUsername = value; }
        }
    }
}

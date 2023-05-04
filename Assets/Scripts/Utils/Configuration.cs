namespace Utils
{
    public class Configuration
    {
        public const int DEAD_POOL_MAX_SIZ = 200;
        public const int INITIAL_POOL_SIZE = 50;

        public const int UI_MESSAGE_TIMER = 3000;

        public const int REMOVE_DISTANCE = 60;
        public const int ADD_DISTANCE = 30;

        public const float HEAD_THRESHOLD_CAMERAS_POSITION = 5f;
        public const float TAIL_THRESHOLD_CAMERAS_POSITION = 5f;
        public const float SWIPE_ELASTIC = 5f;
        public const float STABILIZER_DISTANCE = 10f;

        public const float DISTANCE_BETWEEN_BOXES = 1.2f;
        public const float SPEED_MOVE = 1000f;
        
        // --- Disk params 
        public const int GROUP_SIZE = 100;
        public const string GROUP_PREFIX = "data_";
        public const bool DISABLE_SAVE_TO_DISK = false;
    }
}
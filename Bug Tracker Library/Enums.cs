using System;
using System.Collections.Generic;
using System.Text;

namespace Bug_Tracker_Library
{
    public enum UserPosition
    {
        ADMIN,
        MANAGER,
        WORKER
    }

    public enum ProjectStatus
    {
        TODO,
        IN_PROGRESS,
        ABANDONED,
        FINISHED
    }

    public enum ProjectPriority
    {
        BOTTOM,
        LOW,
        MEDIUM,
        HIGH,
        TOP
    }
}

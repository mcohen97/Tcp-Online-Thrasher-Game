using System;

namespace ActionResults
{
    [Serializable]
    public abstract class ActionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}

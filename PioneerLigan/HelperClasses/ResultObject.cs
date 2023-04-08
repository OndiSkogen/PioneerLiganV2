namespace PioneerLigan.HelperClasses
{
    public class ResultObject
    {
        public int Id { get; set; } = 0;
        public int Result { get; set; } = 0;
        public bool CountThis { get; set; } = false;
        public bool PlayedEvent { get; set; } = true;

        public ResultObject(int id, int res, bool countThis, bool playedEvent)
        {
            Id = id;
            Result = res;
            CountThis = countThis;
            PlayedEvent = playedEvent;
        }
    }
}

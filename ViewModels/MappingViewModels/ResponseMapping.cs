namespace CreditVillageBackend
{
    public abstract class ResponseMapping<T>
    {
        public string Status { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }
}
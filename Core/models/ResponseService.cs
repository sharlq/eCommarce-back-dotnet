namespace Core.models
{
    public class ResponseService<T>
    {
        public T Data { get; set; }

        public Pagination pagination { get; set; }
    }
}

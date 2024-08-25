namespace Ghb.Psicossoma.Webapp.Models.ResultModel
{
    public class ResultModel<T> where T : class
    {
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

        public bool? WasExecuted { get; set; }

        public int? CurrentPage { get; set; }

        public int? PageSize { get; set; }

        public long? TotalItems { get; set; }

        public string Message { get; set; } = string.Empty;

        public Exception? Exception { get; set; } = null;

        public bool HasError { get; set; }

        public int ResponseCode { get; set; }

        public TimeSpan ElapsedTime { get; set; }

        public int? TotalPages { get; set; }

        public bool? HasPreviousPage { get; set; }

        public bool? HasNextPage { get; set; }
    }
}

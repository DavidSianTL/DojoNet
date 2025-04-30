namespace _Evaluacion_Mensual_Abril.Models
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}


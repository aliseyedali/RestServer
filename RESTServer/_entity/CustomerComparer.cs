namespace RESTServer
{
    public class CustomerComparer : IComparer<Customer>
    {
        public int Compare(Customer? x, Customer? y)
        {
            return x.CompareTo(y);
        }
    }
}

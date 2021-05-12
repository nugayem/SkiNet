namespace Core.Specification
{
    public class ProductSpecParams
    {
        private const int MaxPageSize=50;
        public int PageIndex{get;set;}=1;
        private int _pageSize=6;
        public int PageSize
        {
            get=> _pageSize;
            set=> _pageSize=(value>MaxPageSize? 50:value);
        }
        public int? BrandId {get;set;}
        public int? TypeId {get;set;}
        public string Sort{get;set;}
        
        private string _serach;
        public string Search
        {
            get { return _serach; }
            set { _serach = value.ToLower(); }
        }
        

    }
}
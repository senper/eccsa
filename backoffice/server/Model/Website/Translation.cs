namespace ECCSA.BackOffice.Server.Model.Website
{
    public class Translation
    {
        private string _key;
        public string Key
        {
            get { return _key; }
            set { _key = value.Replace(" ", string.Empty).ToLowerInvariant(); }
        }

        public string Default { get; set; }
        public string Local { get; set; }
    }
}
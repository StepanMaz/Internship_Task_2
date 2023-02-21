namespace Configuration
{
    public interface IConfig
    {
        public bool IsValidSecret(string secret);
    }
}
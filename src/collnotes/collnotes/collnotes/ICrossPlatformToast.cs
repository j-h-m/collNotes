namespace collnotes
{
    // interface for cross platform toast-like feedback
    public interface ICrossPlatformToast
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }
}

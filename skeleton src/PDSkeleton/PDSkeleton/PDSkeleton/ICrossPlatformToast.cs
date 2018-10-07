namespace PDSkeleton
{
    // interface for cross platform toast-like feedback
    interface ICrossPlatformToast
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }
}

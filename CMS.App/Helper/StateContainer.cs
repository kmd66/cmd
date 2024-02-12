public class StateContainer
{
    private string? savedString;

    //public string Property
    //{
    //    get => savedString ?? string.Empty;
    //    set
    //    {
    //        savedString = value;
    //        NotifyStateChanged();
    //    }
    //}
    public void SetShopCount()
    {
        NotifySetShopCount();
    }

    public event Action? OnChangeShopCount;

    private void NotifySetShopCount() => OnChangeShopCount?.Invoke();
}
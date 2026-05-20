namespace StockReplenishment.Api.Services;

public interface IStockCheckService
{
    Task RunCheckAsync(int requestId);
}

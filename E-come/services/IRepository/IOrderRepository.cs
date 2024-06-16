using E_come.DTO.OrderDTO;
using E_come.Model;

namespace E_come.services
{
    public interface IOrderRepository
    {
        Task<string> CheckOut(string userId);
        Task<List<OrderDTO>> ViewOrders(string userId);
        Task<string> CancelOrder(string userId, string orderN);
        Task<string> TrackOrder(string userId, string orderN);
    }
}

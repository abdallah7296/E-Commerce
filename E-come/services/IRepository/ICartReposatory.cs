using E_come.DTO.CartDTO;

namespace E_come.services
{
    public interface ICartReposatory
    {
        Task<string> AddToCard(int productID, string userId, int quantity);
        Task<IEnumerable<CartDTO>> ViewCard(string userId);
        Task<string> TotalPayment(string userId);

        Task<string> RemoveFromCard(int productID, string userId, int quantity);
    }
}

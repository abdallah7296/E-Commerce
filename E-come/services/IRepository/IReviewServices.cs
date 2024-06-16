using E_come.DTO.ReviewDTO;

namespace E_come.services.IRepository
{
    public interface IReviewServices
    {
        string CreateReview(int Id, ReviewDTo dTOReview, string userid);
        void DeleteReview(int id);
        void UpdateReview(int id, ReviewDTo dto);
        List<ReviewDTo> GetAllReviews(int ProductId);
    }
}

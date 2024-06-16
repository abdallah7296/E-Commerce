using E_come.DTO.ReviewDTO;
using E_come.Model;
using E_come.services.IRepository;

namespace E_come.services
{
    public class ReviewServices : IReviewServices
    {
        private readonly DBContext context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReviewServices(DBContext _context, IWebHostEnvironment webHostEnvironment)
        {
            context = _context;
            _webHostEnvironment = webHostEnvironment;
        }
        public string CreateReview(int ProductId, ReviewDTo dTOReview,string userid)
        {
            //  Product product = context.products.SingleOrDefault(d => d.Id == ProductId);
           if(userid==null)
                return "You cannot add review to product you must be login ";

            Review review = new Review();
            review.Rate = dTOReview.Rating;
            review.Comment = dTOReview.Comment;
            review.UserId = userid;
            review.ProductId = ProductId;
            context.reviews.Add(review);
            context.SaveChanges();
            return "your review added successfully";
        }

        public void DeleteReview(int id)
        {

            Review review = context.reviews.FirstOrDefault(r => r.Id == id);
            context.reviews.Remove(review);
            context.SaveChanges();
           
        }

        public List<ReviewDTo> GetAllReviews(int ProductId)
        {
            throw new NotImplementedException();
        }

        public void UpdateReview(int id, ReviewDTo dto)
        {
            throw new NotImplementedException();
        }
    }
}

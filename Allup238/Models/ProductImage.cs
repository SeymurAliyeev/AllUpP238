namespace AllUpMVC.Models
{
    public class ProductImage : BaseEntity
    {
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public bool? IsPoster { get; set; } // true Poster False BackPoster null Detail

        public Product Product { get; set; }
    }
}

namespace AllUpMVC.Models
{
    public class Basketitem: BaseEntity
    {
        public string UserId {  get; set; }
        public int ProductId { get; set; }
        public int Count { get; set;}
        public Product Product { get; set; }
        public AppUser AppUser { get; set; }
        
    }
}

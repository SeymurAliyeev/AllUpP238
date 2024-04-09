using AllUpMVC.Models;
using AllupP238.Models;

namespace AllupP238.Models
{
    public class Basketitem: BaseEntity
    {
        public string UserId {  get; set; }
        public int ProductId { get; set; }
        public int Count { get; set;}
        public AllUpMVC.Models.Product Product { get; set; }
        public AppUser AppUser { get; set; }
        
    }
}

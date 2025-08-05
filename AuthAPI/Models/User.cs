namespace AuthAPI.Models
{
   public class User
   {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        
   }
}
/*
    ORM için public tanımlanmalılar çünkü:
    EF core bu sınıfı reflection ile tarar.
    Sadece public property'leri görüp okur ve yazar.
    private olması durumun EF bunları veritabanına yansıtamaz ve sütun oluşturamazdı.
    ASP.Net Web API kullanırken HTTP'den gelen JSON verileri de bu public alanlara deseralize edilir.

 */
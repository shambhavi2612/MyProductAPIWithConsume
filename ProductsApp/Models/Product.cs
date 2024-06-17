namespace ProductsApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public byte[] PhotoData { get; set; }

        
        public string GetBase64Image()
        {
            if (PhotoData == null || PhotoData.Length == 0)
            {
                return null;
            }

            return $"data:image/jpeg;base64,{Convert.ToBase64String(PhotoData)}";
        }





    }
}

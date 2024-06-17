namespace ConsumeProducts.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        // Add a byte array field to hold the image data
        public byte[] PhotoData { get; set; }

        // Optional: Add a method to convert the byte array to a base64 string for displaying in HTML
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

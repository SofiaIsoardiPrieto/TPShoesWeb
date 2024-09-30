namespace TPShoes.Entidades.Clases
{
	public class Brand : ICloneable
	{
		public int BrandId { get; set; }
		public string BrandName { get; set; } = null!;
		public bool Active { get; set; } = true;
		public string? ImageUrl { get; set; }
		public ICollection<Shoe> Shoes { get; set; } = new List<Shoe>();

		public object Clone()
		{
			return MemberwiseClone();
		}
	}
}

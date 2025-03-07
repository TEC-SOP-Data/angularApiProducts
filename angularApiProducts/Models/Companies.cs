namespace angularApiProducts.Models
{
    public class Companies
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string EducationalBranch { get; set; } = string.Empty;
        public string Homepage { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public int? CompanySize { get; set; }


    }
}

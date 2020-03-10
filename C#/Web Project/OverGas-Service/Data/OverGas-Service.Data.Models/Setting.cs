namespace OverGas-Service.Data.Models
{
    using OverGas-Service.Data.Common.Models;

    public class Setting : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}

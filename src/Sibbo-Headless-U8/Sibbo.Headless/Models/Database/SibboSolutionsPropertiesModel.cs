using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Sibbo.Headless.Models.Database
{
    [TableName("SibboSolutionsProperties")]
    [PrimaryKey("Id", AutoIncrement = true)]
    [ExplicitColumns]
    public class SibboSolutionsPropertiesModel
    {
        [PrimaryKeyColumn(AutoIncrement = true, IdentitySeed = 1)]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Values")]
        [SpecialDbType(SpecialDbTypes.NTEXT)]
        public string Value { get; set; }
    }
}

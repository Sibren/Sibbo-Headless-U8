using Sibbo.Headless.Models.Database;
using Umbraco.Core.Migrations;

namespace Sibbo.Headless.Migrations
{
    public class Migration1 : MigrationBase
    {
        public Migration1(IMigrationContext context) : base(context)
        { }

        public override void Migrate()
        {
            Logger.Debug(typeof(Migration1), "Running migration SibboSolutionsPropertiesModel");

            // Lots of methods available in the MigrationBase class - discover with this.
            if (TableExists("SibboSolutionsProperties") == false)
            {
                Create.Table<SibboSolutionsPropertiesModel>().Do();
            }
            else
            {
                Logger.Debug(typeof(Migration1), "The database table {DbTable} already exists, skipping", "SibboSolutionsPropertiesModel");
            }
        }
    }
}

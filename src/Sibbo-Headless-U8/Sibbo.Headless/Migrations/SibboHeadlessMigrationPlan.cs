using Umbraco.Core.Migrations;

namespace Sibbo.Headless.Migrations
{
    public class SibboHeadlessMigrationPlan : MigrationPlan
    {
        public SibboHeadlessMigrationPlan() : base("Sibbo.Headless")
        {
            From(string.Empty).To<Migration1>("first-migration");
        }
    }
}

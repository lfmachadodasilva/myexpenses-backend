namespace MyExpenses.Models
{
    public static class ModelConstants
    {
        public const string TableUser = "User";
        public const string TableGroup = "Group";
        public const string TableGroupUser = "GroupUser";
        public const string TableLabel = "Label";
        public const string TableExpense = "Expense";

        public const string ForeignKeyGroup = "GroupId";
        public const string ForeignKeyLabel = "LabelId";
    }
}

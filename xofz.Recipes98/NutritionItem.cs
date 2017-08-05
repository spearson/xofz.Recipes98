namespace xofz.Recipes98
{
    using System;

    public class NutritionItem
    {
        public NutritionItem()
        {
        }

        public NutritionItem(NutritionItem parent)
        {
            if (parent?.parent != default(NutritionItem))
            {
                throw new InvalidOperationException(
                    "A NutritionItem may only have one set of sub-items."
                    + Environment.NewLine
                    + "If deeper levels are requested, I should be able"
                    + " to make this change");
            }

            this.parent = parent;
        }

        public virtual string Label { get; set; }

        public virtual string Value { get; set; }

        public virtual string PercentDailyValue { get; set; }

        public virtual MaterializedEnumerable<NutritionItem> SubItems
        {
            get => this.subItems;

            set
            {
                if (value != default(MaterializedEnumerable<NutritionItem>))
                {
                    foreach (var subItem in value)
                    {
                        if (subItem.subItems !=
                            default(MaterializedEnumerable<NutritionItem>))
                        {
                            throw new InvalidOperationException(
                                "A NutritionItem may only have one set of sub-items."
                                + Environment.NewLine
                                + "If deeper levels are requested, I should be able"
                                + " to make this change");
                        }
                    }
                }

                this.subItems = value;
            }
        }

        private MaterializedEnumerable<NutritionItem> subItems;
        private readonly NutritionItem parent;
    }
}

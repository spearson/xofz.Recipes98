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

        public virtual Lot<NutritionItem> SubItems
        {
            get => this.subItems;

            set
            {
                if (value != default(Lot<NutritionItem>))
                {
                    foreach (var subItem in value)
                    {
                        if (subItem.subItems !=
                            default(Lot<NutritionItem>))
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

        private Lot<NutritionItem> subItems;
        private readonly NutritionItem parent;
    }
}

namespace xofz.Recipes98.UI.Forms
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Windows.Forms;
    using xofz.UI.Forms;

    public partial class UserControlNutritionalInfoUi : UserControlUi, NutritionalInfoUi
    {
        public UserControlNutritionalInfoUi(Materializer materializer)
        {
            this.materializer = materializer;

            this.InitializeComponent();

            var h = this.Handle;
        }

        public event Action LookupKeyTapped;

        public event Action EditKeyTapped;

        public event Action SaveKeyTapped;

        public event Action ResetKeyTapped;

        NutritionalInfo NutritionalInfoUi.Info
        {
            get
            {
                var info = new NutritionalInfo();
                info.ServingSize = this.servingSizeTextBox.Text;
                info.TotalServings = this.totalServingsTextBox.Text;
                info.Calories = this.caloriesTextBox.Text;
                info.CaloriesFromFat = this.fatCaloriesTextBox.Text;
                this.getFat(info);
                this.getCholesterol(info);
                this.getSodium(info);
                this.getPotassium(info);
                this.getCarbs(info);
                this.getProtein(info);
                this.getVitamins(info);

                return info;
            }

            set
            {
                this.servingSizeTextBox.Text = value?.ServingSize;
                this.totalServingsTextBox.Text = value?.TotalServings;
                this.caloriesTextBox.Text = value?.Calories;
                this.fatCaloriesTextBox.Text = value?.CaloriesFromFat;
                this.setFat(value);
                this.setCholesterol(value);
                this.setSodium(value);
                this.setPotassium(value);
                this.setCarbs(value);
                this.setProtein(value);
                this.setVitamins(value);
            }
        }

        private void getFat(NutritionalInfo info)
        {
            info.Fat = new NutritionItem();
            info.Fat.Label = "Total Fat";
            info.Fat.Value = this.totalFatValueTextBox.Text;
            info.Fat.PercentDailyValue = this.totalFatPdvTextBox.Text;
            var subItems = new[]
            {
                new NutritionItem(info.Fat)
                {
                    Label = this.fatSubItem1LabelLabel.Text,
                    Value = this.fatSubItem1ValueTextBox.Text,
                    PercentDailyValue = this.fatSubItem1PdvTextBox.Text
                },
                new NutritionItem(info.Fat)
                {
                    Label = this.fatSubItem2LabelLabel.Text,
                    Value = this.fatSubItem2ValueTextBox.Text,
                    PercentDailyValue = this.fatSubItem2PdvTextBox.Text
                },
                new NutritionItem(info.Fat)
                {
                    Label = this.fatSubItem3LabelLabel.Text,
                    Value = this.fatSubItem3ValueTextBox.Text,
                    PercentDailyValue = this.fatSubItem3PdvTextBox.Text
                },
                new NutritionItem(info.Fat)
                {
                    Label = this.fatSubItem4LabelLabel.Text,
                    Value = this.fatSubItem4ValueTextBox.Text,
                    PercentDailyValue = this.fatSubItem4PdvTextBox.Text
                },
            };

            info.Fat.SubItems = this.materializer.Materialize(subItems);
        }

        private void getCholesterol(NutritionalInfo info)
        {
            info.Cholesterol = new NutritionItem();
            info.Cholesterol.Label = "Cholesterol";
            info.Cholesterol.Value = this.cholesterolValueTextBox.Text;
            info.Cholesterol.PercentDailyValue 
                = this.cholesterolPdvTextBox.Text;
        }

        private void getSodium(NutritionalInfo info)
        {
            info.Sodium = new NutritionItem();
            info.Sodium.Label = "Sodium";
            info.Sodium.Value = this.sodiumValueTextBox.Text;
            info.Sodium.PercentDailyValue = this.sodiumPdvTextBox.Text;
        }

        private void getPotassium(NutritionalInfo info)
        {
            info.Potassium = new NutritionItem();
            info.Potassium.Label = "Potassium";
            info.Potassium.Value = this.potassiumValueTextBox.Text;
            info.Potassium.PercentDailyValue
                = this.potassiumPdvTextBox.Text;
        }

        private void getCarbs(NutritionalInfo info)
        {
            info.Carbohydrates = new NutritionItem();
            info.Carbohydrates.Label = "Total Carbohydrates";
            info.Carbohydrates.Value = this.totalCarbsValueTextBox.Text;
            info.Carbohydrates.PercentDailyValue
                = this.totalCarbsPdvTextBox.Text;
            var subItems = new[]
            {
                new NutritionItem(info.Carbohydrates)
                {
                    Label = this.carbsSubItem1LabelTextBox.Text,
                    Value = this.carbsSubItem1ValueTextBox.Text,
                    PercentDailyValue = this.carbsSubItem1PdvTextBox.Text
                },
                new NutritionItem(info.Carbohydrates)
                {
                    Label = this.carbsSubItem2LabelTextBox.Text,
                    Value = this.carbsSubItem2ValueTextBox.Text,
                    PercentDailyValue = this.carbsSubItem2PdvTextBox.Text
                },
                new NutritionItem(info.Carbohydrates)
                {
                    Label = this.carbsSubItem3LabelTextBox.Text,
                    Value = this.carbsSubItem3ValueTextBox.Text,
                    PercentDailyValue = this.carbsSubItem3PdvTextBox.Text
                },
                new NutritionItem(info.Carbohydrates)
                {
                    Label = this.carbsSubItem4LabelTextBox.Text,
                    Value = this.carbsSubItem4ValueTextBox.Text,
                    PercentDailyValue = this.carbsSubItem4PdvTextBox.Text
                },
            };

            info.Carbohydrates.SubItems 
                = this.materializer.Materialize(subItems);
        }

        private void getProtein(NutritionalInfo info)
        {
            info.Protein = new NutritionItem();
            info.Protein.Label = "Protein";
            info.Protein.Value = this.proteinValueTextBox.Text;
            info.Protein.PercentDailyValue = this.proteinPdvTextBox.Text;
        }

        private void getVitamins(NutritionalInfo info)
        {
            info.VitaminsAndMinerals = new NutritionItem();
            var subItems = new[]
            {
                new NutritionItem(info.VitaminsAndMinerals)
                {
                    Label = this.vitaminsSubItem1LabelTextBox.Text,
                    Value = this.vitaminsSubItem1ValueTextBox.Text,
                    PercentDailyValue = this.vitaminsSubItem1PdvTextBox.Text
                },
                new NutritionItem(info.VitaminsAndMinerals)
                {
                    Label = this.vitaminsSubItem2LabelTextBox.Text,
                    Value = this.vitaminsSubItem2ValueTextBox.Text,
                    PercentDailyValue = this.vitaminsSubItem2PdvTextBox.Text
                },
                new NutritionItem(info.VitaminsAndMinerals)
                {
                    Label = this.vitaminsSubItem3LabelTextBox.Text,
                    Value = this.vitaminsSubItem3ValueTextBox.Text,
                    PercentDailyValue = this.vitaminsSubItem3PdvTextBox.Text
                },
                new NutritionItem(info.VitaminsAndMinerals)
                {
                    Label = this.vitaminsSubItem4LabelTextBox.Text,
                    Value = this.vitaminsSubItem4ValueTextBox.Text,
                    PercentDailyValue = this.vitaminsSubItem4PdvTextBox.Text
                },
                new NutritionItem(info.VitaminsAndMinerals)
                {
                    Label = this.vitaminsSubItem5LabelTextBox.Text,
                    Value = this.vitaminsSubItem5ValueTextBox.Text,
                    PercentDailyValue = this.vitaminsSubItem5PdvTextBox.Text
                },
                new NutritionItem(info.VitaminsAndMinerals)
                {
                    Label = this.vitaminsSubItem6LabelTextBox.Text,
                    Value = this.vitaminsSubItem6ValueTextBox.Text,
                    PercentDailyValue = this.vitaminsSubItem6PdvTextBox.Text
                },
                new NutritionItem(info.VitaminsAndMinerals)
                {
                    Label = this.vitaminsSubItem7LabelTextBox.Text,
                    Value = this.vitaminsSubItem7ValueTextBox.Text,
                    PercentDailyValue = this.vitaminsSubItem7PdvTextBox.Text
                },
                new NutritionItem(info.VitaminsAndMinerals)
                {
                    Label = this.vitaminsSubItem8LabelTextBox.Text,
                    Value = this.vitaminsSubItem8ValueTextBox.Text,
                    PercentDailyValue = this.vitaminsSubItem8PdvTextBox.Text
                },
                new NutritionItem(info.VitaminsAndMinerals)
                {
                    Label = this.vitaminsSubItem9LabelTextBox.Text,
                    Value = this.vitaminsSubItem9ValueTextBox.Text,
                    PercentDailyValue = this.vitaminsSubItem9PdvTextBox.Text
                },
                new NutritionItem(info.VitaminsAndMinerals)
                {
                    Label = this.vitaminsSubItem10LabelTextBox.Text,
                    Value = this.vitaminsSubItem10ValueTextBox.Text,
                    PercentDailyValue = this.vitaminsSubItem10PdvTextBox.Text
                },
                new NutritionItem(info.VitaminsAndMinerals)
                {
                    Label = this.vitaminsSubItem11LabelTextBox.Text,
                    Value = this.vitaminsSubItem11ValueTextBox.Text,
                    PercentDailyValue = this.vitaminsSubItem11PdvTextBox.Text
                },
                new NutritionItem(info.VitaminsAndMinerals)
                {
                    Label = this.vitaminsSubItem12LabelTextBox.Text,
                    Value = this.vitaminsSubItem12ValueTextBox.Text,
                    PercentDailyValue = this.vitaminsSubItem12PdvTextBox.Text
                },
                new NutritionItem(info.VitaminsAndMinerals)
                {
                    Label = this.vitaminsSubItem13LabelTextBox.Text,
                    Value = this.vitaminsSubItem13ValueTextBox.Text,
                    PercentDailyValue = this.vitaminsSubItem13PdvTextBox.Text
                },
                new NutritionItem(info.VitaminsAndMinerals)
                {
                    Label = this.vitaminsSubItem14LabelTextBox.Text,
                    Value = this.vitaminsSubItem14ValueTextBox.Text,
                    PercentDailyValue = this.vitaminsSubItem14PdvTextBox.Text
                },
                new NutritionItem(info.VitaminsAndMinerals)
                {
                    Label = this.vitaminsSubItem15LabelTextBox.Text,
                    Value = this.vitaminsSubItem15ValueTextBox.Text,
                    PercentDailyValue = this.vitaminsSubItem15PdvTextBox.Text
                },
                new NutritionItem(info.VitaminsAndMinerals)
                {
                    Label = this.vitaminsSubItem16LabelTextBox.Text,
                    Value = this.vitaminsSubItem16ValueTextBox.Text,
                    PercentDailyValue = this.vitaminsSubItem16PdvTextBox.Text
                },
            };

            info.VitaminsAndMinerals.SubItems
                = this.materializer.Materialize(subItems);
        }

        private void setFat(NutritionalInfo info)
        {
            this.clearFatTextBoxes();
            if (info == default(NutritionalInfo))
            {
                return;
            }

            this.totalFatValueTextBox.Text = info.Fat?.Value;
            this.totalFatPdvTextBox.Text = info.Fat?.PercentDailyValue;
            if (info.Fat?.SubItems 
                == default(MaterializedEnumerable<NutritionItem>))
            {
                return;
            }

            using (var e = info.Fat.SubItems.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    return;
                }

                this.fatSubItem1ValueTextBox.Text = e.Current?.Value;
                this.fatSubItem1PdvTextBox.Text 
                    = e.Current?.PercentDailyValue;

                if (!e.MoveNext())
                {
                    return;
                }

                this.fatSubItem2ValueTextBox.Text = e.Current?.Value;
                this.fatSubItem2PdvTextBox.Text
                    = e.Current?.PercentDailyValue;

                if (!e.MoveNext())
                {
                    return;
                }

                this.fatSubItem3ValueTextBox.Text = e.Current?.Value;
                this.fatSubItem3PdvTextBox.Text
                    = e.Current?.PercentDailyValue;

                if (!e.MoveNext())
                {
                    return;
                }

                this.fatSubItem4ValueTextBox.Text = e.Current?.Value;
                this.fatSubItem4PdvTextBox.Text
                    = e.Current?.PercentDailyValue;
            }
        }

        private void clearFatTextBoxes()
        {
            this.totalFatValueTextBox.Text = null;
            this.totalFatPdvTextBox.Text = null;
            this.fatSubItem1ValueTextBox.Text = null;
            this.fatSubItem1PdvTextBox.Text = null;
            this.fatSubItem2ValueTextBox.Text = null;
            this.fatSubItem2PdvTextBox.Text = null;
            this.fatSubItem3ValueTextBox.Text = null;
            this.fatSubItem3PdvTextBox.Text = null;
            this.fatSubItem4ValueTextBox.Text = null;
            this.fatSubItem4PdvTextBox.Text = null;
        }

        private void setCholesterol(NutritionalInfo info)
        {
            this.cholesterolValueTextBox.Text = null;
            this.cholesterolPdvTextBox.Text = null;

            if (info == default(NutritionalInfo))
            {
                return;
            }

            this.cholesterolValueTextBox.Text
                = info.Cholesterol?.Value;
            this.cholesterolPdvTextBox.Text
                = info.Cholesterol?.PercentDailyValue;
        }

        private void setSodium(NutritionalInfo info)
        {
            this.sodiumValueTextBox.Text = null;
            this.sodiumPdvTextBox.Text = null;
            if (info == default(NutritionalInfo))
            {
                return;
            }

            this.sodiumValueTextBox.Text
                = info.Sodium?.Value;
            this.sodiumPdvTextBox.Text
                = info.Sodium?.PercentDailyValue;
        }

        private void setPotassium(NutritionalInfo info)
        {
            this.potassiumValueTextBox.Text = null;
            this.potassiumPdvTextBox.Text = null;
            if (info == default(NutritionalInfo))
            {
                return;
            }

            this.potassiumValueTextBox.Text
                = info.Potassium?.Value;
            this.potassiumPdvTextBox.Text
                = info.Potassium?.PercentDailyValue;
        }

        private void setCarbs(NutritionalInfo info)
        {
            this.clearCarbsTextBoxes();
            if (info == default(NutritionalInfo))
            {
                return;
            }

            this.totalCarbsValueTextBox.Text = info.Carbohydrates?.Value;
            this.totalCarbsPdvTextBox.Text 
                = info.Carbohydrates?.PercentDailyValue;
            if (info.Carbohydrates?.SubItems
                == default(MaterializedEnumerable<NutritionItem>))
            {
                return;
            }

            using (var e = info.Carbohydrates.SubItems.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    return;
                }

                this.carbsSubItem1LabelTextBox.Text = e.Current?.Label;
                this.carbsSubItem1ValueTextBox.Text = e.Current?.Value;
                this.carbsSubItem1PdvTextBox.Text
                    = e.Current?.PercentDailyValue;

                if (!e.MoveNext())
                {
                    return;
                }

                this.carbsSubItem2LabelTextBox.Text = e.Current?.Label;
                this.carbsSubItem2ValueTextBox.Text = e.Current?.Value;
                this.carbsSubItem2PdvTextBox.Text
                    = e.Current?.PercentDailyValue;

                if (!e.MoveNext())
                {
                    return;
                }

                this.carbsSubItem3LabelTextBox.Text = e.Current?.Label;
                this.carbsSubItem3ValueTextBox.Text = e.Current?.Value;
                this.carbsSubItem3PdvTextBox.Text
                    = e.Current?.PercentDailyValue;

                if (!e.MoveNext())
                {
                    return;
                }

                this.carbsSubItem4LabelTextBox.Text = e.Current?.Label;
                this.carbsSubItem4ValueTextBox.Text = e.Current?.Value;
                this.carbsSubItem4PdvTextBox.Text
                    = e.Current?.PercentDailyValue;
            }
        }

        private void clearCarbsTextBoxes()
        {
            this.totalCarbsValueTextBox.Text = null;
            this.totalCarbsPdvTextBox.Text = null;
            this.carbsSubItem1LabelTextBox.Text = null;
            this.carbsSubItem1ValueTextBox.Text = null;
            this.carbsSubItem1PdvTextBox.Text = null;
            this.carbsSubItem2LabelTextBox.Text = null;
            this.carbsSubItem2ValueTextBox.Text = null;
            this.carbsSubItem2PdvTextBox.Text = null;
            this.carbsSubItem3LabelTextBox.Text = null;
            this.carbsSubItem3ValueTextBox.Text = null;
            this.carbsSubItem3PdvTextBox.Text = null;
            this.carbsSubItem4LabelTextBox.Text = null;
            this.carbsSubItem4ValueTextBox.Text = null;
            this.carbsSubItem4PdvTextBox.Text = null;
        }

        private void setProtein(NutritionalInfo info)
        {
            this.proteinValueTextBox.Text = null;
            this.proteinPdvTextBox.Text = null;
            if (info == default(NutritionalInfo))
            {
                return;
            }

            this.proteinValueTextBox.Text
                = info.Protein?.Value;
            this.proteinPdvTextBox.Text
                = info.Protein?.PercentDailyValue;
        }

        private void setVitamins(NutritionalInfo info)
        {
            this.clearVitaminsTextBoxes();

            if (info == default(NutritionalInfo))
            {
                return;
            }

            if (info.VitaminsAndMinerals?.SubItems
                == default(MaterializedEnumerable<NutritionItem>))
            {
                return;
            }

            using (var e = info.VitaminsAndMinerals
                .SubItems.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    return;
                }

                this.vitaminsSubItem1LabelTextBox.Text = e.Current?.Label;
                this.vitaminsSubItem1ValueTextBox.Text = e.Current?.Value;
                this.vitaminsSubItem1PdvTextBox.Text
                    = e.Current?.PercentDailyValue;

                if (!e.MoveNext())
                {
                    return;
                }

                this.vitaminsSubItem2LabelTextBox.Text = e.Current?.Label;
                this.vitaminsSubItem2ValueTextBox.Text = e.Current?.Value;
                this.vitaminsSubItem2PdvTextBox.Text
                    = e.Current?.PercentDailyValue;

                if (!e.MoveNext())
                {
                    return;
                }

                this.vitaminsSubItem3LabelTextBox.Text = e.Current?.Label;
                this.vitaminsSubItem3ValueTextBox.Text = e.Current?.Value;
                this.vitaminsSubItem3PdvTextBox.Text
                    = e.Current?.PercentDailyValue;

                if (!e.MoveNext())
                {
                    return;
                }

                this.vitaminsSubItem4LabelTextBox.Text = e.Current?.Label;
                this.vitaminsSubItem4ValueTextBox.Text = e.Current?.Value;
                this.vitaminsSubItem4PdvTextBox.Text
                    = e.Current?.PercentDailyValue;

                if (!e.MoveNext())
                {
                    return;
                }

                this.vitaminsSubItem5LabelTextBox.Text = e.Current?.Label;
                this.vitaminsSubItem5ValueTextBox.Text = e.Current?.Value;
                this.vitaminsSubItem5PdvTextBox.Text
                    = e.Current?.PercentDailyValue;

                if (!e.MoveNext())
                {
                    return;
                }

                this.vitaminsSubItem6LabelTextBox.Text = e.Current?.Label;
                this.vitaminsSubItem6ValueTextBox.Text = e.Current?.Value;
                this.vitaminsSubItem6PdvTextBox.Text
                    = e.Current?.PercentDailyValue;

                if (!e.MoveNext())
                {
                    return;
                }

                this.vitaminsSubItem7LabelTextBox.Text = e.Current?.Label;
                this.vitaminsSubItem7ValueTextBox.Text = e.Current?.Value;
                this.vitaminsSubItem7PdvTextBox.Text
                    = e.Current?.PercentDailyValue;

                if (!e.MoveNext())
                {
                    return;
                }

                this.vitaminsSubItem8LabelTextBox.Text = e.Current?.Label;
                this.vitaminsSubItem8ValueTextBox.Text = e.Current?.Value;
                this.vitaminsSubItem8PdvTextBox.Text
                    = e.Current?.PercentDailyValue;

                if (!e.MoveNext())
                {
                    return;
                }

                this.vitaminsSubItem9LabelTextBox.Text = e.Current?.Label;
                this.vitaminsSubItem9ValueTextBox.Text = e.Current?.Value;
                this.vitaminsSubItem9PdvTextBox.Text
                    = e.Current?.PercentDailyValue;

                if (!e.MoveNext())
                {
                    return;
                }

                this.vitaminsSubItem10LabelTextBox.Text = e.Current?.Label;
                this.vitaminsSubItem10ValueTextBox.Text = e.Current?.Value;
                this.vitaminsSubItem10PdvTextBox.Text
                    = e.Current?.PercentDailyValue;

                if (!e.MoveNext())
                {
                    return;
                }

                this.vitaminsSubItem11LabelTextBox.Text = e.Current?.Label;
                this.vitaminsSubItem11ValueTextBox.Text = e.Current?.Value;
                this.vitaminsSubItem11PdvTextBox.Text
                    = e.Current?.PercentDailyValue;

                if (!e.MoveNext())
                {
                    return;
                }

                this.vitaminsSubItem12LabelTextBox.Text = e.Current?.Label;
                this.vitaminsSubItem12ValueTextBox.Text = e.Current?.Value;
                this.vitaminsSubItem12PdvTextBox.Text
                    = e.Current?.PercentDailyValue;

                if (!e.MoveNext())
                {
                    return;
                }

                this.vitaminsSubItem13LabelTextBox.Text = e.Current?.Label;
                this.vitaminsSubItem13ValueTextBox.Text = e.Current?.Value;
                this.vitaminsSubItem13PdvTextBox.Text
                    = e.Current?.PercentDailyValue;

                if (!e.MoveNext())
                {
                    return;
                }

                this.vitaminsSubItem14LabelTextBox.Text = e.Current?.Label;
                this.vitaminsSubItem14ValueTextBox.Text = e.Current?.Value;
                this.vitaminsSubItem14PdvTextBox.Text
                    = e.Current?.PercentDailyValue;

                if (!e.MoveNext())
                {
                    return;
                }

                this.vitaminsSubItem15LabelTextBox.Text = e.Current?.Label;
                this.vitaminsSubItem15ValueTextBox.Text = e.Current?.Value;
                this.vitaminsSubItem15PdvTextBox.Text
                    = e.Current?.PercentDailyValue;

                if (!e.MoveNext())
                {
                    return;
                }

                this.vitaminsSubItem16LabelTextBox.Text = e.Current?.Label;
                this.vitaminsSubItem16ValueTextBox.Text = e.Current?.Value;
                this.vitaminsSubItem16PdvTextBox.Text
                    = e.Current?.PercentDailyValue;
            }
        }

        private void clearVitaminsTextBoxes()
        {
            this.vitaminsSubItem1LabelTextBox.Text = null;
            this.vitaminsSubItem1ValueTextBox.Text = null;
            this.vitaminsSubItem1PdvTextBox.Text = null;

            this.vitaminsSubItem2LabelTextBox.Text = null;
            this.vitaminsSubItem2ValueTextBox.Text = null;
            this.vitaminsSubItem2PdvTextBox.Text = null;

            this.vitaminsSubItem3LabelTextBox.Text = null;
            this.vitaminsSubItem3ValueTextBox.Text = null;
            this.vitaminsSubItem3PdvTextBox.Text = null;

            this.vitaminsSubItem4LabelTextBox.Text = null;
            this.vitaminsSubItem4ValueTextBox.Text = null;
            this.vitaminsSubItem4PdvTextBox.Text = null;

            this.vitaminsSubItem5LabelTextBox.Text = null;
            this.vitaminsSubItem5ValueTextBox.Text = null;
            this.vitaminsSubItem5PdvTextBox.Text = null;

            this.vitaminsSubItem6LabelTextBox.Text = null;
            this.vitaminsSubItem6ValueTextBox.Text = null;
            this.vitaminsSubItem6PdvTextBox.Text = null;

            this.vitaminsSubItem7LabelTextBox.Text = null;
            this.vitaminsSubItem7ValueTextBox.Text = null;
            this.vitaminsSubItem7PdvTextBox.Text = null;

            this.vitaminsSubItem8LabelTextBox.Text = null;
            this.vitaminsSubItem8ValueTextBox.Text = null;
            this.vitaminsSubItem8PdvTextBox.Text = null;

            this.vitaminsSubItem9LabelTextBox.Text = null;
            this.vitaminsSubItem9ValueTextBox.Text = null;
            this.vitaminsSubItem9PdvTextBox.Text = null;

            this.vitaminsSubItem10LabelTextBox.Text = null;
            this.vitaminsSubItem10ValueTextBox.Text = null;
            this.vitaminsSubItem10PdvTextBox.Text = null;

            this.vitaminsSubItem11LabelTextBox.Text = null;
            this.vitaminsSubItem11ValueTextBox.Text = null;
            this.vitaminsSubItem11PdvTextBox.Text = null;

            this.vitaminsSubItem12LabelTextBox.Text = null;
            this.vitaminsSubItem12ValueTextBox.Text = null;
            this.vitaminsSubItem12PdvTextBox.Text = null;
            
            this.vitaminsSubItem13LabelTextBox.Text = null;
            this.vitaminsSubItem13ValueTextBox.Text = null;
            this.vitaminsSubItem13PdvTextBox.Text = null;

            this.vitaminsSubItem14LabelTextBox.Text = null;
            this.vitaminsSubItem14ValueTextBox.Text = null;
            this.vitaminsSubItem14PdvTextBox.Text = null;

            this.vitaminsSubItem15LabelTextBox.Text = null;
            this.vitaminsSubItem15ValueTextBox.Text = null;
            this.vitaminsSubItem15PdvTextBox.Text = null;

            this.vitaminsSubItem16LabelTextBox.Text = null;
            this.vitaminsSubItem16ValueTextBox.Text = null;
            this.vitaminsSubItem16PdvTextBox.Text = null;

        }

        string NutritionalInfoUi.LookupRecipeName
        {
            get => this.lookupNameTextBox.Text;

            set => this.lookupNameTextBox.Text = value;
        }

        string NutritionalInfoUi.MatchRecipeName
        {
            get
            {
                var split = this.mainLabel.Text.Split('-');
                if (split.Length < 2)
                {
                    return default(string);
                }
                string untrimmedName;
                if (split.Length > 2)
                {
                    var pieces = new string[split.Length - 1];
                    for (var i = 1; i < split.Length; ++i)
                    {
                        pieces[i - 1] = split[i];
                    }

                    untrimmedName = string.Join("-", pieces);
                }
                else
                {
                    untrimmedName = split[1];
                }

                return untrimmedName.Trim();
            }

            set
            {
                if (StringHelpers.NullOrWhiteSpace(value))
                {
                    this.mainLabel.Text = @"Nut.' Info";
                    return;
                }

                this.mainLabel.Text = @"Nut.'l Info - " + value;
            }
        }

        bool NutritionalInfoUi.EditKeyEnabled
        {
            get => this.editKey.Enabled;

            set => this.editKey.Enabled = value;
        }

        bool NutritionalInfoUi.SaveKeyEnabled
        {
            get => this.saveKey.Enabled;

            set => this.saveKey.Enabled = value;
        }

        bool NutritionalInfoUi.Editable
        {
            get => this.servingSizeTextBox.ReadOnly;

            set
            {
                foreach (var textBox in
                    MEHelpers.PrivateFieldsOfType<TextBox>(this))
                {
                    textBox.ReadOnly = !value;
                }
            }
        }

        private void lookupKey_Click(object sender, EventArgs e)
        {
            new Thread(() => this.LookupKeyTapped?.Invoke()).Start();
        }

        private void editKey_Click(object sender, EventArgs e)
        {
            new Thread(() => this.EditKeyTapped?.Invoke()).Start();
        }

        private void saveKey_Click(object sender, EventArgs e)
        {
            new Thread(() => this.SaveKeyTapped?.Invoke()).Start();
        }

        private void resetKey_Click(object sender, EventArgs e)
        {
            new Thread(() => this.ResetKeyTapped?.Invoke()).Start();
        }

        private readonly Materializer materializer;
    }
}

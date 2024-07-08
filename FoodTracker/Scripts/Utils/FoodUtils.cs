namespace FoodTracker.Scripts.Utils
{
    public enum Unit
    {
        grams,
        millilitres,
    }

    public enum Macros
    {
        Carb,
        Protein,
        Fat
    }

    public class Macro
    {
        public Macros macro { get; private set; }
        public float value { get; private set; }

        public Macro(Macros macro, float value)
        {
            this.macro = macro;
            this.value = value;
        }
    }

    public static class FoodUtils
    {
        public static Dictionary<Unit, string> unitToAbbreviation = new Dictionary<Unit, string>()
        {
            { Unit.grams, "g" },
            { Unit.millilitres, "ml" }
        };

        public static Dictionary<Macros, Unit> macrosToUnit = new Dictionary<Macros, Unit>()
        {
            { Macros.Carb, Unit.grams },
            { Macros.Protein, Unit.grams },
            { Macros.Fat, Unit.grams }
        };

        public static string MacroToUnitAbbv(Macros macro)
        {
            return unitToAbbreviation[macrosToUnit[macro]];
        }
    }
}

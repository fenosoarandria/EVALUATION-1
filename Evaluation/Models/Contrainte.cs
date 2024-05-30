using System.Globalization;
public class Contrainte{
    public static DateTime? ParseDate(string? value)
    {
        // Tableau de formats de date à essayer
        string[] formats = { "dd/MM/yyyy", "dd-MM-yyyy", "yyyy/MM/dd", "yyyy-MM-dd" };

        // Essayer de parser la date en utilisant plusieurs formats
        if (DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
        {
            return result;
        }
        else
        {
            // Gérer les cas où la valeur de la date n'est pas dans l'un des formats attendus
            throw new ArgumentException($"La valeur de la date '{value}' n'est pas dans un format de date valide.");
        }
    }

    public static string ApplyConstraints(string value)
    {
        // Vérifier si la valeur est null ou vide
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException($"La valeur de  {value} est null ou vide.");
        }

        // Supprimer les espaces blancs inutiles
        value = value.Trim();
        value = value.Replace(",",".");

        if (value.EndsWith("%"))
        {
            value = value.TrimEnd('%');
            value = (double.Parse(value, CultureInfo.InvariantCulture)).ToString();
        }

        // Retourner la valeur après application des contraintes
        return value;
    }

}
namespace Shared.Interactions;

public class Interaction<TSource>
{
    public TSource Source = default;

    public static Interaction<TSource> From(TSource source)
    {
        var interaction = new Interaction<TSource>()
        {
            Source = source,
        };

        return interaction;
    }
}

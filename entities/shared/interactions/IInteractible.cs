using CSharpFunctionalExtensions;
using utilities;

namespace Shared.Interactions;

public interface IInteractible
{
}

public interface IInteractible<TSource, TResult> : IInteractible
{
    public Result<TResult, GameError> Interact(Interaction<TSource> interaction);
}

namespace Sec.Edgar.Models
{
    internal class GenericModel<T>
    {
        internal GenericModel(T model)
        {
            IsValid = true;
            Model = model;
        }

        internal GenericModel()
        {
            IsValid = false;
        }

        internal T Model { get; private set; }
        internal bool IsValid { get; private set; }
    }
}
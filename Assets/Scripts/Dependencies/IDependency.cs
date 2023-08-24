namespace SF3DRacing
{
    public interface IDependency<T>
    {
        void Construct(T dependency);
    }
}

namespace Model.Shooter
{
    /// <summary>
    /// Layers are used so that collisions only happen with different layers. 
    /// Becareful with the number of layers for performance purpose.
    /// </summary>
    public enum Layer
    {
        Friend,
        Enemy
    }
}

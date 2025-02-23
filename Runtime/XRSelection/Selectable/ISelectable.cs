namespace XRSelection.Selectable
{
    public interface ISelectable
    {
        /// <summary>
        /// Identifies a selectable, so it is not selected multiple times.
        /// </summary>
        /// <returns>
        /// An Integer identifier.
        /// </returns>
        public int GetIdentifier();

        /// <summary>
        /// Called once when Selectable ist selected.
        /// </summary>
        public void Select();
    
        /// <summary>
        /// Called every frame on the hovered Selectable.
        /// </summary>
        public void Hover();
    }
}

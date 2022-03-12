namespace StoryFramework
{
    /// <summary>
    /// Base interface for all drop item targets.
    /// </summary>
    public interface IDropItemTarget
    {
        /// <summary>
        /// Called when trying to drop an item.
        /// </summary>
        /// <param name="item">Item being dropped</param>
        /// <returns>If the item was accepted</returns>
        bool TryDropItem(InventoryItem item);
    }
}
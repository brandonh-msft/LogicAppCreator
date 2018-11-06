namespace LogicAppCreator
{
    /// <summary>
    /// 
    /// </summary>
    public class RunAfter
    {
        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        /// <value>
        /// The name of the action.
        /// </value>
        public string ActionName { get; set; }
        /// <summary>
        /// Gets or sets the result mask.
        /// </summary>
        public ActionResult ResultMask { get; set; } = ActionResult.Succeeded;
    }
}
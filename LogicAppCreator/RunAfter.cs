namespace LogicAppCreator
{
    /// <summary>
    /// 
    /// </summary>
    public class RunAfter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RunAfter"/> class.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        public RunAfter(string actionName) => this.ActionName = actionName;

        /// <summary>
        /// Gets the name of the action.
        /// </summary>
        /// <value>
        /// The name of the action.
        /// </value>
        public string ActionName { get; private set; }
        /// <summary>
        /// Gets or sets the result mask.
        /// </summary>
        public ActionResult ResultMask { get; set; } = ActionResult.Succeeded;
    }
}
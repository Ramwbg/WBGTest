

namespace LOBConsole
{
    using System;
    
    /// <summary>
    /// Message for activities.
    /// </summary>
    public class ActivityMessage
    {
        /// <summary>
        /// Gets or sets the activity id.  Leave blank for new activities.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets an id supplied by the source system such as expense report "1234."
        /// </summary>
        /// <value>
        /// The source identifier.
        /// </value>
        public string SourceId { get; set; }

        /// <summary>
        /// Gets or sets the version of the activity app rendering template, such as "1.0"
        /// </summary>
        public string RenderingTemplateVersion { get; set; }

        /// <summary>
        /// Gets or sets the email of the actor that initiated the activity.
        /// </summary>
        public string ActorEmail { get; set; }

        /// <summary>
        /// Gets or sets the list of user emails the activity is directed to.  Leave blank for public activity.
        /// </summary>
        /// <value>
        /// The user emails.
        /// </value>
        public string[] DirectedToUserEmails { get; set; }
        
        /// <summary>
        /// Gets or sets the id of the activity app, such as "ExpenseReport."
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// Gets or sets custom data specific to the activity app.  The recommended format is JSON.
        /// </summary>
        public string AppData { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the activity.
        /// </summary>
        public DateTime ActivityDate { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the name of the group the activity is targeted to.
        /// </summary>
        /// <value>
        /// The name of the group.
        /// </value>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the topics.
        /// </summary>
        /// <value>
        /// The topics.
        /// </value>
        public string[] Topics { get; set; }

        /// <summary>
        /// Gets or sets the Open Graph Item.
        /// </summary>
        /// <value>
        /// The open graph item.
        /// </value>
        public OpenGraphItemMessage OpenGraphItem { get; set; }

        public ActivityTaskMessage TaskItem { get; set; }
    }
}

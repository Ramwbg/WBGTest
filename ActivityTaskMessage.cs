//-----------------------------------------------------------------------
// <copyright file="ActivityTaskMessage.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// All rights reserved.
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
// </copyright>
//-----------------------------------------------------------------------
namespace LOBConsole
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Message for activity task.
    /// </summary>
    [DataContract]
    public class ActivityTaskMessage
    {
        /// <summary>
        /// Gets or sets the task title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the task description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the task start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        [DataMember]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the task due date.
        /// </summary>
        /// <value>
        /// The due date.
        /// </value>
        [DataMember]
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Gets or sets the email of activity approver.
        /// </summary>
        /// <value>
        /// Approver's email.
        /// </value>
        [DataMember]
        public string ActorEmail { get; set; }
    }
}

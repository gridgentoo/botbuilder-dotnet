﻿// Licensed under the MIT License.
// Copyright (c) Microsoft Corporation. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Actions;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Dialogs.Adaptive.Testing
{
    /// <summary>
    /// Mock one or more property values.
    /// </summary>
    [DebuggerDisplay("SetProperties")]
    public class SetProperties : TestAction
    {
        /// <summary>
        /// Kind to serialize.
        /// </summary>
        [JsonProperty("$kind")]
        public const string Kind = "Microsoft.Test.SetProperties";

        /// <summary>
        /// Initializes a new instance of the <see cref="SetProperties"/> class.
        /// </summary>
        /// <param name="path">optional path.</param>
        /// <param name="line">optional line.</param>
        [JsonConstructor]
        public SetProperties([CallerFilePath] string path = "", [CallerLineNumber] int line = 0)
        {
            RegisterSourcePath(path, line);
        }

        /// <summary>
        /// Gets the property assignments.
        /// </summary>
        /// <value>
        /// Property assignments as property=value pairs. In first match first use order.
        /// </value>
        [JsonProperty("assignments")]
        public List<PropertyAssignment> Assignments { get; } = new List<PropertyAssignment>();

        /// <inheritdoc/>
        public async override Task ExecuteAsync(TestAdapter adapter, BotCallbackHandler callback, Inspector inspector = null)
        {
            if (inspector != null)
            {
                await inspector((dc) =>
                {
                    foreach (var assignment in Assignments)
                    {
                        dc.State.SetValue(assignment.Property.GetValue(dc.State), assignment.Value.GetValue(dc.State));
                    }
                }).ConfigureAwait(false);
                Trace.TraceInformation($"[Turn Ended => SetProperties completed]");
            }
            else
            {
                throw new Exception("No inspector to use for setting properties");
            }
        }
    }
}

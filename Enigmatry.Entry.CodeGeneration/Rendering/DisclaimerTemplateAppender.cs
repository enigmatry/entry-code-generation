﻿using System;
using System.IO;
using JetBrains.Annotations;

namespace Enigmatry.Entry.CodeGeneration.Rendering;

[UsedImplicitly]
public class DisclaimerTemplateAppender : ITemplateWriterAppender
{
    private const string Disclaimer =

        @"// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------;
/* eslint-disable */
";

    public bool AppendAtStart(string path) => Path.GetExtension(path) == ".ts";

    public bool AppendAtEnd(string path) => false;

    public string TextToAppendAtStart() => Disclaimer;

    public string TextToAppendAtEnd() => String.Empty;
}
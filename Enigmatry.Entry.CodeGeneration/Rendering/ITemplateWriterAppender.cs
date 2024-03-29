﻿namespace Enigmatry.Entry.CodeGeneration.Rendering;

public interface ITemplateWriterAppender
{
    bool AppendAtStart(string path);

    bool AppendAtEnd(string path);

    string TextToAppendAtStart();

    string TextToAppendAtEnd();
}